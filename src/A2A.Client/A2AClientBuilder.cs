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
/// <param name="services">The <see cref="IServiceCollection"/> to configure.</param>
public sealed class A2AClientBuilder(IServiceCollection services)
    : IA2AClientBuilder
{

    Type? transportType;

    /// <inheritdoc/>
    public IServiceCollection Services { get; } = services;

    /// <inheritdoc/>
    public IA2AClientBuilder UseTransport<TTransport>()
        where TTransport : class, IA2AClientTransport
    {
        transportType = typeof(TTransport);
        return this;
    }

    /// <inheritdoc/>
    public void Build()
    {
        if (transportType is null) throw new InvalidOperationException("The transport type must be specified before building the client.");
        Services.AddSingleton(typeof(IA2AClientTransport), transportType);
        Services.AddSingleton<IA2AClient, A2AClient>();
    }

}