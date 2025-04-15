using Microsoft.Extensions.DependencyInjection;
using Nerdbank.Streams;
using Neuroglia.A2A.IntegrationTests.Services;
using Neuroglia.A2A.Models;
using Neuroglia.A2A.Requests;
using Neuroglia.A2A.Server;
using Neuroglia.A2A.Server.Infrastructure;
using Neuroglia.A2A.Server.Infrastructure.Services;
using StreamJsonRpc;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using System.Net.WebSockets;
using System.Threading;
using Microsoft.AspNetCore.TestHost;

namespace Neuroglia.A2A.IntegrationTests.Cases;

public class A2AJsonRpcServerTests
    : IDisposable
{

    public A2AJsonRpcServerTests()
    {
        var services = new ServiceCollection();
        services.AddLogging();
        services.AddDistributedMemoryCache();
        services.AddA2AProtocolHandler(builder =>
        {
            builder
                .UseAgentRuntime<MockAgentRuntime>()
                .UseDistributedCacheTaskRepository();
        });
        ServiceProvider = services.BuildServiceProvider();
        var duplexStream = FullDuplexStream.CreatePair();
        RpcServer = new JsonRpc(new HeaderDelimitedMessageHandler(duplexStream.Item1, new SystemTextJsonFormatter()), ProtocolHandler);
        RpcClient = new JsonRpc(new HeaderDelimitedMessageHandler(duplexStream.Item2, new SystemTextJsonFormatter()), ProtocolHandler);
    }

    ServiceProvider ServiceProvider { get; }

    IA2AProtocolHandler ProtocolHandler => ServiceProvider.GetRequiredService<IA2AProtocolHandler>();

    JsonRpc RpcServer { get; }

    JsonRpc RpcClient { get; }

    [Fact]
    public async System.Threading.Tasks.Task SendTask_Should_Work()
    {
        //arrange
        RpcServer.StartListening();
        RpcClient.StartListening();
        var request = new SendTaskRequest()
        {
            Id = Guid.NewGuid().ToString("N"),
            Params = new()
            {
                Id = Guid.NewGuid().ToString("N"),
                SessionId = Guid.NewGuid().ToString("N"),
                Message = new()
                {
                    Role = MessageRole.User,
                    Parts = 
                    [
                        new TextPart()
                        {
                            Text = "tell me a joke"
                        }
                    ]
                }
            }
        };

        //act
        var response = await RpcClient.InvokeAsync<RpcResponse<Models.Task>>("tasks/send", request);

        //assert
    }

    void IDisposable.Dispose() => ServiceProvider.Dispose();

}
