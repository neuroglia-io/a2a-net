// Copyright � 2025-Present the a2a-net Authors
//
// Licensed under the Apache License, Version 2.0 (the "License"),
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using A2A.Events;
using A2A.Samples.SemanticKernel.Client;

using Microsoft.VisualStudio.Threading;

using Spectre.Console.Extensions;

using System.Text;

var configuration = new ConfigurationBuilder()
    .AddCommandLine(args)
    .AddEnvironmentVariables()
    .AddUserSecrets(typeof(ApplicationOptions).Assembly)
    .AddJsonFile("appsettings.json", true)
    .Build();
var applicationOptions = new ApplicationOptions();
configuration.Bind(applicationOptions);
ArgumentNullException.ThrowIfNull(applicationOptions.Server);

using var httpClient = new HttpClient();
var discoveryDocument = await httpClient.GetA2ADiscoveryDocumentAsync(applicationOptions.Server);
var agents = discoveryDocument.Agents;
if (agents is null || agents.Count == 0)
{
    AnsiConsole.MarkupLineInterpolated($"[red]❌ No agent(s) found at {applicationOptions.Server}[/]");
    return;
}

var agent = discoveryDocument.Agents[0];

// Allow `--streaming` to be a flag or a value
if (applicationOptions.Streaming is false)
{
    var streamingArg = args.Select((v, i) => (i, v)).FirstOrDefault(i => i.v.Contains("streaming", StringComparison.OrdinalIgnoreCase));
    if (streamingArg != default)
    {
        applicationOptions.Streaming = streamingArg.i + 1 >= args.Length || !bool.TryParse(args[streamingArg.i + 1], out var b) || b;
    }
}

var services = new ServiceCollection();
if (Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT") is "Development")
{
    services.ConfigureHttpClientDefaults(b => b.ConfigureHttpClient(c => c.Timeout = TimeSpan.FromDays(1)));
}

services.AddA2AProtocolHttpClient(options =>
{
    options.Endpoint = agent.Url.IsAbsoluteUri ? agent.Url : new(applicationOptions.Server, agent.Url);
});

var provider = services.BuildServiceProvider();
var client = provider.GetRequiredService<IA2AProtocolClient>();

var agentCts = new CancellationTokenSource();
AnsiConsole.Write(new FigletText("A2A Protocol Chat").Color(Color.Blue));
AnsiConsole.MarkupLine("[gray]Type your prompts below. Press [bold]Ctrl+C[/] to exit.[/]\n");
var responseSoFar = new StringBuilder();
var session = Guid.NewGuid().ToString("N");

var agentCommSpinnerDef = AnsiConsole.Status()
            .Spinner(Spinner.Known.BouncingBar)
            .SpinnerStyle(Style.Parse("green"));
var toolSpinnerDef = AnsiConsole.Status()
    .Spinner(Spinner.Known.SquareCorners)
    .SpinnerStyle(Style.Parse("grey58"));

CancellationTokenSource spinnerCts = new();
System.Threading.Tasks.Task spinner = System.Threading.Tasks.Task.CompletedTask;
void cancelSpinner()
{
    spinnerCts.Cancel();
    try
    {
        spinner.Wait(spinnerCts.Token);
    }
    catch (Exception e) when (e is TaskCanceledException or OperationCanceledException)
    {
        // Ignore cancellation exceptions
    }
}

void spinStopThen(Action runThis)
{
    cancelSpinner();
    runThis();
}

while (true)
{
    cancelSpinner();
    var prompt = AnsiConsole.Ask<string>("[bold blue]User>[/]");
    if (string.IsNullOrWhiteSpace(prompt))
    {
        AnsiConsole.MarkupLine("[yellow]⚠️ Please enter a prompt.[/]");
        continue;
    }

    if (prompt is "/reset")
    {
        session = Guid.NewGuid().ToString("N");
        responseSoFar.Clear();

        AnsiConsole.MarkupLine("[yellow]⚠️ Chat history reset.[/]");
        continue;
    }
    else if (prompt is "/agent" or "/agents" or "/card" or "/cards")
    {
        printAgentCards();
        continue;
    }

    var filePath = AnsiConsole.Ask<string>("[blue]File path (optional, <enter> to skip)>[/]", string.Empty).TrimStart('"').TrimEnd('"');
    string? filename = !string.IsNullOrWhiteSpace(filePath) ? Path.GetFileName(filePath) : null;
    var fileBytes = !string.IsNullOrWhiteSpace(filePath) ? System.IO.File.ReadAllBytes(filePath) : null;

    spinnerCts = new();

    try
    {
        var parts = new List<Part>() { new TextPart(prompt) };
        if (!string.IsNullOrWhiteSpace(filePath))
        {
            parts.Add(new FilePart { File = new() { Bytes = Convert.ToBase64String(fileBytes!), Name = filename } });
        }

        var taskParams = new TaskSendParameters
        {
            SessionId = session,
            PushNotification = applicationOptions.PushNotificationClient is null ? null : new() { Url = applicationOptions.PushNotificationClient },
            Message = new()
            {
                Role = MessageRole.User,
                Parts = [.. parts]
            }
        };

        if (applicationOptions.Streaming is true)
        {
            spinner = agentCommSpinnerDef
                .StartAsync("Communicating with Agent...", ctx => System.Threading.Tasks.Task.Run(() => { while (true) ctx.Refresh(); }, spinnerCts.Token).WaitAsync(spinnerCts.Token));

            var request = new SendTaskStreamingRequest { Params = taskParams };

            bool firstArtifact = true;
            try
            {
                await foreach (var response in client.SendTaskStreamingAsync(request, agentCts.Token))
                {
                    if (response.Error is not null)
                    {
                        spinStopThen(() => AnsiConsole.MarkupLineInterpolated($"[red]❌ Error: {response.Error.Message}[/]"));
                        continue;
                    }

                    if (response.Result is TaskArtifactUpdateEvent artifactEvent)
                    {
                        if (firstArtifact)
                        {
                            cancelSpinner();
                            AnsiConsole.Markup($"[bold green]Agent>[/] ");
                            firstArtifact = false;
                        }
                        else if (artifactEvent.Artifact.Append is false)
                        {
                            Console.WriteLine();
                        }

                        await PrintArtifactAsync(artifactEvent.Artifact);

                        if (artifactEvent.Artifact.LastChunk is true)
                        {
                            Console.WriteLine();
                        }
                    }
                    else if (response.Result is TaskStatusUpdateEvent evt)
                    {
                        var msg = evt.Status.Message?.ToText() ?? string.Empty;

                        if (msg.Contains("ToolCalls:InProgress") is true && spinnerCts.IsCancellationRequested)
                        {
                            cancelSpinner();
                            spinnerCts = new CancellationTokenSource();
                            spinner = toolSpinnerDef.StartAsync("[grey23]Running tool...[/]", ctx => System.Threading.Tasks.Task.Run(() => { while (true) ctx.Refresh(); }, spinnerCts.Token).WaitAsync(spinnerCts.Token));

                            continue;
                        }
                        else if (msg.Contains("ToolsCalls:Completed") is true && !spinnerCts.IsCancellationRequested)
                        {
                            cancelSpinner();
                        }
                        else if (!string.IsNullOrWhiteSpace(msg))
                        {
                            spinStopThen(() => AnsiConsole.MarkupLineInterpolated($"[grey23]{msg}[/]"));
                        }

                        if (evt.Final is true)
                        {
                            spinStopThen(() => Console.WriteLine());
                        }
                    }
                    else
                    {
                        spinStopThen(() => AnsiConsole.MarkupLineInterpolated($"[red]Unknown event type: {response.Result?.GetType().Name}[/]"));
                    }
                }
            }
            finally
            {
                spinStopThen(() => Console.WriteLine());
            }
        }
        else
        {
            var request = new SendTaskRequest { Params = taskParams };

            try
            {
                var task = await agentCommSpinnerDef
                    .StartAsync("Communicating with Agent...", async ctx => await client.SendTaskAsync(request, agentCts.Token));

                if (task.Error is not null)
                {
                    AnsiConsole.MarkupLineInterpolated($"[red]❌ Error: {task.Error.Message}[/]");
                    continue;
                }

                if (task.Result?.Artifacts?.Count is not null and not 0)
                {
                    AnsiConsole.Markup($"[bold green]Agent>[/] ");
                    foreach (var a in task.Result?.Artifacts ?? [])
                    {
                        await PrintArtifactAsync(a);
                    }
                }
                else
                {
                    AnsiConsole.MarkupLine($"[red]❌ Error: No artifacts found in response.[/]");
                }
            }
            finally
            {
                spinStopThen(() => Console.WriteLine());
            }
        }
    }
    catch (OperationCanceledException)
    {
        break;
    }
    catch (Exception ex)
    {
        spinStopThen(() => AnsiConsole.MarkupLineInterpolated($"[red]❌ Error: {ex.Message}[/]"));
    }
}

void printAgentCards()
{
    AnsiConsole.MarkupLine("[bold green]Available Agents:[/]");
    foreach (var agent in agents)
    {
        AnsiConsole.MarkupLineInterpolated($"[green]{agent.Name} - {agent.Description} - v{(string.IsNullOrWhiteSpace(agent.Version) ? "??" : agent.Version)}[/]");
        AnsiConsole.MarkupLineInterpolated($"[green]  {nameof(agent.Url)} - {agent.Url}[/]");
        AnsiConsole.MarkupLineInterpolated($"[green]  {nameof(agent.Authentication)} - {(agent.Authentication is null ? "None" : string.Join(", ", agent.Authentication!.Schemes))}[/]");
        AnsiConsole.MarkupLineInterpolated($"[green]  {nameof(agent.Capabilities)} - {nameof(agent.Capabilities.Streaming)}: {(agent.Capabilities.Streaming ? "✅" : "❌")} | {nameof(agent.Capabilities.PushNotifications)}: {(agent.Capabilities.PushNotifications ? "✅" : "❌")} | {nameof(agent.Capabilities.StateTransitionHistory)}: {(agent.Capabilities.StateTransitionHistory ? "✅" : "❌")}[/]");
        AnsiConsole.MarkupLineInterpolated($"[green]  {nameof(agent.DocumentationUrl)} - {agent.DocumentationUrl}[/]");
        AnsiConsole.MarkupLineInterpolated($"[green]  {nameof(agent.Skills)} - {(agent.Skills.Count is 0 ? "None" : string.Join(", ", agent.Skills.Select(s => s.Name)))}[/]");
    }
}

static async System.Threading.Tasks.Task PrintArtifactAsync(Artifact artifact)
{
    foreach (var p in artifact.Parts ?? [])
    {
        if (p is TextPart t)
        {
            AnsiConsole.Markup(p.ToText()?.EscapeMarkup() ?? string.Empty);
        }
        else if (p is FilePart f)
        {
            AnsiConsole.MarkupLineInterpolated($"[darkgreen]File: {f.File.Name}[/]");
            if (f.File.Uri is not null)
            {
                AnsiConsole.MarkupLineInterpolated($"[darkgreen]URI: {f.File.Uri}[/]");
            }

            if (f.File.Bytes is not null)
            {
                var filename = Path.Combine(Path.GetTempPath(), f.File.Name!);
                await System.IO.File.WriteAllBytesAsync(filename, Convert.FromBase64String(f.File.Bytes!));
                AnsiConsole.MarkupLineInterpolated($"[darkgreen]Downloaded to: {filename}[/]");
            }
        }
        else if (p is DataPart d)
        {
            AnsiConsole.MarkupLineInterpolated($"[darkgreen]Data: {(d.Type is null ? "Unknown" : d.Type)}[/]");
            foreach (var i in d.Metadata ?? [])
            {
                AnsiConsole.MarkupLineInterpolated($"[darkgreen]{i.Key}: {i.Value}[/]");
            }
        }
        else
        {
            AnsiConsole.MarkupLineInterpolated($"[red]Unknown part type: {p.Type}[/]");
        }
    }
}