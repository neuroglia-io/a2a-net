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

using System.Diagnostics.CodeAnalysis;

namespace A2A.Server.Services;

/// <summary>
/// Defines the fundamentals of a service used to configure and create an A2A server.
/// </summary>
public interface IA2AServerBuilder
{

    /// <summary>
    /// Gets the <see cref="IServiceCollection"/> to configure.
    /// </summary>
    IServiceCollection Services { get; }

    /// <summary>
    /// Configures the server to support streaming capabilities.
    /// </summary>
    /// <returns>The configured <see cref="IA2AServerBuilder"/>.</returns>
    IA2AServerBuilder SupportsStreaming();

    /// <summary>
    /// Configures the server to support push notifications.
    /// </summary>
    /// <returns>The configured <see cref="IA2AServerBuilder"/>.</returns>
    IA2AServerBuilder SupportsPushNotifications();

    /// <summary>
    /// Configures the server to support state transition history.
    /// </summary>
    /// <returns>The configured <see cref="IA2AServerBuilder"/>.</returns>
    IA2AServerBuilder SupportsStateTransitionHistory();

    /// <summary>
    /// Configures the server to use the specified <see cref="IA2ATaskEventStream"/> implementation.
    /// </summary>
    /// <typeparam name="TStream">The type of <see cref="IA2ATaskEventStream"/> to use.</typeparam>
    /// <returns>The configured <see cref="IA2AServerBuilder"/>.</returns>
    IA2AServerBuilder UseTaskEventStream<TStream>()
        where TStream : class, IA2ATaskEventStream;

    /// <summary>
    /// Configures the server to use the specified <see cref="IA2AStore"/> implementation.
    /// </summary>
    /// <typeparam name="TStore">The type of <see cref="IA2AStore"/> to use.</typeparam>
    /// <returns>The configured <see cref="IA2AServerBuilder"/>.</returns>
    IA2AServerBuilder UseStore<TStore>()
        where TStore : class, IA2AStore;

    /// <summary>
    /// Configures the server to use the specified <see cref="IA2ATaskQueue"/> implementation.
    /// </summary>
    /// <typeparam name="TQueue">The type of <see cref="IA2ATaskQueue"/> to use.</typeparam>
    /// <returns>The configured <see cref="IA2AServerBuilder"/>.</returns>
    IA2AServerBuilder UseTaskQueue<TQueue>()
        where TQueue : class, IA2ATaskQueue;

    /// <summary>
    /// Configures the server to use the specified <see cref="IA2AServerTransport"/> implementation.
    /// </summary>
    /// <typeparam name="TTransport">The type of <see cref="IA2AServerTransport"/> to use.</typeparam>
    /// <param name="protocolBinding">The transport's protocol binding.</param>
    /// <param name="path">The route path at which the transport will be made available.</param>
    /// <returns>The configured <see cref="IA2AServerBuilder"/>.</returns>
    IA2AServerBuilder UseTransport<TTransport>(string protocolBinding, [StringSyntax("Route")] string path)
        where TTransport : class, IA2AServerTransport;

    /// <summary>
    /// Configures the server to use the specified <see cref="IA2APushNotificationSender"/> implementation.
    /// </summary>
    /// <typeparam name="TSender">The type of <see cref="IA2APushNotificationSender"/> to use.</typeparam>
    /// <returns>The configured <see cref="IA2AServerBuilder"/>.</returns>
    IA2AServerBuilder UsePushNotificationSender<TSender>()
        where TSender : class, IA2APushNotificationSender;

    /// <summary>
    /// Configures and hosts an A2A agent using the specified setup action. 
    /// </summary>
    /// <param name="setup">A delegate that receives an agent definition builder used to configure the hosted agent. Cannot be null.</param>
    /// <returns>An instance of <see cref="IA2AServerBuilder"/> for further configuration or chaining.</returns>
    IA2AServerBuilder Host(Action<IA2AHostedAgentDefinitionBuilder> setup);

    /// <summary>
    /// Builds the configured A2A server and returns the underlying service collection.
    /// </summary>
    /// <returns>The configured <see cref="IServiceCollection"/>.</returns>
    IServiceCollection Build();

}
