// Copyright � 2025-Present Neuroglia SRL
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

namespace Neuroglia.A2A.Server.Infrastructure.Services;

/// <summary>
/// Defines the fundamentals of a service used to build and configure an A2A protocol server
/// </summary>
public interface IA2AProtocolServerBuilder
{

    /// <summary>
    /// Configures the server to use the specified <see cref="IAgentRuntime"/>
    /// </summary>
    /// <typeparam name="TRuntime">The type of <see cref="IAgentRuntime"/> to use</typeparam>
    /// <param name="serviceLifetime">The <see cref="IAgentRuntime"/>'s service lifetime</param>
    /// <returns>The configured <see cref="IA2AProtocolServerBuilder"/></returns>
    IA2AProtocolServerBuilder UseAgentRuntime<TRuntime>(ServiceLifetime serviceLifetime = ServiceLifetime.Singleton)
        where TRuntime : class, IAgentRuntime;

    /// <summary>
    /// Configures the server to use the specified <see cref="ITaskEventStream"/>
    /// </summary>
    /// <typeparam name="TStream">The type of <see cref="ITaskEventStream"/> to use</typeparam>
    /// <param name="serviceLifetime">The <see cref="ITaskEventStream"/>'s service lifetime</param>
    /// <returns>The configured <see cref="IA2AProtocolServerBuilder"/></returns>
    IA2AProtocolServerBuilder UseTaskEventStream<TStream>(ServiceLifetime serviceLifetime = ServiceLifetime.Singleton)
        where TStream : class, ITaskEventStream;

    /// <summary>
    /// Configures the server to use the specified <see cref="ITaskRepository"/>
    /// </summary>
    /// <typeparam name="TRepository">The type of <see cref="ITaskRepository"/> to use</typeparam>
    /// <param name="serviceLifetime">The <see cref="ITaskRepository"/>'s service lifetime</param>
    /// <returns>The configured <see cref="IA2AProtocolServerBuilder"/></returns>
    IA2AProtocolServerBuilder UseTaskRepository<TRepository>(ServiceLifetime serviceLifetime = ServiceLifetime.Singleton)
        where TRepository : class, ITaskRepository;

    /// <summary>
    /// Configures the server to use the specified <see cref="ITaskHandler"/>
    /// </summary>
    /// <typeparam name="THandler">The type of <see cref="ITaskHandler"/> to use</typeparam>
    /// <param name="serviceLifetime">The <see cref="ITaskHandler"/>'s service lifetime</param>
    /// <returns>The configured <see cref="IA2AProtocolServerBuilder"/></returns>
    IA2AProtocolServerBuilder UseTaskHandler<THandler>(ServiceLifetime serviceLifetime = ServiceLifetime.Singleton)
        where THandler : class, ITaskHandler;

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
