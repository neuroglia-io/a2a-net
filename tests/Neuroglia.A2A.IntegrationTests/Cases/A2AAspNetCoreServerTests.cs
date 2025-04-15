using Neuroglia.A2A.Models;
using Neuroglia.A2A.Requests;
using StreamJsonRpc;

namespace Neuroglia.A2A.IntegrationTests.Cases;

public class A2AAspNetCoreServerTests(A2AWebApplicationFactory factory)
    : IClassFixture<A2AWebApplicationFactory>
{

    [Fact]
    public async System.Threading.Tasks.Task SendTask_Should_Work()
    {
        //arrange
        var request = new SendTaskRequest()
        {
            Params = new()
            {
                Id = Guid.NewGuid().ToString("N"),
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
        var client = factory.Server.CreateWebSocketClient();
        var uri = new UriBuilder("ws", factory.Server.BaseAddress.Host, factory.Server.BaseAddress.Port, "a2a").Uri;
        var socket = await client.ConnectAsync(uri, default);
        using var jsonRpc = new JsonRpc(new WebSocketMessageHandler(socket, new SystemTextJsonFormatter()));

        //act
        jsonRpc.StartListening();
        var response = await jsonRpc.InvokeAsync<RpcResponse<Models.Task>>("tasks/send", request);

        //assert

    }

}
