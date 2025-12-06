// Copyright © 2025-Present the a2a-net Authors
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

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddHttpContextAccessor();
builder.Services.AddKernel();
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
            .UseJsonRpcTransport("/json-rpc");
});

var app = builder.Build();
app.MapA2A();

await app.RunAsync();