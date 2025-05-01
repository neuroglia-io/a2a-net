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

using A2A.Server.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;

namespace A2A.IntegrationTests;

public class A2AWebServerStartup
    : StartupBase
{

    public override void ConfigureServices(IServiceCollection services)
    {
        services.AddLogging();
        services.AddA2AWellKnownAgent((provider, agent) => agent
            .WithName("fake-agent-1")
            .WithDescription("fake-agent-1-description")
            .WithVersion("1.0.0")
            .WithUrl(new("http://localhost/a2a/fake-agent"))
            .WithProvider(provider => provider
                .WithOrganization("Neuroglia SRL")
                .WithUrl(new("https://neuroglia.io")))
            .WithSkill(skill => skill
                .WithId("fake-skill-id")
                .WithName("fake-skill-name")
                .WithDescription("fake-skill-description")));
        services.AddA2AWellKnownAgent((provider, agent) => agent
            .WithName("fake-agent-2")
            .WithDescription("fake-agent-2-description")
            .WithVersion("1.0.0")
            .WithUrl(new("http://localhost/a2a/fake-agent"))
            .WithProvider(provider => provider
                .WithOrganization("Neuroglia SRL")
                .WithUrl(new("https://neuroglia.io")))
            .WithSkill(skill => skill
                .WithId("fake-skill-id")
                .WithName("fake-skill-name")
                .WithDescription("fake-skill-description")));
        services.AddDistributedMemoryCache();
        services.AddA2AProtocolServer(builder =>
        {
            builder
                .SupportsStreaming()
                .SupportsPushNotifications()
                .SupportsStateTransitionHistory()
                .UseAgentRuntime<MockAgentRuntime>()
                .UseDistributedCacheTaskRepository()
                .UsePushNotificationSender<TestPushNotificationSender>();
        });
    }

    public override void Configure(IApplicationBuilder app)
    {
        app.UseRouting();
        app.MapA2AWellKnownAgentEndpoint();
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapA2AHttpEndpoint("/a2a");
            endpoints.MapA2AWebSocketEndpoint("/a2a/ws");
        });
    }

}
