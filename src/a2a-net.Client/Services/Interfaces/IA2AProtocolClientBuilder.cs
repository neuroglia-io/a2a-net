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

namespace A2A.Client.Services;

/// <summary>
/// Defines the fundamentals of a service used to build <see cref="IA2AProtocolClient"/>s
/// </summary>
public interface IA2AProtocolClientBuilder
{

    /// <summary>
    /// Gets the <see cref="IServiceCollection"/> to configure
    /// </summary>
    IServiceCollection Services { get; }

    /// <summary>
    /// Configures the lifetime of the A2A protocol client and its dependencies
    /// </summary>
    /// <param name="lifetime">The <see cref="ServiceLifetime"/> to use</param>
    /// <returns>The configured <see cref="IA2AProtocolClientBuilder"/></returns>
    IA2AProtocolClientBuilder WithLifetime(ServiceLifetime lifetime);

    /// <summary>
    /// Configures the A2A protocol client to use the specified <see cref="IJsonRpcTransportFactory"/>
    /// </summary>
    /// <typeparam name="TFactory">The type of <see cref="IJsonRpcTransportFactory"/> to use</typeparam>
    /// <returns>The configured <see cref="IA2AProtocolClientBuilder"/></returns>
    IA2AProtocolClientBuilder UseTransportFactory<TFactory>()
        where TFactory : class, IJsonRpcTransportFactory;

    /// <summary>
    /// Configures the A2A protocol client to use the specified <see cref="IJsonRpcMessageFormatter"/>
    /// </summary>
    /// <typeparam name="TFormatter">The type of <see cref="IJsonRpcMessageFormatter"/> to use</typeparam>
    /// <returns>The configured <see cref="IA2AProtocolClientBuilder"/></returns>
    IA2AProtocolClientBuilder UseMessageFormatter<TFormatter>()
        where TFormatter : class, IJsonRpcMessageFormatter;

    /// <summary>
    /// Configures the type of the <see cref="IA2AProtocolClient"/> implementation to use
    /// </summary>
    /// <typeparam name="TClient">The type of the <see cref="IA2AProtocolClient"/> implementation to use</typeparam>
    /// <returns>The configured <see cref="IA2AProtocolClientBuilder"/></returns>
    IA2AProtocolClientBuilder OfType<TClient>()
        where TClient : class, IA2AProtocolClient;

    /// <summary>
    /// Builds the configured <see cref="IA2AProtocolClientBuilder"/>
    /// </summary>
    /// <returns>The configured <see cref="IServiceCollection"/></returns>
    IServiceCollection Build();

}
