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

- **`a2a-net.Client`**  
  Provides the default implementation of an A2A client for sending tasks and interacting with agents via JSON-RPC.

- **`a2a-net.Client.Transport.WebSocket`**  
  Implements the WebSocket transport for `a2a-net.Client`.  
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
dotnet add package a2a-net.Client.Transport.WebSocket
dotnet add package a2a-net.Server
dotnet add package a2a-net.Server.AspNetCore
```

### Configure the client

```csharp
services.AddA2ProtocolClient(builder =>
{
    builder.UseWebSocketTransport(options => 
    {
        options.Endpoint = new("ws://localhost/a2a");
    });
});
```

### Host an agent

#### Configure services

```csharp
services.AddDistributedMemoryCache();
services.AddA2AProtocolServer(builder =>
{
    builder
        .UseAgentRuntime<CustomAgentRuntime>()
        .UseDistributedCacheTaskRepository();
});
```

#### Map A2A Endpoints

```csharp
app.MapA2AEndpoint();
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
