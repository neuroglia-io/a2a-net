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

internal static class A2AServerFactory
{

    internal static IA2AServer Create(ILogger<A2AServer>? logger = null, IServiceProvider? serviceProvider = null, IA2APushNotificationSender? pushNotificationSender = null, 
        IA2AStore? store = null, IA2ATaskEventStream? taskEvents = null, IA2ATaskQueue? taskQueue = null, IA2AAgentRuntime? agent = null, AgentCard? agentCard = null)
    {
        logger ??= new Mock<ILogger<A2AServer>>().Object;
        serviceProvider ??= new ServiceCollection().BuildServiceProvider();
        pushNotificationSender ??= new Mock<IA2APushNotificationSender>().Object;
        store ??= new Mock<IA2AStore>().Object;
        taskEvents ??= new Mock<IA2ATaskEventStream>().Object;
        taskQueue ??= new Mock<IA2ATaskQueue>().Object;
        agent ??= new Mock<IA2AAgentRuntime>().Object;
        agentCard ??= AgentCardFactory.Create();
        return new A2AServer(logger, serviceProvider, pushNotificationSender, store, taskEvents, taskQueue, agent, agentCard);
    }

}
