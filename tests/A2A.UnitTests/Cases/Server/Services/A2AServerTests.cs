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

using Task = System.Threading.Tasks.Task;

namespace A2A.UnitTests.Cases.Server.Services;

public class A2AServerTests
{

    [Fact]
    public async Task SendMessage_With_Null_Request_Should_Throw()
    {
        //arrange
        var server = A2AServerFactory.Create();

        //act
        var task = () => server.SendMessageAsync(null!);

        //assert
        await task.Should().ThrowAsync<ArgumentNullException>();
    }

    [Fact]
    public async Task SendMessage_When_PushNotificationUrl_IsInvalid__Should_Throw()
    {
        //arrange
        var pushNotificationSender = new Mock<IA2APushNotificationSender>(MockBehavior.Strict);
        pushNotificationSender.Setup(p => p.VerifyPushNotificationUrlAsync(It.IsAny<Uri>(), It.IsAny<CancellationToken>())).ReturnsAsync(false);
        var server = A2AServerFactory.Create(pushNotificationSender: pushNotificationSender.Object);
        var request = SendMessageRequestFactory.Create();

        //act
        var task = () => server.SendMessageAsync(request);

        //assert
        await task.Should().ThrowAsync<A2AException>();
    }

    [Fact]
    public async Task SendMessage_For_NonExistingTask_Should_Throw()
    {
        //arrange
        var store = new Mock<IA2AStore>(MockBehavior.Strict);
        store.Setup(t => t.GetTaskAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>())).ReturnsAsync((Models.Task?)null);
        var server = A2AServerFactory.Create(store: store.Object);
        var request = SendMessageRequestFactory.Create();

        //act
        var task = () => server.SendMessageAsync(request);

        //assert
        await task.Should().ThrowAsync<A2AException>();
    }

}
