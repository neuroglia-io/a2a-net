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

namespace A2A.Client;

/// <summary>
/// Represents the default implementation of the <see cref="IA2AClientBuilder"/> interface.
/// </summary>
public sealed class A2AClientBuilder 
    : IA2AClientBuilder
{

    Type? transportType;

    internal A2AClientBuilder(IServiceCollection services)
    {
        Services = services;
        Services.AddSingleton<IA2AClient, A2AClient>();
    }

    /// <inheritdoc/>
    public IServiceCollection Services { get; }

    /// <inheritdoc/>
    public IA2AClientBuilder UseTransport<TTransport>()
        where TTransport : class, IA2AClientTransport
    {
        transportType = typeof(TTransport);
        return this;
    }

    /// <inheritdoc/>
    public IA2AClient Build()
    {
        if (transportType is null) throw new InvalidOperationException("The transport type must be specified before building the client.");
        return Services.BuildServiceProvider().GetRequiredService<IA2AClient>();
    }

    /// <summary>
    /// Creates a new <see cref="IA2AClientBuilder"/>.
    /// </summary>
    /// <returns>A new <see cref="IA2AClientBuilder"/>.</returns>
    public static IA2AClientBuilder Create() => new A2AClientBuilder(new ServiceCollection());

}