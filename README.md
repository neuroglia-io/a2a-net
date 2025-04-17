# A2A-NET

**Agent-to-Agent (A2A)** is a lightweight, extensible protocol and framework for orchestrating tasks and exchanging structured content between autonomous agents using JSON-RPC 2.0.

This repository provides a complete set of libraries and components for building, hosting, and communicating with A2A-compliant agents across various transport layers.

---

## 🧩 Projects

### 🧠 Core

- **`a2a-net.Core`**  
  Contains the core abstractions, models, contracts, and data types shared across both clients and servers.  
  _This package is dependency-free and safe to use in any environment._

---

### 📡 Client

- **`a2a-net.Client.Abstractions`**  
  Contains core interfaces and contracts for implementing A2A clients.

- **`a2a-net.Client`**  
  Includes client-side functionality for A2A agent discovery and metadata resolution.

- **`a2a-net.Client.Http`**  
  Implements the HTTP transport for `IA2AProtocolClient`
  Allows establishing persistent agent-to-agent communication over HTTP connections.

- **`a2a-net.Client.WebSocket`**  
  Implements the WebSocket transport for `IA2AProtocolClient`
  Allows establishing persistent agent-to-agent communication over WebSocket connections.

---

### 🛠️ Server

- **`a2a-net.Server`**  
  Core components for building A2A-compatible agents.  
  Includes task execution, state management, event streaming, and runtime integration.

- **`a2a-net.Server.AspNetCore`**  
  ASP.NET Core integration layer that allows hosting A2A endpoints over WebSocket using JSON-RPC.  
  Provides middleware, routing, and server bootstrap extensions.

---

### 🧱 Server Infrastructure

- **`a2a-net.Server.Infrastructure.Abstractions`**  
  Defines abstractions for task persistence, event streaming, and other infrastructure concerns.  
  Enables support for custom and pluggable storage/event backends.

- **`a2a-net.Server.Infrastructure.DistributedCache`**  
  Distributed cache–based implementation of A2A task storage using `IDistributedCache`.  
  Useful for scenarios that require scalable, lightweight task state persistence.

---

## 🚀 Getting Started

### Install the packages

```
dotnet add package a2a-net.Client
dotnet add package a2a-net.Client.Http
dotnet add package a2a-net.Client.WebSocket
dotnet add package a2a-net.Server.Infrastructure.DistributedCache
dotnet add package a2a-net.Server.AspNetCore
```

### Discover a remote agent

```csharp
 var discoveryDocument = await httpClient.GetA2ADiscoveryDocumentAsync(new Uri("http://localhost"));
```

### Configure and use a client

```csharp
services.AddA2ProtocolHttpClient(options => 
{
    options.Endpoint = new("http://localhost/a2a");
});
```

```csharp
services.AddA2ProtocolWebSocketClient(options => 
{
    options.Endpoint = new("ws://localhost/a2a");
});
```

```csharp
var request = new SendTaskRequest()
{
    Params = new()
    {
        Message = new()
        {
            Role = MessageRole.User,
            Parts =
            [
                new TextPart("tell me a joke")
            ]
        }
    }
};
var response = await Client.SendTaskAsync(request);
```

### Host an agent

#### Configure services

```csharp
services.AddDistributedMemoryCache();
services.AddA2AProtocolServer(builder =>
{
    builder
        .SupportsStreaming()
        .SupportsPushNotifications()
        .SupportsStateTransitionHistory()
        .UseAgentRuntime<MockAgentRuntime>()
        .UseDistributedCacheTaskRepository();
});
```

#### Map A2A Endpoints

```csharp
app.MapA2AAgentHttpEndpoint("/a2a");
app.MapA2AAgentWebSocketEndpoint("/a2a/ws")
```

---

## 📚 Documentation

For a full overview of the A2A protocol, see [google.github.io/A2A](https://google.github.io/A2A/#/documentation)

---

## 🛡 License

This project is licensed under the [Apache-2.0 License](LICENSE).

---

## 🤝 Contributing

Contributions are welcome! Please open issues and PRs to help improve the ecosystem.

See [contribution guidelines](CONTRIBUTING.md) for more information on how to contribute.
