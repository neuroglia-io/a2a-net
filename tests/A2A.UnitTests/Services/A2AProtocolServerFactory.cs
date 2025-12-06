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

namespace A2A.UnitTests.Services;

internal static class A2AProtocolServerFactory
{

    internal static IA2AProtocolServer Create(ILogger<A2AProtocolServer>? logger = null, IServiceProvider? serviceProvider = null, IPushNotificationSender? pushNotificationSender = null, 
        IStateStore? store = null, ITaskEventStream? taskEvents = null, ITaskQueue? taskQueue = null, IAgentRuntime? agent = null)
    {
        logger ??= new Mock<ILogger<A2AProtocolServer>>().Object;
        serviceProvider ??= new ServiceCollection().BuildServiceProvider();
        pushNotificationSender ??= new Mock<IPushNotificationSender>().Object;
        store ??= new Mock<IStateStore>().Object;
        taskEvents ??= new Mock<ITaskEventStream>().Object;
        taskQueue ??= new Mock<ITaskQueue>().Object;
        agent ??= new Mock<IAgentRuntime>().Object;
        return new A2AProtocolServer(logger, serviceProvider, pushNotificationSender, store, taskEvents, taskQueue, agent);
    }

}