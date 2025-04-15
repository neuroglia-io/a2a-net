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
/// Represents the default implementation of the <see cref="IA2AProtocolClientBuilder"/>
/// </summary>
/// <param name="services">The <see cref="IServiceCollection"/> to configure</param>
public class A2AProtocolClientBuilder(IServiceCollection services)
    : IA2AProtocolClientBuilder
{

    /// <summary>
    /// Gets the <see cref="IServiceCollection"/> to configure
    /// </summary>
    public IServiceCollection Services { get; } = services;

    /// <summary>
    /// Gets/sets the lifetime of the A2A protocol client and its dependencies
    /// </summary>
    protected ServiceLifetime ServiceLifetime { get; set; } = ServiceLifetime.Singleton;

    /// <summary>
    /// Gets the type of the <see cref="IJsonRpcTransportFactory"/> implementation to use
    /// </summary>
    protected Type? TransportFactoryType { get; set; }

    /// <summary>
    /// Gets the type of the <see cref="IJsonRpcMessageFormatter"/> implementation to use
    /// </summary>
    protected Type MessageFormatterType { get; set; } = typeof(SystemTextJsonFormatter);

    /// <summary>
    /// Gets the type of the <see cref="IA2AProtocolClient"/> implementation to use
    /// </summary>
    protected Type ClientType { get; set; } = typeof(A2AProtocolClient);

    /// <inheritdoc/>
    public virtual IA2AProtocolClientBuilder WithLifetime(ServiceLifetime lifetime)
    {
        ServiceLifetime = lifetime;
        return this;
    }

    /// <inheritdoc/>
    public virtual IA2AProtocolClientBuilder UseTransportFactory<TFactory>()
        where TFactory : class, IJsonRpcTransportFactory
    {
        TransportFactoryType = typeof(TFactory);
        return this;
    }

    /// <inheritdoc/>
    public virtual IA2AProtocolClientBuilder UseMessageFormatter<TFormatter>()
        where TFormatter : class, IJsonRpcMessageFormatter
    {
        MessageFormatterType = typeof(TFormatter);
        return this;
    }

    /// <inheritdoc/>
    public virtual IA2AProtocolClientBuilder OfType<TClient>()
        where TClient : class, IA2AProtocolClient
    {
        ClientType = typeof(TClient);
        return this;
    }

    /// <inheritdoc/>
    public virtual IServiceCollection Build()
    {
        if (TransportFactoryType == null) throw new NullReferenceException("The JSON-RPC transport factory must be set");
        Services.Add(new(typeof(IJsonRpcMessageFormatter), MessageFormatterType, ServiceLifetime));
        Services.Add(new(typeof(IJsonRpcTransportFactory), TransportFactoryType, ServiceLifetime));
        Services.Add(new(typeof(IA2AProtocolClient), ClientType, ServiceLifetime));
        return Services;
    }

}