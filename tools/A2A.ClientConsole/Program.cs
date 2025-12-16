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
        var contextId = Guid.NewGuid().ToString("N");
        var taskId = (string?)null;
    CollectInput:
        AnsiConsole.WriteLine();
        var userInput = AnsiConsole.Ask<string>("[bold blue]You:[/]");
        var userMessage = new Message
        {
            ContextId = contextId,
            TaskId = taskId,
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
            var loop = false;
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
                            taskId = streamResponse.Task.Id;
                            AnsiConsole.MarkupLine($"\n[blue]Agent:[/] [dim]Task started: {taskId}[/]");
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
                                            AnsiConsole.Write(textPart.Text);
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
                                    AnsiConsole.Write(textPart.Text);
                                }
                            }
                            AnsiConsole.WriteLine();
                        }
                        else if (streamResponse.StatusUpdate != null)
                        {
                            AnsiConsole.MarkupLine($"\n[blue]Agent:[/] [dim]Task status: {streamResponse.StatusUpdate.Status?.State}[/]");
                            if (streamResponse.StatusUpdate.Status?.Message is not null)
                            {
                                foreach (var part in streamResponse.StatusUpdate.Status.Message.Parts)
                                {
                                    switch (part)
                                    {
                                        case TextPart textPart:
                                            AnsiConsole.Write(textPart.Text);
                                            break;
                                    }
                                }
                                AnsiConsole.WriteLine();
                            }
                            if (streamResponse.StatusUpdate.Status?.State == TaskState.InputRequired)
                            {
                                loop = true;
                                break;
                            }
                        }
                        else if (streamResponse.ArtifactUpdate != null)
                        {
                            if (streamResponse.ArtifactUpdate.Artifact.ArtifactId != currentArtifactId) currentArtifactId = streamResponse.ArtifactUpdate.Artifact.ArtifactId;
                            foreach (var part in streamResponse.ArtifactUpdate.Artifact.Parts)
                            {
                                switch (part)
                                {
                                    case TextPart textPart:
                                        AnsiConsole.Write(textPart.Text);
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
                        if (taskId != null)
                        {
                            try
                            {
                                await client!.CancelTaskAsync(taskId);
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
                if (loop) goto CollectInput;
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
                    taskId = task.Id;
                    AnsiConsole.MarkupLine($"[yellow]⚙ Task created: {taskId}[/]");
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

        var queryOptions = new TaskQueryOptions { PageSize = 10 };
        var pageSize = (int)queryOptions.PageSize;
        uint pageNumber = 1;
        var previousPageTokens = new Stack<string?>();
        var taskIndex = 0;
        var pageStartIndex = 0;
        var backToMenu = false;
        while (!backToMenu)
        {
            var tasksPage = await LoadTasksAsync(queryOptions);
            if (tasksPage == null)
            {
                AnsiConsole.MarkupLine("Press any key to return to the menu...");
                Console.ReadKey(true);
                return;
            }
            pageStartIndex = ((int)pageNumber - 1) * pageSize;
            if (tasksPage.Tasks.Count > 0) taskIndex = Math.Clamp(taskIndex, pageStartIndex, pageStartIndex + tasksPage.Tasks.Count - 1);
            await DisplayTasksPageAsync(tasksPage, pageNumber);
            while (true)
            {
                var key = Console.ReadKey(true).Key;

                switch (key)
                {
                    case ConsoleKey.UpArrow:
                        {
                            if (string.IsNullOrWhiteSpace(tasksPage.NextPageToken)) continue;
                            previousPageTokens.Push(queryOptions.PageToken);
                            pageNumber++;
                            queryOptions = queryOptions with { PageToken = tasksPage.NextPageToken };
                            taskIndex = ((int)pageNumber - 1) * pageSize;
                            goto ReloadPage;
                        }

                    case ConsoleKey.DownArrow:
                        {
                            if (pageNumber == 1) continue;
                            pageNumber--;
                            var prevToken = previousPageTokens.Count > 0 ? previousPageTokens.Pop() : null;
                            queryOptions = queryOptions with { PageToken = prevToken };
                            taskIndex = ((int)pageNumber - 1) * pageSize;
                            goto ReloadPage;
                        }
                    case ConsoleKey.Enter:
                        {
                            if (tasksPage.Tasks.Count == 0) continue;
                            while (true)
                            {
                                var localIndex = taskIndex - pageStartIndex;
                                localIndex = Math.Clamp(localIndex, 0, tasksPage.Tasks.Count - 1);
                                DisplayNewPage();
                                var totalTasks = (tasksPage.TotalSize > 0) ? tasksPage.TotalSize : ((uint)pageStartIndex + tasksPage.Tasks.Count);
                                DisplayTask(tasksPage.Tasks.ElementAt(localIndex), taskIndex + 1, (int)totalTasks);
                                var dkey = Console.ReadKey(true).Key;
                                switch (dkey)
                                {
                                    case ConsoleKey.RightArrow:
                                        {
                                            var nextGlobal = taskIndex + 1;
                                            if (nextGlobal <= pageStartIndex + tasksPage.Tasks.Count - 1)
                                            {
                                                taskIndex = nextGlobal;
                                                break;
                                            }
                                            if (string.IsNullOrWhiteSpace(tasksPage.NextPageToken)) break;
                                            previousPageTokens.Push(queryOptions.PageToken);
                                            pageNumber++;
                                            queryOptions = queryOptions with { PageToken = tasksPage.NextPageToken };
                                            var nextPage = await LoadTasksAsync(queryOptions);
                                            if (nextPage == null || nextPage.Tasks.Count == 0)
                                            {
                                                pageNumber--;
                                                queryOptions = queryOptions with { PageToken = previousPageTokens.Count > 0 ? previousPageTokens.Pop() : null };
                                                break;
                                            }
                                            tasksPage = nextPage;
                                            pageStartIndex = ((int)pageNumber - 1) * pageSize;
                                            taskIndex = pageStartIndex;
                                            break;
                                        }

                                    case ConsoleKey.LeftArrow:
                                        {
                                            var prevGlobal = taskIndex - 1;
                                            if (prevGlobal >= pageStartIndex)
                                            {
                                                taskIndex = prevGlobal;
                                                break;
                                            }
                                            if (pageNumber == 1) break;
                                            pageNumber--;
                                            var prevToken = previousPageTokens.Count > 0 ? previousPageTokens.Pop() : null;
                                            queryOptions = queryOptions with { PageToken = prevToken };
                                            var prevPage = await LoadTasksAsync(queryOptions);
                                            if (prevPage == null || prevPage.Tasks.Count == 0)
                                            {
                                                pageNumber++;
                                                previousPageTokens.Push(prevToken);
                                                break;
                                            }
                                            tasksPage = prevPage;
                                            pageStartIndex = ((int)pageNumber - 1) * pageSize;
                                            taskIndex = pageStartIndex + tasksPage.Tasks.Count - 1;
                                            break;
                                        }

                                    case ConsoleKey.L:
                                        {
                                            taskIndex = pageStartIndex;
                                            DisplayNewPage();
                                            await DisplayTasksPageAsync(tasksPage, pageNumber);
                                            goto BackToList;
                                        }

                                    case ConsoleKey.C:
                                        backToMenu = true;
                                        goto ExitAll;
                                }
                            }

                        BackToList:
                            continue;
                        }
                }
            }

        ReloadPage:
            continue;
        }

    ExitAll:
        return;
    }

    async Task SwitchTransportAsync()
    {
        DisplayNewPage();
        SelectInterface();
    }

    async Task DisplayTasksPageAsync(TaskQueryResult? page, uint pageNumber)
    {
        DisplayNewPage();
        if (page == null || page.Tasks.Count == 0) 
        {
            AnsiConsole.MarkupLine("[yellow]No tasks found.[/]");
            return;
        }
        var tasksTable = new Table()
            .Border(TableBorder.Rounded)
            .BorderColor(Color.Blue);
        tasksTable.AddColumn("[bold]Task ID[/]");
        tasksTable.AddColumn("[bold]Context ID[/]");
        tasksTable.AddColumn("[bold]Status[/]");
        tasksTable.AddColumn("[bold]Status Timestamp[/]");
        foreach (var task in page.Tasks) tasksTable.AddRow(task.Id, task.ContextId, $"[{GetTaskStatusColor(task.Status?.State)}]{task.Status?.State[11..] ?? "UNSPECIFIED"}[/]", task.Status?.Timestamp.ToString() ?? "N/A");
        var pageSize = 10;
        var totalCount = page.TotalSize;
        var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
        var footer = new Grid()
            .AddColumn(new GridColumn().NoWrap())
            .AddRow($"[dim]Page[/] [bold]{pageNumber}[/]/[bold]{totalPages}[/]  •  [dim]Total results:[/] [bold]{totalCount}[/]")
            .AddEmptyRow()
            .AddRow("[dim]Controls:[/] [bold]Up[/]=next page  •  [bold]Down[/]=previous page  •  [bold]Enter[/]=task details •  [bold]c[/]=back to menu");
        AnsiConsole.Write(new Panel(new Rows(tasksTable, footer))
            .Border(BoxBorder.Rounded)
            .BorderStyle(new Style(Color.Blue))
            .Padding(1, 0, 1, 0));
    }

    async Task<TaskQueryResult?> LoadTasksAsync(TaskQueryOptions queryOptions)
    {
        try
        {
            TaskQueryResult tasks = null!;
            await AnsiConsole.Status().StartAsync("Loading tasks...", async context =>
            {
                context.Spinner(Spinner.Known.Dots);
                tasks = await client!.ListTasksAsync(queryOptions);
            });
            return tasks;
        }
        catch (Exception ex)
        {
            DisplayError($"An error occurred while loading tasks: {ex.Message}");
            return null;
        }
    }

    static void DisplayError(string message)
    {
        AnsiConsole.MarkupLine($"[red]{message}[/]");
    }

    static void DisplayTask(A2A.Models.Task task, int taskIndex, int totalTasks)
    {
        static string Esc(string? s) => Markup.Escape(s ?? string.Empty);

        static string AggregateTextParts(IEnumerable<object>? parts)
        {
            if (parts == null) return string.Empty;

            var sb = new System.Text.StringBuilder();

            var haveEmittedAnyText = false;
            var newlinePendingBecauseOfNonText = false;

            foreach (var part in parts)
            {
                var pt = part?.GetType();
                var textProp = pt?.GetProperty("Text");
                var isTextPart = textProp != null && textProp.PropertyType == typeof(string);

                if (isTextPart)
                {
                    var text = (string?)textProp!.GetValue(part);
                    if (string.IsNullOrEmpty(text))
                        continue;

                    // Only insert a newline when a NON-text part was seen since the last emitted text
                    if (haveEmittedAnyText && newlinePendingBecauseOfNonText)
                        sb.AppendLine();

                    sb.Append(text);

                    haveEmittedAnyText = true;
                    newlinePendingBecauseOfNonText = false;
                }
                else
                {
                    // Mark a boundary ONLY if we've already emitted text;
                    // this boundary becomes a newline only if we later see more text.
                    if (haveEmittedAnyText)
                        newlinePendingBecauseOfNonText = true;
                }
            }

            return sb.ToString().Trim();
        }

        var renderables = new List<IRenderable>();

        // --- Summary table ---
        var summary = new Table()
            .Border(TableBorder.Rounded)
            .BorderColor(Color.Blue)
            .Expand();

        summary.AddColumn("[bold]Task ID[/]");
        summary.AddColumn("[bold]Context ID[/]");
        summary.AddColumn("[bold]Status[/]");
        summary.AddColumn("[bold]Status Timestamp[/]");

        summary.AddRow(
            Esc(task.Id),
            Esc(task.ContextId),
            $"[{GetTaskStatusColor(task.Status?.State)}]{Esc(task.Status?.State?[11..] ?? "UNSPECIFIED")}[/]",
            Esc(task.Status?.Timestamp.ToString() ?? "N/A")
        );

        renderables.Add(summary);

        // --- History messages ---
        var history = task.History;
        if (history != null && history.Count > 0)
        {
            renderables.Add(new Rule("[bold]History[/]").RuleStyle(new Style(Color.Grey)));

            var historyTable = new Table()
                .Border(TableBorder.Rounded)
                .BorderColor(Color.Blue)
                .Expand();

            historyTable.AddColumn(new TableColumn("[bold]Message ID[/]").NoWrap());
            historyTable.AddColumn(new TableColumn("[bold]Role[/]").NoWrap());
            historyTable.AddColumn(new TableColumn("[bold]Parts[/]").NoWrap());
            historyTable.AddColumn(new TableColumn("[bold]Text[/]"));

            foreach (var message in history)
            {
                var aggregatedText = AggregateTextParts(message.Parts);
                historyTable.AddRow(
                    Esc(message.MessageId),
                    Esc(message.Role[5..]),
                    Esc(message.Parts?.Count.ToString() ?? "0"),
                    Esc(string.IsNullOrWhiteSpace(aggregatedText) ? "—" : aggregatedText)
                );
            }

            renderables.Add(historyTable);
        }

        // --- Artifacts ---
        var artifacts = task.Artifacts; // adjust casing if needed
        if (artifacts != null && artifacts.Count > 0)
        {
            renderables.Add(new Rule("[bold]Artifacts[/]").RuleStyle(new Style(Color.Grey)));

            var artifactsTable = new Table()
                .Border(TableBorder.Rounded)
                .BorderColor(Color.Blue)
                .Expand();

            artifactsTable.AddColumn("[bold]Name[/]");
            artifactsTable.AddColumn("[bold]Parts[/]");
            artifactsTable.AddColumn("[bold]Text[/]");

            foreach (var artifact in artifacts)
            {
                var at = artifact?.GetType();

                var name =
                    (string?)at?.GetProperty("Name")?.GetValue(artifact) ??
                    (string?)at?.GetProperty("Id")?.GetValue(artifact) ??
                    "(unnamed)";

                var partsObj = at?.GetProperty("Parts")?.GetValue(artifact) as System.Collections.IEnumerable;
                var parts = partsObj?.Cast<object>().ToList() ?? new List<object>();

                var aggregatedText = AggregateTextParts(parts);

                artifactsTable.AddRow(
                    Esc(name),
                    Esc(parts.Count.ToString()),
                    Esc(string.IsNullOrWhiteSpace(aggregatedText) ? "—" : aggregatedText)
                );
            }

            renderables.Add(artifactsTable);
        }

        // --- Footer ---
        var footer = new Grid()
            .AddColumn(new GridColumn().NoWrap())
            .AddRow($"[dim]Task[/] [bold]{taskIndex}[/]/[bold]{totalTasks}[/]")
            .AddEmptyRow()
            .AddRow("[dim]Controls:[/] [bold]left[/]=previous task  •  [bold]right[/]=next task •  [bold]l[/]=back to list  •  [bold]c[/]=back to menu");

        renderables.Add(footer);

        AnsiConsole.Write(
            new Panel(new Rows(renderables.ToArray()))
                .Border(BoxBorder.Rounded)
                .BorderStyle(new Style(Color.Blue))
                .Padding(1, 0, 1, 0)
        );
    }

    static string GetTaskStatusColor(string? state) => state switch
    {
        TaskState.Completed => "green",
        TaskState.Failed or TaskState.Rejected => "red",
        TaskState.Cancelled => "yellow",
        TaskState.Unspecified => "dim",
        _ => "blue"
    };

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