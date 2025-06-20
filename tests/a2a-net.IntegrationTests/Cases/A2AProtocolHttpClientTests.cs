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

using A2A.Client.Configuration;

namespace A2A.IntegrationTests.Cases;

public class A2AProtocolHttpClientTests
    : IClassFixture<A2AWebServerFactory>, IDisposable
{

    public A2AProtocolHttpClientTests(A2AWebServerFactory webServerFactory)
    {
        WebServerFactory = webServerFactory;
        var services = new ServiceCollection();
        services.AddLogging();
        services.Configure<A2AProtocolClientOptions>(options =>
        {
            options.Endpoint = new UriBuilder("http", WebServerFactory.Server.BaseAddress.Host, WebServerFactory.Server.BaseAddress.Port, "a2a").Uri;
        });
        services.AddTransient<IA2AProtocolClient>(provider =>
        {
            return ActivatorUtilities.CreateInstance<A2AProtocolHttpClient>(provider, WebServerFactory.CreateClient());
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
        var request = new SendMessageRequest()
        {
            Params = new()
            {
                Message = new()
                {
                    TaskId = Guid.NewGuid().ToString("N"),
                    ContextId = Guid.NewGuid().ToString("N"),
                    MessageId = Guid.NewGuid().ToString("N"),
                    Role = MessageRole.User,
                    Parts =
                    [
                        new TextPart("tell me a joke")
                    ]
                }
            }
        };

        //act
        var response = await Client.SendMessageAsync(request);

        //assert
        response.Should().NotBeNull();
        response.Id.Should().Be(request.Id);
        response.Result.Should().NotBeNull();
        response.Result.Id.Should().Be(request.Params.Message.TaskId);
        response.Result.Artifacts.Should().NotBeNullOrEmpty();
    }

    [Fact]
    public async System.Threading.Tasks.Task SendTaskStreaming_Should_Work()
    {
        //arrange
        var request = new StreamMessageRequest()
        {
            Params = new()
            {
                Message = new()
                {
                    TaskId = Guid.NewGuid().ToString("N"),
                    ContextId = Guid.NewGuid().ToString("N"),
                    MessageId = Guid.NewGuid().ToString("N"),
                    Role = MessageRole.User,
                    Parts =
                    [
                        new TextPart("tell me a joke")
                    ]
                }
            }
        };

        //act
        var stream = Client.StreamMessageAsync(request);
        var events = await stream.ToListAsync();

        //assert
        stream.Should().NotBeNull();
        events.Should().NotBeNullOrEmpty();
    }

    [Fact]
    public async System.Threading.Tasks.Task Resubscribe_Should_Work()
    {
        //arrange
        var sendRequest = new SendMessageRequest()
        {
            Params = new()
            {
                Message = new()
                {
                    TaskId = Guid.NewGuid().ToString("N"),
                    ContextId = Guid.NewGuid().ToString("N"),
                    MessageId = Guid.NewGuid().ToString("N"),
                    Role = MessageRole.User,
                    Parts =
                    [
                        new TextPart("tell me a joke")
                    ]
                }
            }
        };
        var resubscribeRequest = new ResubscribeToTaskRequest()
        {
            Params = new()
            {
                Id = sendRequest.Params.Message.TaskId
            }
        };

        //act
        await Client.SendMessageAsync(sendRequest);
        var stream = Client.ResubscribeToTaskAsync(resubscribeRequest);

        //assert
        stream.Should().NotBeNull();
    }

    [Fact]
    public async System.Threading.Tasks.Task Get_Should_Work()
    {
        //arrange
        var sendRequest = new SendMessageRequest()
        {
            Params = new()
            {
                Message = new()
                {
                    TaskId = Guid.NewGuid().ToString("N"),
                    ContextId = Guid.NewGuid().ToString("N"),
                    MessageId = Guid.NewGuid().ToString("N"),
                    Role = MessageRole.User,
                    Parts =
                    [
                        new TextPart("tell me a joke")
                    ]
                }
            }
        };
        var getRequest = new GetTaskRequest()
        {
            Params = new()
            {
                Id = sendRequest.Params.Message.TaskId
            }
        };

        //act
        await Client.SendMessageAsync(sendRequest);
        var response = await Client.GetTaskAsync(getRequest);

        //assert
        response.Should().NotBeNull();
        response.Id.Should().Be(sendRequest.Id);
        response.Result.Should().NotBeNull();
        response.Result.Id.Should().Be(sendRequest.Params.Message.TaskId);
    }

    [Fact]
    public async System.Threading.Tasks.Task Cancel_Should_Work()
    {
        //arrange
        var sendRequest = new SendMessageRequest()
        {
            Params = new()
            {
                Message = new()
                {
                    TaskId = Guid.NewGuid().ToString("N"),
                    ContextId = Guid.NewGuid().ToString("N"),
                    MessageId = Guid.NewGuid().ToString("N"),
                    Role = MessageRole.User,
                    Parts =
                    [
                        new TextPart("tell me a joke")
                    ]
                }
            }
        };
        var cancelRequest = new CancelTaskRequest()
        {
            Params = new()
            {
                Id = sendRequest.Params.Message.TaskId
            }
        };

        //act
        await Client.SendMessageAsync(sendRequest);
        var response = await Client.CancelTaskAsync(cancelRequest);

        //assert
        response.Should().NotBeNull();
        response.Id.Should().Be(sendRequest.Id);
        response.Result.Should().NotBeNull();
        response.Result.Id.Should().Be(sendRequest.Params.Message.TaskId);
    }

    [Fact]
    public async System.Threading.Tasks.Task SetPushNotifications_Should_Work()
    {
        //arrange
        var sendRequest = new SendMessageRequest()
        {
            Params = new()
            {
                Message = new()
                {
                    TaskId = Guid.NewGuid().ToString("N"),
                    ContextId = Guid.NewGuid().ToString("N"),
                    MessageId = Guid.NewGuid().ToString("N"),
                    Role = MessageRole.User,
                    Parts =
                    [
                        new TextPart("tell me a joke")
                    ]
                }
            }
        };
        var setPushNotifications = new SetTaskPushNotificationsRequest()
        {
            Params = new()
            {
                Id = sendRequest.Params.Message.TaskId,
                PushNotificationConfig = new()
                {
                    Url = new(WebServerFactory.Server.BaseAddress, "/callback")
                }
            }
        };

        //act
        await Client.SendMessageAsync(sendRequest);
        var response = await Client.SetTaskPushNotificationsAsync(setPushNotifications);

        //assert
        response.Should().NotBeNull();
        response.Id.Should().Be(sendRequest.Id);
        response.Result.Should().NotBeNull();
        response.Result.Id.Should().Be(sendRequest.Params.Message.TaskId);
    }

    [Fact]
    public async System.Threading.Tasks.Task GetPushNotifications_Should_Work()
    {
        //arrange
        var sendRequest = new SendMessageRequest()
        {
            Params = new()
            {
                Message = new()
                {
                    TaskId = Guid.NewGuid().ToString("N"),
                    ContextId = Guid.NewGuid().ToString("N"),
                    MessageId = Guid.NewGuid().ToString("N"),
                    Role = MessageRole.User,
                    Parts =
                    [
                        new TextPart("tell me a joke")
                    ]
                }
            }
        };
        var setPushNotifications = new GetTaskPushNotificationsRequest()
        {
            Params = new()
            {
                Id = sendRequest.Params.Message.TaskId
            }
        };

        //act
        await Client.SendMessageAsync(sendRequest);
        var response = await Client.GetTaskPushNotificationsAsync(setPushNotifications);

        //assert
        response.Should().NotBeNull();
        response.Id.Should().Be(sendRequest.Id);
        response.Result.Should().NotBeNull();
        response.Result.Id.Should().Be(sendRequest.Params.Message.TaskId);
    }

    void IDisposable.Dispose()
    {
        ServiceProvider.Dispose();
        GC.SuppressFinalize(this);
    }

}
