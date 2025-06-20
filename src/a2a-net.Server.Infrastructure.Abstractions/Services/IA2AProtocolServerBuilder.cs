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

namespace A2A.Server.Infrastructure.Services;

/// <summary>
/// Defines the fundamentals of a service used to build and configure an A2A protocol server
/// </summary>
public interface IA2AProtocolServerBuilder
{

    /// <summary>
    /// Configures the <see cref="ServiceLifetime"/> of the <see cref="IA2AProtocolServer"/> to build
    /// </summary>
    /// <param name="lifetime">The <see cref="ServiceLifetime"/> of the <see cref="IA2AProtocolServer"/> to build</param>
    /// <returns>The configured <see cref="IA2AProtocolServerBuilder"/></returns>
    IA2AProtocolServerBuilder WithLifetime(ServiceLifetime lifetime);

    /// <summary>
    /// Configures the server to support streaming
    /// </summary>
    /// <returns>The configured <see cref="IA2AProtocolServerBuilder"/></returns>
    IA2AProtocolServerBuilder SupportsStreaming();

    /// <summary>
    /// Configures the server to support push notifications
    /// </summary>
    /// <returns>The configured <see cref="IA2AProtocolServerBuilder"/></returns>
    IA2AProtocolServerBuilder SupportsPushNotifications();

    /// <summary>
    /// Configures the server to support state transition history
    /// </summary>
    /// <returns>The configured <see cref="IA2AProtocolServerBuilder"/></returns>
    IA2AProtocolServerBuilder SupportsStateTransitionHistory();

    /// <summary>
    /// Configures the server to use the specified <see cref="IAgentRuntime"/>
    /// </summary>
    /// <typeparam name="TRuntime">The type of <see cref="IAgentRuntime"/> to use</typeparam>
    /// <returns>The configured <see cref="IA2AProtocolServerBuilder"/></returns>
    IA2AProtocolServerBuilder UseAgentRuntime<TRuntime>()
        where TRuntime : class, IAgentRuntime;

    /// <summary>
    /// Configures the server to use the specified <see cref="IAgentRuntime"/>
    /// </summary>
    /// <param name="factory">A <see cref="Func{T, TResult}"/> used to create the <see cref="IAgentRuntime"/> to use</param>
    /// <returns>The configured <see cref="IA2AProtocolServerBuilder"/></returns>
    IA2AProtocolServerBuilder UseAgentRuntime(Func<IServiceProvider, IAgentRuntime> factory);

    /// <summary>
    /// Configures the server to use the specified <see cref="ITaskEventStream"/>
    /// </summary>
    /// <typeparam name="TStream">The type of <see cref="ITaskEventStream"/> to use</typeparam>
    /// <returns>The configured <see cref="IA2AProtocolServerBuilder"/></returns>
    IA2AProtocolServerBuilder UseTaskEventStream<TStream>()
        where TStream : class, ITaskEventStream;

    /// <summary>
    /// Configures the server to use the specified <see cref="ITaskRepository"/>
    /// </summary>
    /// <typeparam name="TRepository">The type of <see cref="ITaskRepository"/> to use</typeparam>
    /// <returns>The configured <see cref="IA2AProtocolServerBuilder"/></returns>
    IA2AProtocolServerBuilder UseTaskRepository<TRepository>()
        where TRepository : class, ITaskRepository;

    /// <summary>
    /// Configures the server to use the specified <see cref="ITaskHandler"/>
    /// </summary>
    /// <typeparam name="THandler">The type of <see cref="ITaskHandler"/> to use</typeparam>
    /// <returns>The configured <see cref="IA2AProtocolServerBuilder"/></returns>
    IA2AProtocolServerBuilder UseTaskHandler<THandler>()
        where THandler : class, ITaskHandler;

    /// <summary>
    /// Configures the server to use the specified <see cref="IA2AProtocolServerBuilder"/>
    /// </summary>
    /// <typeparam name="TSender">The type of <see cref="IA2AProtocolServerBuilder"/> to use</typeparam>
    /// <returns>The configured <see cref="IA2AProtocolServerBuilder"/></returns>
    IA2AProtocolServerBuilder UsePushNotificationSender<TSender>()
        where TSender : class, IPushNotificationSender;

    /// <summary>
    /// Configures the type of the <see cref="IA2AProtocolServer"/> to build
    /// </summary>
    /// <typeparam name="TServer">The type of <see cref="IA2AProtocolServer"/> to build</typeparam>
    /// <returns>The configured <see cref="IA2AProtocolServerBuilder"/></returns>
    IA2AProtocolServerBuilder OfType<TServer>()
        where TServer : class, IA2AProtocolServer;

    /// <summary>
    /// Registers the configured services and builds the <see cref="IA2AProtocolServer"/>
    /// </summary>
    /// <returns>The configured <see cref="IServiceCollection"/></returns>
    IServiceCollection Build();

}
