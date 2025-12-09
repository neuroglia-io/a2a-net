# A2A-NET

**Agent-to-Agent (A2A)** is a lightweight, extensible protocol and framework for orchestrating tasks and exchanging structured content between autonomous agents using JSON-RPC 2.0.

[![Build Status](https://img.shields.io/github/actions/workflow/status/neuroglia-io/a2a-net/test.yml?branch=main)](https://github.com/neuroglia-io/a2a-net/actions)
[![Release](https://img.shields.io/github/v/release/neuroglia-io/a2a-net?include_prereleases)](https://github.com/neuroglia-io/a2a-net/releases)
[![NuGet](https://img.shields.io/nuget/v/A2A.Core.svg)](https://nuget.org/packages/A2A.Core)
[![License](https://img.shields.io/github/license/neuroglia-io/a2a-net)](LICENSE)

---

## 🧩 Projects

### 🧠 Core

- **`A2A.Core`**  
  Contains the core abstractions, models, contracts, and data types shared across both clients and servers.  
  Includes agent cards, messages, tasks, artifacts, capabilities, and JSON-RPC protocol definitions.  
  _This package is dependency-free and safe to use in any environment._

---

### 📡 Client

- **`A2A.Client.Abstractions`**  
  Defines the core interfaces and contracts for implementing A2A clients, including `IA2AClient` and protocol APIs.

- **`A2A.Client`**  
  Provides client-side functionality for A2A agent discovery and metadata resolution.  
  Includes utilities for retrieving agent cards and establishing connections.

- **`A2A.Client.Transports.Http`**  
  Implements the HTTP transport for the `IA2AClient`.  
  Enables agent-to-agent communication over HTTP using JSON-RPC 2.0.

- **`A2A.Client.Transports.Grpc`**  
  Implements the gRPC transport for the `IA2AClient`.  
  Enables persistent, bidirectional agent-to-agent communication over gRPC connections.

- **`A2A.Client.Transports.JsonRpc`**  
  Implements the JSON-RPC transport for the `IA2AClient`.  
  Enables persistent, bidirectional agent-to-agent communication over JSON-RPC connections.

---

### 🛠️ Server

- **`A2A.Server.Abstractions`**  
  Defines core server-side abstractions including `IA2AServer`, `IA2AAgentRuntime`, `IA2AStore`, task queuing, and event streaming interfaces.  
  Provides the foundation for building custom A2A-compatible agent implementations.

- **`A2A.Server`**  
  Core server components for building A2A-compatible agents.  
  Includes task execution orchestration, state management, event streaming, and agent runtime integration.

- **`A2A.Server.Transports.Http`**  
  Implements the HTTP transport for the `IA2AServer`.  
  Enables agent-to-agent communication over HTTP using JSON-RPC 2.0.

- **`A2A.Server.Transports.Grpc`**  
  Implements the gRPC transport for the `IA2AServer`.  
  Enables persistent, bidirectional agent-to-agent communication over gRPC connections.

- **`A2A.Server.Transports.JsonRpc`**  
  Implements the JSON-RPC transport for the `IA2AServer`.  
  Enables persistent, bidirectional agent-to-agent communication over JSON-RPC connections.

- **`A2A.Server.Persistence.Memory`**  
  In-memory implementation of `IA2AStore` for lightweight, ephemeral task state persistence.

- **`A2A.Server.Persistence.Redis`**
  [Redis](https://github.com/StackExchange/StackExchange.Redis)-based implementation of `IA2AStore` for distributed, scalable task state persistence.

- **`A2A.Server.Scheduling.Memory`**  
  In-memory task scheduling and queuing implementation.

- **`A2A.Server.Scheduling.Quartz`**  
  [Quartz](https://github.com/quartznet/quartznet)-based task scheduling and queuing implementation for advanced scheduling scenarios.

---

## 🚀 Getting Started

### Install the packages

```bash
dotnet add package A2A.Core 
dotnet add package A2A.Client.Abstractions
dotnet add package A2A.Client 
dotnet add package A2A.Client.Transports.Http 
dotnet add package A2A.Client.Transports.Grpc
dotnet add package A2A.Client.Transports.JsonRpc
dotnet add package A2A.Server.Abstractions
dotnet add package A2A.Server
dotnet add package A2A.Server.Transports.Http
dotnet add package A2A.Server.Transports.Grpc
dotnet add package A2A.Server.Transports.JsonRpc
dotnet add package A2A.Server.Persistence.Memory
dotnet add package A2A.Server.Persistence.Redis
dotnet add package A2A.Server.Scheduling.Memory
dotnet add package A2A.Server.Scheduling.Quartz
```

---

### Discover a remote agent

```c#
var discoveryDocument = await httpClient.GetA2ADiscoveryDocumentAsync(new Uri("http://localhost"));
```

---

### Configure and use a client

```c#
services.AddA2AClient(client => 
{
  client.UseHttpTransport(new Uri("http://localhost"));
  client.UseGrpcTransport(new Uri("http://localhost:5000"));
  client.UseJsonRpcTransport(new Uri("http://localhost:6000"));
});
```

---

### Host an agent

#### Configure services

```c#
builder.Services.AddA2AServer(server =>
{
    server
        .Host(agent => agent
            .WithCard(card => card
                .WithName("Sample Agent")
                .WithDescription("An example agent for demonstration purposes.")
                .WithVersion("1.0.0")
                .WithSkill(skill => skill
                    .WithName("Chat")
                    .WithDescription("A skill for engaging in conversations.")
                    .WithTag("chat"))
                .SupportsPushNotifications()
                .SupportsStreaming()
                .SupportsStateTransitionHistory())
                .UseRuntime<ChatAgent>())
            .UseMemoryStore()
            .UseMemoryTaskQueue()
            .UseHttpTransport()
            .UseGrpcTransport()
            .UseJsonRpcTransport("/json-rpc");
});
```

#### Configure the application

```c#
app.UseA2AServer();
```

#### Implement an agent runtime

```c#
public class ChatAgent(Kernel kernel)
    : IA2AAgentRuntime
{ 

    public Task<Models.Response> ProcessAsync(Models.Message message, CancellationToken cancellationToken = default)
    {
        var task = new Models.Task()
        {
            ContextId = message.ContextId ?? Guid.NewGuid().ToString("N"),
            History = [message]
        };
        return Task.FromResult<Models.Response>(task);
    }

    public async IAsyncEnumerable<Models.TaskEvent> ExecuteAsync(Models.Task task, [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        var chat = kernel.GetRequiredService<IChatClient>();
        var message = task.History?.LastOrDefault() ?? throw new NullReferenceException($"The history of the task with the specified id '{task.Id}' is null or empty.");
        var messageText = string.Join('\n', message.Parts.OfType<Models.TextPart>().Select(p => p.Text));
        var artifactId = Guid.NewGuid().ToString("N");
        var isFirstChunk = true;
        yield return new Models.TaskStatusUpdateEvent()
        {
            ContextId = task.ContextId,
            TaskId = task.Id,
            Status = new()
            {
                State = TaskState.Working,
                Message = new()
                {
                    ContextId = task.ContextId,
                    TaskId = task.Id,
                    Role = Role.Agent,
                    Parts =
                    [
                        new Models.TextPart()
                        {
                            Text = "Processing started by Semantic Kernel Chat Agent."
                        }
                    ]
                }
            }
        };
        var stopwatch = Stopwatch.StartNew();
        await foreach (var content in chat.GetStreamingResponseAsync(messageText, new(), cancellationToken))
        {
            yield return new Models.TaskArtifactUpdateEvent()
            {
                ContextId = task.ContextId,
                TaskId = task.Id,
                Artifact = new() 
                { 
                    ArtifactId = artifactId,
                    Parts = 
                    [            
                        new Models.TextPart() 
                        { 
                            Text = content.Text
                        }
                    ]
                },
                Append = !isFirstChunk
            };
            isFirstChunk = false;
        }
        stopwatch.Stop();
        yield return new Models.TaskStatusUpdateEvent()
        {
            ContextId = task.ContextId,
            TaskId = task.Id,
            Status = new()
            {
                State = TaskState.Completed,
                Message = new()
                {
                    ContextId = task.ContextId,
                    TaskId = task.Id,
                    Role = Role.Agent,
                    Parts =
                    [
                        new Models.TextPart()
                        {
                            Text = $"Processing completed in {stopwatch.ElapsedMilliseconds}ms."
                        }
                    ]
                }
            },
            Final = true
        };
    }

}
```

---

## 📚 Documentation

For a full overview of the A2A protocol, see [https://a2a-protocol.org/](https://a2a-protocol.org/).

---

## 🛠 Tools

Explore tools for working with the A2A protocol and ecosystem:

- **[Client Console](/tools/A2A.ClientConsole/)**
  A simple console application that demonstrates how to interact with A2A-compatible agents using the A2A-NET client.  
  It allows you to discover agents, send messages, and receive responses over different transports (HTTP, gRPC, JSON-RPC).

---

## 🧪 Samples

Explore sample projects demonstrating how to use the A2A-NET solution:

- **[Semantic Kernel](/samples/A2A.Samples.SemanticKernel.Server/)**  
  Demonstrates how to build and host an A2A-compatible agent using [Microsoft's Semantic Kernel](https://aka.ms/semantic-kernel) and `OpenAI`.  
  Includes both a server that exposes the agent and a client that interacts with it over `HTTP` using the `JSON-RPC` protocol.

---

## 🎯 Key Features

- **🔄 Protocol Compliance**: Full implementation of the A2A protocol
- **🚀 Multiple Transports**: HTTP, gRPC and JSON-RPC support for flexible communication patterns
- **📊 Task Management**: Complete lifecycle management for agent tasks with state persistence
- **🔔 Push Notifications**: Configurable push notification system for real-time updates
- **📡 Event Streaming**: Real-time task event streaming with artifact and status updates
- **🔌 Pluggable Architecture**: Extensible infrastructure with custom storage and event streaming backends
- **🎭 Agent Discovery**: Built-in agent card discovery and metadata resolution

---

## 🛡 License

This project is licensed under the [Apache-2.0 License](LICENSE).

---

## 🤝 Contributing

Contributions are welcome! Please open issues and PRs to help improve the ecosystem.

See [contribution guidelines](CONTRIBUTING.md) for more information on how to contribute.

---

## 🙏 Acknowledgments

This project implements the Agent-to-Agent (A2A) protocol specification.  
For more information about the A2A protocol, visit [https://a2a-protocol.org/](https://a2a-protocol.org/).