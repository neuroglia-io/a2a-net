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

namespace A2A.IntegrationTests.Cases;

public class A2AProtocolServerTests
    : IDisposable
{

    public A2AProtocolServerTests()
    {
        var services = new ServiceCollection();
        services.AddLogging();
        services.AddDistributedMemoryCache();
        services.AddA2AProtocolServer(builder =>
        {
            builder
                .UseAgentRuntime<MockAgentRuntime>()
                .UseDistributedCacheTaskRepository();
        });
        ServiceProvider = services.BuildServiceProvider();
        var duplexStream = FullDuplexStream.CreatePair();
        RpcServer = new JsonRpc(new HeaderDelimitedMessageHandler(duplexStream.Item1, new SystemTextJsonFormatter()), Server);
        RpcClient = new JsonRpc(new HeaderDelimitedMessageHandler(duplexStream.Item2, new SystemTextJsonFormatter()), Server);
    }

    ServiceProvider ServiceProvider { get; }

    IA2AProtocolServer Server => ServiceProvider.GetRequiredService<IA2AProtocolServer>();

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
        response.Should().NotBeNull();
        response.Id.Should().Be(request.Id);
        response.Result.Should().NotBeNull();
        response.Result.Id.Should().Be(request.Params.Id);
    }

    void IDisposable.Dispose() => ServiceProvider.Dispose();

}
