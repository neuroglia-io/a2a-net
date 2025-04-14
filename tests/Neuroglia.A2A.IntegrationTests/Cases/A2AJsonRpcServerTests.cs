using Nerdbank.Streams;
using Neuroglia.A2A.Models;
using Neuroglia.A2A.Requests;
using Neuroglia.A2A.Server.Services;
using StreamJsonRpc;

namespace Neuroglia.A2A.IntegrationTests.Cases;

public class A2AJsonRpcServerTests
{

    [Fact]
    public async System.Threading.Tasks.Task SendTask_Should_Work()
    {
        var duplexStream = FullDuplexStream.CreatePair();
        var rpcServer = new JsonRpc(new HeaderDelimitedMessageHandler(duplexStream.Item1, new SystemTextJsonFormatter()), new A2AJsonRpcServer());
        rpcServer.StartListening();
        var rpcClient = new JsonRpc(new HeaderDelimitedMessageHandler(duplexStream.Item2, new SystemTextJsonFormatter()), new A2AJsonRpcServer());
        rpcClient.StartListening();
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
        var response = await rpcClient.InvokeAsync<RpcResponse<Models.Task>>("tasks/send", request);
    }

    [Fact]
    public async System.Threading.Tasks.Task SendTaskSubscribe_Should_Work()
    {
        var duplexStream = FullDuplexStream.CreatePair();
        var rpcServer = new JsonRpc(new HeaderDelimitedMessageHandler(duplexStream.Item1, new SystemTextJsonFormatter()), new A2AJsonRpcServer());
        rpcServer.StartListening();
        var rpcClient = new JsonRpc(new HeaderDelimitedMessageHandler(duplexStream.Item2, new SystemTextJsonFormatter()), new A2AJsonRpcServer());
        rpcClient.StartListening();
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
        var stream = await rpcClient.InvokeAsync<IAsyncEnumerable<RpcEvent>>("tasks/sendSubscribe", request);
        await foreach(var e in stream)
        {

        }
    }


}
