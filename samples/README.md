# 🧪 Samples

Welcome to the `samples/` directory!  
This folder contains example projects that demonstrate how to use [a2a-net](#) to build, expose, and consume AI agents using the [Agent-to-Agent (A2A)](https://google.github.io/A2A/) protocol.

## 📦 Available Samples

- [Semantic Kernel](/samples/semantic-kernel/)  
  Demonstrates how to build and host an A2A-compatible agent using [Microsoft's Semantic Kernel](https://aka.ms/semantic-kernel) and OpenAI.  
  Includes:
  - a server that hosts the agent and exposes it via A2A and HTTP endpoints
  - a client that connects to the agent using the JSON-RPC protocol over HTTP
  - a clientt used to consume push notifications