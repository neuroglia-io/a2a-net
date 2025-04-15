using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Neuroglia.A2A.IntegrationTests.Services;
using Neuroglia.A2A.Server;
using Neuroglia.A2A.Server.Infrastructure;
using Neuroglia.A2A.Server.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace Neuroglia.A2A.IntegrationTests.Cases;

public class TestWebApplicationStartup
    : StartupBase
{

    public override void ConfigureServices(IServiceCollection services)
    {
        services.AddLogging();
        services.AddDistributedMemoryCache();
        services.AddA2AProtocolHandler(builder =>
        {
            builder
                .UseAgentRuntime<MockAgentRuntime>()
                .UseDistributedCacheTaskRepository();
        });
    }

    public override void Configure(IApplicationBuilder app)
    {
        app.MapA2AEndpoint();
    }

}