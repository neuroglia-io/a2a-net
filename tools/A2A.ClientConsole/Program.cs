using A2A;
using A2A.Client;
using A2A.Models;
using Microsoft.Extensions.DependencyInjection;
using Spectre.Console;
using Spectre.Console.Rendering;
using System.Text;
using Task = System.Threading.Tasks.Task;

var application = new Application();
await application.RunAsync();

public sealed class Application
{

    Uri? serverUri;
    AgentCard? agentCard;
    AgentInterface? agentInterface;
    IA2AClient? client;

    public async Task RunAsync()
    {
        Console.OutputEncoding = Encoding.UTF8;
        while (true)
        {
            if (!AcquireServerUri()) return;
            await DiscoverAgentAsync();
            DisplayNewPage();
            DisplayAgentCard();
            if (agentCard!.SupportedInterfaces?.Count < 1)
            {
                DisplayError("The discovered agent does not have any supported interfaces.");
                AnsiConsole.MarkupLine("Press any key to continue...");
                Console.ReadLine();
                continue;
            }
            var agentSelectionChoice = AnsiConsole.Prompt(new SelectionPrompt<string>().AddChoices([AgentSelectionChoice.Continue, AgentSelectionChoice.Switch, AgentSelectionChoice.Exit]));
            if (agentSelectionChoice == AgentSelectionChoice.Exit) return;
            else if (agentSelectionChoice == AgentSelectionChoice.Switch) continue;
            DisplayNewPage();
            SelectInterface();
            DisplayNewPage();
            BuildClient();
            await RunInteractionLoopAsync();
        }
    }

    bool AcquireServerUri()
    {
        while (true)
        {
            DisplayNewPage();
            var serverAddress = AnsiConsole.Ask<string>("Enter the address of the [green]A2A server[/] to connect to:");
            if (!Uri.TryCreate(serverAddress, UriKind.Absolute, out serverUri))
            {
                DisplayError("Invalid server address provided.");
                if (!AnsiConsole.Confirm("Try again?")) return false;
                continue;
            }
            return true;
        }
    }

    async Task DiscoverAgentAsync()
    {
        await AnsiConsole.Status().StartAsync("Discovering agent...", async context =>
        {
            context.Spinner(Spinner.Known.Dots);
            context.SpinnerStyle(Style.Parse("green"));
            try
            {
                using var httpClient = new HttpClient();
                var discoveryDocument = await httpClient.GetA2ADiscoveryDocumentAsync(serverUri!);
                agentCard = discoveryDocument.Agent;
            }
            catch (Exception ex)
            {
                DisplayError($"Error discovering agent: {ex.Message}");
                throw;
            }
        });
    }

    void DisplayAgentCard()
    {
        var infoGrid = new Grid().AddColumn().AddColumn();
        infoGrid.AddRow(new Markup("[bold blue]Agent:[/]"), new Markup(agentCard!.Name));
        infoGrid.AddRow(new Markup("[bold]Description:[/]"), new Markup(agentCard.Description ?? "[dim]N/A[/]"));
        infoGrid.AddRow(new Markup("[bold]Version:[/]"), new Markup($"{agentCard.Version} [dim](Protocol: {agentCard.ProtocolVersion})[/]"));
        if (agentCard.Provider != null) infoGrid.AddRow(new Markup("[bold]Provider:[/]"), new Markup(agentCard.Provider.Organization ?? "[dim]N/A[/]"));
        var infoPanel = new Panel(infoGrid)
            .Header("[blue]Agent Information[/]")
            .Border(BoxBorder.Rounded)
            .BorderColor(Color.Blue)
            .Expand();
        Panel? capabilitiesPanel = null;
        if (agentCard.Capabilities != null)
        {
            var capabilitiesMarkup = new StringBuilder();
            capabilitiesMarkup.AppendLine($"{(agentCard.Capabilities.Streaming == true ? "[green]☑[/]" : "[dim]☐[/]")}  Streaming");
            capabilitiesMarkup.AppendLine($"{(agentCard.Capabilities.PushNotifications == true ? "[green]☑[/]" : "[dim]☐[/]")}  Push Notifications");
            capabilitiesMarkup.AppendLine($"{(agentCard.Capabilities.StateTransitionHistory == true ? "[green]☑[/]" : "[dim]☐[/]")}  State Transition History");

            if (agentCard.Capabilities.Extensions?.Count > 0)
            {
                capabilitiesMarkup.AppendLine();
                capabilitiesMarkup.AppendLine("[bold underline]Supported Extensions:[/]");
                foreach (var extension in agentCard.Capabilities.Extensions)
                {
                    var requiredBadge = extension.Required == true ? "[red](required)[/]" : "[dim](optional)[/]";
                    capabilitiesMarkup.AppendLine($"  • [cyan]{extension.Uri}[/] {requiredBadge}");
                    if (!string.IsNullOrEmpty(extension.Description))
                    {
                        capabilitiesMarkup.AppendLine($"    [dim]{extension.Description}[/]");
                    }
                }
            }

            capabilitiesPanel = new Panel(new Markup(capabilitiesMarkup.ToString().TrimEnd()))
                .Header("[blue]Capabilities[/]")
                .Border(BoxBorder.Rounded)
                .BorderColor(Color.Blue)
                .Expand();
        }

        var modesMarkup = new StringBuilder();
        if (agentCard.DefaultInputModes?.Count > 0) modesMarkup.AppendLine($"[bold]Input Modes:[/] {string.Join(", ", agentCard.DefaultInputModes)}");
        if (agentCard.DefaultOutputModes?.Count > 0) modesMarkup.AppendLine($"[bold]Output Modes:[/] {string.Join(", ", agentCard.DefaultOutputModes)}");

        var modesPanel = new Panel(new Markup(modesMarkup.ToString().TrimEnd()))
            .Header("[blue]Input/Output Modes[/]")
            .Border(BoxBorder.Rounded)
            .BorderColor(Color.Blue)
            .Expand();

        var skillsPanels = new List<Panel>();
        if (agentCard.Skills?.Count > 0) foreach (var skill in agentCard.Skills)
        {
            var skillContent = new StringBuilder();
            skillContent.AppendLine($"[bold]Description:[/] {skill.Description ?? "[dim]No description[/]"}");

            if (skill.Tags?.Count > 0) skillContent.AppendLine($"[bold]Tags:[/] {string.Join(", ", skill.Tags.Select(t => $"[cyan]{t}[/]"))}");

            if (skill.InputModes?.Count > 0) skillContent.AppendLine($"[bold]Input Modes:[/] {string.Join(", ", skill.InputModes)}");

            if (skill.OutputModes?.Count > 0) skillContent.AppendLine($"[bold]Output Modes:[/] {string.Join(", ", skill.OutputModes)}");

            if (skill.Examples?.Count > 0)
            {
                skillContent.AppendLine("[bold]Examples:[/]");
                foreach (var example in skill.Examples) skillContent.AppendLine($"  • [dim]{example}[/]");
            }

            var skillPanel = new Panel(new Markup(skillContent.ToString().TrimEnd()))
                .Header($"[blue]{skill.Name}[/]")
                .Border(BoxBorder.Rounded)
                .BorderColor(Color.Blue)
                .Expand();

            skillsPanels.Add(skillPanel);
        }

        var allSections = new List<IRenderable> { infoPanel };
        if (capabilitiesPanel != null) allSections.Add(capabilitiesPanel);
        allSections.Add(modesPanel);
        allSections.AddRange(skillsPanels);

        var sectionsLayout = new Rows(allSections);

        var agentCardPanel = new Panel(sectionsLayout)
            .Header("[grey]Agent Card[/]")
            .Border(BoxBorder.Double)
            .BorderColor(Color.Grey)
            .Expand();

        AnsiConsole.Write(agentCardPanel);
        AnsiConsole.WriteLine();
    }

    void SelectInterface() 
    {
        var interfaceChoices = agentCard!.SupportedInterfaces!.Select((iface, index) => $"{iface.ProtocolBinding, -9} [dim][[{iface.Url}]][/]").ToList();
        var selectedInterface = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("Select the [green]transport[/] to use:")
                .PageSize(3)
                .AddChoices(interfaceChoices)
        );
        var selectedIndex = interfaceChoices.IndexOf(selectedInterface);
        agentInterface = agentCard.SupportedInterfaces!.ElementAt(selectedIndex);
    }

    void BuildClient()
    {
        var services = new ServiceCollection();
        Action<IA2AClientBuilder> clientSetup = agentInterface!.ProtocolBinding switch
        {
            ProtocolBinding.Http => builder => builder.UseHttpTransport(agentInterface.Url!),
            ProtocolBinding.Grpc => builder => builder.UseGrpcTransport(agentInterface.Url!),
            ProtocolBinding.JsonRpc => builder => builder.UseJsonRpcTransport(agentInterface.Url!),
            _ => throw new NotSupportedException($"The specified protocol binding '{agentInterface.ProtocolBinding}' is not supported."),
        };
        services.AddA2AClient(clientSetup);
        var serviceProvider = services.BuildServiceProvider();
        client = serviceProvider.GetRequiredService<IA2AClient>();
    }

    void DisplayNewPage()
    {
        AnsiConsole.Clear();
        AnsiConsole.Write(new FigletText("A2A Client Console").Color(Color.Blue));
        var rule = agentCard == null ? new Rule() : new Rule($"[blue]{agentCard.Name}{(agentInterface == null ? string.Empty : $" [[{agentInterface.ProtocolBinding}]]")}[/]");
        rule.RuleStyle(Style.Parse("blue"));
        AnsiConsole.Write(rule);
        AnsiConsole.WriteLine();
    }

    async Task RunInteractionLoopAsync()
    {
        while (true)
        {
            DisplayNewPage();
            var menuChoice = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("What would you like to do?")
                    .AddChoices([MenuChoice.Chat, MenuChoice.Tasks, MenuChoice.Transport, MenuChoice.Disconnect])
            );
            switch (menuChoice)
            {
                case MenuChoice.Chat:
                    await ChatAsync();
                    break;
                case MenuChoice.Tasks:
                    await ManageTasksAsync();
                    break;
                case MenuChoice.Transport:
                    await SwitchTransportAsync();
                    break;
                case MenuChoice.Disconnect:
                    return;
            }
        }
    }

    async Task ChatAsync()
    {
        DisplayNewPage();
        var userInput = AnsiConsole.Ask<string>("[bold blue]You:[/]");
        var userMessage = new Message
        {
            Role = Role.User,
            Parts = [new TextPart { Text = userInput }]
        };
        var request = new SendMessageRequest
        {
            Message = userMessage
        };

        try
        {
            var cancellationTokenSource = new CancellationTokenSource();
            var supportsStreaming = agentCard!.Capabilities?.Streaming == true;
            var currentTaskId = (string?)null;
            if (supportsStreaming)
            {
                var responseText = new StringBuilder();
                var streamingTask = Task.Run(async () =>
                {
                    string? currentArtifactId = null;
                    await foreach (var streamResponse in client!.SendStreamingMessageAsync(request, cancellationTokenSource.Token))
                    {
                        if (streamResponse.Task != null)
                        {
                            currentTaskId = streamResponse.Task.Id;
                            AnsiConsole.MarkupLine($"\n[blue]Agent:[/] [dim]Task started: {currentTaskId}[/]");
                            AnsiConsole.MarkupLine("[blue]Agent:[/] [dim]Status: {0}[/]", streamResponse.Task.Status?.State ?? "unknown");
                            if (streamResponse.Task.Artifacts != null)
                            {
                                foreach (var artifact in streamResponse.Task.Artifacts)
                                {

                                    foreach (var part in artifact.Parts)
                                    {
                                        if (part is TextPart textPart)
                                        {
                                            responseText.Append(textPart.Text);
                                            AnsiConsole.Markup(textPart.Text);
                                        }
                                    }
                                }
                            }
                        }
                        else if (streamResponse.Message != null)
                        {
                            foreach (var part in streamResponse.Message.Parts)
                            {
                                if (part is TextPart textPart)
                                {
                                    responseText.Append(textPart.Text);
                                    AnsiConsole.Markup(textPart.Text);
                                }
                            }
                        }
                        else if (streamResponse.StatusUpdate != null)
                        {
                            AnsiConsole.MarkupLine($"\n[blue]Agent:[/] [dim]Task status: {streamResponse.StatusUpdate.Status?.State}[/]");
                        }
                        else if (streamResponse.ArtifactUpdate != null)
                        {
                            if (streamResponse.ArtifactUpdate.Artifact.ArtifactId != currentArtifactId) currentArtifactId = streamResponse.ArtifactUpdate.Artifact.ArtifactId;
                            foreach (var part in streamResponse.ArtifactUpdate.Artifact.Parts)
                            {
                                switch (part)
                                {
                                    case TextPart textPart:
                                        AnsiConsole.Markup(textPart.Text);
                                        break;
                                }
                            }
                        }
                    }
                }, cancellationTokenSource.Token);
                AnsiConsole.MarkupLine("\n[dim]Press 'C' to cancel the task...[/]");
                while (!streamingTask.IsCompleted)
                {
                    if (Console.KeyAvailable && Console.ReadKey(true).Key == ConsoleKey.C)
                    {
                        if (currentTaskId != null)
                        {
                            try
                            {
                                await client!.CancelTaskAsync(currentTaskId);
                                AnsiConsole.MarkupLine("\n[yellow]Task cancelled.[/]");
                            }
                            catch (Exception ex)
                            {
                                AnsiConsole.MarkupLine($"\n[red]Failed to cancel task: {ex.Message}[/]");
                            }
                        }
                        break;
                    }
                    await System.Threading.Tasks.Task.Delay(100);
                }
                try
                {
                    await streamingTask;
                }
                catch (OperationCanceledException)
                {
                    AnsiConsole.MarkupLine("\n[yellow]Operation cancelled.[/]");
                }

                if (responseText.Length > 0)
                {
                    var agentMessage = new Message
                    {
                        Role = Role.Agent,
                        Parts = [new TextPart { Text = responseText.ToString() }]
                    };
                }
            }
            else
            {
                var response = await client!.SendMessageAsync(request);
                if (response is Message message)
                {
                    AnsiConsole.MarkupLine("[blue]Agent:[/]");
                    foreach (var part in message.Parts)
                    {
                        switch (part)
                        {
                            case TextPart textPart:
                                AnsiConsole.Markup(textPart.Text);
                                break;
                        }
                    }
                }
                else if (response is A2A.Models.Task task)
                {
                    currentTaskId = task.Id;
                    AnsiConsole.MarkupLine($"[yellow]⚙ Task created: {currentTaskId}[/]");
                    AnsiConsole.MarkupLine($"[dim]Status: {task.Status?.State ?? "unknown"}[/]");
                }
            }
        }
        catch (Exception ex)
        {
            AnsiConsole.MarkupLine($"[red]Error sending message: {ex.Message}[/]");
        }
        AnsiConsole.WriteLine();
        AnsiConsole.MarkupLine("[dim]Press any key to continue...[/]");
        Console.ReadKey(true);
    }

    async Task ManageTasksAsync()
    {
        DisplayNewPage();

    }

    async Task SwitchTransportAsync()
    {
        DisplayNewPage();
        SelectInterface();
    }

    static void DisplayError(string message)
    {
        AnsiConsole.MarkupLine($"[red]{message}[/]");
    }

    static class AgentSelectionChoice
    {

        public const string Continue = "Continue with this agent";
        public const string Switch = "Connect to a different server";
        public const string Exit = "[red]Exit[/]";

    }

    static class MenuChoice
    {

        public const string Chat = "Chat";
        public const string Tasks = "Manage Tasks";
        public const string Transport = "Switch Transport";
        public const string Disconnect = "[red]Disconnect[/]";

    }

}

//while (true)
//{


//       

//        var currentMode = "chat";
//        var conversationHistory = new List<Message>();
//        var currentTaskId = (string?)null;
//        var cancellationTokenSource = new CancellationTokenSource();

//        while (true)
//        {
//            AnsiConsole.Clear();
//            AnsiConsole.Write(new FigletText("A2A Protocol Chat").Color(Color.Blue));
//            AnsiConsole.MarkupLine($"[grey]Connected to: {agentCard.Name} | Mode: {currentMode.ToUpper()}[/]");
//            AnsiConsole.WriteLine();

//            var modeChoice = AnsiConsole.Prompt(
//                new SelectionPrompt<string>()
//                    .Title($"Current mode: [blue]{currentMode.ToUpper()}[/]. What would you like to do?")
//                    .AddChoices(currentMode == "chat" ? ["Send a message", "Switch to Tasks", "Disconnect"] : ["View task details", "Switch to Chat", "Disconnect"])
//            );

//            if (modeChoice == "Disconnect") break;

//            if (modeChoice == "Switch to Tasks")
//            {
//                currentMode = "tasks";

//                if (client != null)
//                {
//                    try
//                    {
//                        await AnsiConsole.Status().StartAsync("Loading tasks...", async ctx =>
//                        {
//                            ctx.Spinner(Spinner.Known.Dots);
//                            var taskList = await client.ListTasksAsync();

//                            AnsiConsole.Clear();
//                            AnsiConsole.Write(new FigletText("A2A Protocol Chat").Color(Color.Blue));
//                            AnsiConsole.MarkupLine($"[grey]Connected to: {agentCard.Name} | Mode: TASKS[/]");
//                            AnsiConsole.WriteLine();

//                            if (taskList.Tasks.Count > 0)
//                            {
//                                var tasksTable = new Table()
//                                    .Border(TableBorder.Rounded)
//                                    .BorderColor(Color.Blue);

//                                tasksTable.AddColumn("[bold]Task ID[/]");
//                                tasksTable.AddColumn("[bold]Context ID[/]");
//                                tasksTable.AddColumn("[bold]Status[/]");
//                                tasksTable.AddColumn("[bold]Status Timestamp[/]");

//                                foreach (var task in taskList.Tasks)
//                                {
//                                    var statusColor = task.Status?.State switch
//                                    {
//                                        TaskState.Completed => "green",
//                                        TaskState.Failed or TaskState.Rejected => "red",
//                                        TaskState.Cancelled => "yellow",
//                                        TaskState.Unspecified => "dim",
//                                        _ => "blue"
//                                    };

//                                    tasksTable.AddRow(task.Id, task.ContextId, $"[{statusColor}]{task.Status?.State[11..] ?? "UNSPECIFIED"}[/]", task.Status?.Timestamp.ToString() ?? "N/A");
//                                }

//                                AnsiConsole.Write(tasksTable);
//                            }
//                            else
//                            {
//                                AnsiConsole.MarkupLine("[yellow]No tasks found.[/]");
//                            }
//                        });
//                    }
//                    catch (Exception ex)
//                    {
//                        AnsiConsole.MarkupLine($"[red]Error loading tasks: {ex.Message}[/]");
//                    }
//                }
//                else
//                {
//                    AnsiConsole.MarkupLine("[yellow]Client not available. Cannot retrieve tasks.[/]");
//                }

//                AnsiConsole.WriteLine();
//                AnsiConsole.MarkupLine("Press any key to continue...");
//                Console.ReadKey(true);
//                continue;
//            }

//            if (modeChoice == "Switch to Chat")
//            {
//                currentMode = "chat";
//                continue;
//            }

//            if (modeChoice == "Send a message")
//            {
//                var userInput = AnsiConsole.Ask<string>("[green]You:[/]");

//                if (string.IsNullOrWhiteSpace(userInput)) continue;

//                if (client == null)
//                {
//                    AnsiConsole.MarkupLine("[red]Client not available. Cannot send message.[/]");
//                    AnsiConsole.MarkupLine("Press any key to continue...");
//                    Console.ReadKey(true);
//                    continue;
//                }

//                var userMessage = new Message
//                {
//                    Role = Role.User,
//                    Parts = [new TextPart { Text = userInput }]
//                };

//                conversationHistory.Add(userMessage);

//                var request = new SendMessageRequest
//                {
//                    Message = userMessage
//                };

//                try
//                {
//                    var supportsStreaming = agentCard.Capabilities?.Streaming == true;

//                    if (supportsStreaming)
//                    {
//                        AnsiConsole.MarkupLine("[blue]Agent:[/] [dim](streaming...)[/]");
//                        var responseText = new StringBuilder();

//                        cancellationTokenSource = new CancellationTokenSource();

//                        var streamingTask = System.Threading.Tasks.Task.Run(async () =>
//                        {
//                            string? currentArtifactId = null;
//                            await foreach (var streamResponse in client.SendStreamingMessageAsync(request, cancellationTokenSource.Token))
//                            {
//                                if (streamResponse.Task != null)
//                                {
//                                    currentTaskId = streamResponse.Task.Id;
//                                    AnsiConsole.MarkupLine($"\n[yellow]⚙  Task started: {currentTaskId}[/]");
//                                    AnsiConsole.MarkupLine("[dim]Status: {0}[/]", streamResponse.Task.Status?.State ?? "unknown");
//                                    if (streamResponse.Task.Artifacts != null)
//                                    {
//                                        foreach(var artifact in streamResponse.Task.Artifacts)
//                                        {
//                                            foreach (var part in artifact.Parts)
//                                            {
//                                                if (part is TextPart textPart)
//                                                {
//                                                    responseText.Append(textPart.Text);
//                                                    AnsiConsole.Markup(textPart.Text);
//                                                }
//                                            }
//                                        }
//                                    }   
//                                }
//                                else if (streamResponse.Message != null)
//                                {
//                                    foreach (var part in streamResponse.Message.Parts)
//                                    {
//                                        if (part is TextPart textPart)
//                                        {
//                                            responseText.Append(textPart.Text);
//                                            AnsiConsole.Markup(textPart.Text);
//                                        }
//                                    }
//                                }
//                                else if (streamResponse.StatusUpdate != null)
//                                {
//                                    AnsiConsole.MarkupLine($"\n[dim]Task status: {streamResponse.StatusUpdate.Status?.State}[/]");
//                                }
//                                else if (streamResponse.ArtifactUpdate != null)
//                                {
//                                    if (streamResponse.ArtifactUpdate.Artifact.ArtifactId != currentArtifactId) currentArtifactId = streamResponse.ArtifactUpdate.Artifact.ArtifactId;
//                                    foreach (var part in streamResponse.ArtifactUpdate.Artifact.Parts)
//                                    {
//                                        switch (part)
//                                        {
//                                            case TextPart textPart:
//                                                AnsiConsole.Markup(textPart.Text);
//                                                break;
//                                        }
//                                    }
//                                }
//                            }
//                        }, cancellationTokenSource.Token);

//                        AnsiConsole.MarkupLine("\n[dim]Press 'C' to cancel the task...[/]");
//                        while (!streamingTask.IsCompleted)
//                        {
//                            if (Console.KeyAvailable && Console.ReadKey(true).Key == ConsoleKey.C)
//                            {
//                                if (currentTaskId != null)
//                                {
//                                    try
//                                    {
//                                        await client.CancelTaskAsync(currentTaskId);
//                                        AnsiConsole.MarkupLine("\n[yellow]Task cancelled.[/]");
//                                    }
//                                    catch (Exception ex)
//                                    {
//                                        AnsiConsole.MarkupLine($"\n[red]Failed to cancel task: {ex.Message}[/]");
//                                    }
//                                }
//                                break;
//                            }
//                            await System.Threading.Tasks.Task.Delay(100);
//                        }

//                        try
//                        {
//                            await streamingTask;
//                        }
//                        catch (OperationCanceledException)
//                        {
//                            AnsiConsole.MarkupLine("\n[yellow]Operation cancelled.[/]");
//                        }

//                        if (responseText.Length > 0)
//                        {
//                            var agentMessage = new Message
//                            {
//                                Role = Role.Agent,
//                                Parts = [new TextPart { Text = responseText.ToString() }]
//                            };
//                            conversationHistory.Add(agentMessage);
//                        }
//                    }
//                    else
//                    {
//                        var response = await client.SendMessageAsync(request);

//                        if (response is Message message)
//                        {
//                            AnsiConsole.MarkupLine("[blue]Agent:[/]");
//                            foreach (var part in message.Parts)
//                            {
//                                if (part is TextPart textPart)
//                                {
//                                    AnsiConsole.MarkupLine(textPart.Text);
//                                }
//                            }
//                            conversationHistory.Add(message);
//                        }
//                        else if (response is A2A.Models.Task task)
//                        {
//                            currentTaskId = task.Id;
//                            AnsiConsole.MarkupLine($"[yellow]⚙ Task created: {currentTaskId}[/]");
//                            AnsiConsole.MarkupLine($"[dim]Status: {task.Status?.State ?? "unknown"}[/]");
//                        }
//                    }
//                }
//                catch (Exception ex)
//                {
//                    AnsiConsole.MarkupLine($"[red]Error sending message: {ex.Message}[/]");
//                }

//                AnsiConsole.WriteLine();
//                AnsiConsole.MarkupLine("[dim]Press any key to continue...[/]");
//                Console.ReadKey(true);
//            }

//            if (modeChoice == "View task details" && currentTaskId != null)
//            {
//                try
//                {
//                    var task = await client!.GetTaskAsync(currentTaskId);

//                    var tasksTable = new Table().Border(TableBorder.Rounded).BorderColor(Color.Blue);

//                    tasksTable.AddColumn("[bold]Task ID[/]");
//                    tasksTable.AddColumn("[bold]Context ID[/]");
//                    tasksTable.AddColumn("[bold]Status[/]");
//                    tasksTable.AddColumn("[bold]Status Timestamp[/]");

//                    var statusColor = task.Status?.State switch
//                    {
//                        TaskState.Completed => "green",
//                        TaskState.Failed or TaskState.Rejected => "red",
//                        TaskState.Cancelled => "yellow",
//                        TaskState.Unspecified => "dim",
//                        _ => "blue"
//                    };

//                    tasksTable.AddRow(task.Id, task.ContextId, $"[{statusColor}]{task.Status?.State[11..] ?? "UNSPECIFIED"}[/]", task.Status?.Timestamp.ToString() ?? "N/A");
//                    AnsiConsole.Write(tasksTable);
//                }
//                catch (Exception ex)
//                {
//                    AnsiConsole.MarkupLine($"[red]Error retrieving task: {ex.Message}[/]");
//                }

//                AnsiConsole.WriteLine();
//                AnsiConsole.MarkupLine("[dim]Press any key to continue...[/]");
//                Console.ReadKey(true);
//            }
//        }

//        break;
//    }
//    else
//    {
//        AnsiConsole.MarkupLine("[yellow]⚠[/] No interfaces available on this agent.");
//        if (!AnsiConsole.Confirm("Try a different server?")) return;
//    }
//}