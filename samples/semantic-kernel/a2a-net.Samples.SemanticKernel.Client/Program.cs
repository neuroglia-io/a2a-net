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

var configuration = new ConfigurationBuilder()
    .AddCommandLine(args)
    .AddEnvironmentVariables()
    .AddUserSecrets(typeof(ApplicationOptions).Assembly)
    .AddJsonFile("appsettings.json", true)
    .Build();
var applicationOptions = new ApplicationOptions();
configuration.Bind(applicationOptions);
var services = new ServiceCollection();
services.AddA2AProtocolHttpClient(options =>
{
    options.Endpoint = applicationOptions.Server;
});
var provider = services.BuildServiceProvider();
var client = provider.GetRequiredService<IA2AProtocolClient>();
var cancellationSource = new CancellationTokenSource();
AnsiConsole.Write(new FigletText("A2A Protocol Chat").Color(Color.Blue));
AnsiConsole.MarkupLine("[gray]Type your prompts below. Press [bold]Ctrl+C[/] to exit.[/]\n");
while (true)
{
    var prompt = AnsiConsole.Ask<string>("[bold blue]User>[/]");
    if (string.IsNullOrWhiteSpace(prompt))
    {
        AnsiConsole.MarkupLine("[yellow]⚠️ Please enter a prompt.[/]");
        continue;
    }
    var request = new SendTaskRequest
    {
        Params = new()
        {
            Message = new()
            {
                Role = MessageRole.User,
                Parts = [new TextPart(prompt)]
            }
        }
    };
    try
    {
        var response = await client.SendTaskAsync(request, cancellationSource.Token);
        if (response.Result?.Artifacts is { Count: > 0 })
        {
            foreach (var artifact in response.Result.Artifacts)
            {
                if (artifact.Parts != null)
                {
                    foreach (var part in artifact.Parts.OfType<TextPart>()) AnsiConsole.MarkupLine($"[bold green]Agent>[/] {part.Text}");
                }
            }
        }
        else
        {
            AnsiConsole.MarkupLine("[italic gray]Agent> (no response received)[/]");
        }
    }
    catch (OperationCanceledException)
    {
        break;
    }
    catch (Exception ex)
    {
        AnsiConsole.MarkupLineInterpolated($"[red]❌ Error: {ex.Message}[/]");
    }
}
