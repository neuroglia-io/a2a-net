# 🧠 SemanticKernel Sample

This sample demonstrates how to use [a2a-net](https://github.com/neuroglia-io/a2a-net) with [Semantic Kernel](https://github.com/microsoft/semantic-kernel) and [OpenAI](https://platform.openai.com/docs/api-reference/chat/create) to build a simple agent-to-agent (A2A) interaction over HTTP using JSON-RPC.

It consists of two services:

- `a2a-net.Samples.SemanticKernel.Server` – Hosts the OpenAI-powered agent
- `a2a-net.Samples.SemanticKernel.Client` – Acts as a remote client invoking the agent

---

## 🛠️ Server Setup

The server exposes an AI agent using OpenAI's Chat Completion API and supports the [Agent2Agent protocol](https://github.com/google/A2A).

### 1. 🔐 Configure Your OpenAI API Key

You can provide the API key in one of two ways:

**Option 1: Use `appsettings.json`**

```
{
  "Agent": {
    "Kernel": {
      "ApiKey": "YOUR-OPENAI-API-KEY"
    }
  }
}
```

**Option 2: Use .NET user secrets (recommended)**

```
dotnet user-secrets set "Agent:Kernel:ApiKey" "YOUR-OPENAI-API-KEY"
```

This avoids hardcoding secrets in your codebase.

---

## 🌐 Agent Discovery

Once the server is running, it exposes its A2A discovery document at:

```
http://<hostname>:<port>/.well-known/agent.json
```

This discovery endpoint returns the [AgentCard](https://github.com/google/A2A/blob/main/specs/A2A.md#agentcard) used by remote clients to discover how to interact with the agent.

The agent itself is available at:

```
http://<hostname>:<port>/a2a
```

---

## 🤖 Client Usage

The client uses the agent's discovery endpoint and communicates using JSON-RPC over HTTP. It allows you to input prompts in the terminal and see the agent's streamed responses.

Start the client by running:

```
dotnet run --project ./samples/semantic-kernel/a2a-net.Samples.SemanticKernel.Client --server http://<hostname>:<port>
```

You will see a prompt like:

```
     _      ____       _        ____                   _                            _      ____   _               _
    / \    |___ \     / \      |  _ \   _ __    ___   | |_    ___     ___    ___   | |    / ___| | |__     __ _  | |_
   / _ \     __) |   / _ \     | |_) | | '__|  / _ \  | __|  / _ \   / __|  / _ \  | |   | |     | '_ \   / _` | | __|
  / ___ \   / __/   / ___ \    |  __/  | |    | (_) | | |_  | (_) | | (__  | (_) | | |   | |___  | | | | | (_| | | |_
 /_/   \_\ |_____| /_/   \_\   |_|     |_|     \___/   \__|  \___/   \___|  \___/  |_|    \____| |_| |_|  \__,_|  \__|

Type your prompts below. Press Ctrl+C to exit.

User>
```

Simply type your prompt and press Enter. The agent's response will be streamed back in real-time.

---

## 📂 Project Structure

```
samples/
└── semantic-kernel/
    ├── a2a-net.Samples.SemanticKernel.Server/   # A2A-enabled agent host
    └── a2a-net.Samples.SemanticKernel.Client/   # A2A JSON-RPC client
```

---

## 📖 Related Links

- [A2A Specification](https://github.com/google/A2A)
- [Semantic Kernel](https://github.com/microsoft/semantic-kernel)
- [OpenAI API](https://platform.openai.com/docs/api-reference/chat)

---
```
