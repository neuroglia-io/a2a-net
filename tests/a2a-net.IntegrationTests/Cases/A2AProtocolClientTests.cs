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

public class A2AProtocolClientTests
    : IClassFixture<A2AWebServerFactory>, IDisposable
{

    public A2AProtocolClientTests(A2AWebServerFactory webServerFactory)
    {
        WebServerFactory = webServerFactory;
        var services = new ServiceCollection();
        services.AddLogging();
        services.AddA2ProtocolClient(builder =>
        {
            builder.Services.Configure<JsonRpcWebSocketTransportFactoryOptions>(options =>
            {
                options.Endpoint = new UriBuilder("ws", WebServerFactory.Server.BaseAddress.Host, WebServerFactory.Server.BaseAddress.Port, "a2a").Uri;
            });
            builder.Services.AddSingleton(provider => WebServerFactory.Server.CreateWebSocketClient());
            builder.UseTransportFactory<JsonRpcTestWebSocketTransportFactory>();
        });
        ServiceProvider = services.BuildServiceProvider();
    }

    A2AWebServerFactory WebServerFactory { get; }

    ServiceProvider ServiceProvider { get; }

    IA2AProtocolClient Client => ServiceProvider.GetRequiredService<IA2AProtocolClient>();

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

        //act
        var response = await Client.SendTaskAsync(request);

        //assert
        response.Should().NotBeNull();
        response.Id.Should().Be(request.Id);
        response.Result.Should().NotBeNull();
        response.Result.Id.Should().Be(request.Params.Id);
    }

    void IDisposable.Dispose() => ServiceProvider.Dispose();

}
