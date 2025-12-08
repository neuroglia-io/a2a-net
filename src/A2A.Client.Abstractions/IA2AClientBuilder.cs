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
/// Defines the fundamentals of a service used to build an A2A client.
/// </summary>
public interface IA2AClientBuilder
{

    /// <summary>
    /// Gets the <see cref="IServiceCollection"/> to configure.
    /// </summary>
    IServiceCollection Services { get; }

    /// <summary>
    /// Configures the transport the <see cref="IA2AClient"/> will use.
    /// </summary>
    /// <typeparam name="TTransport">The type of <see cref="IA2AClientTransport"/> to use.</typeparam>
    /// <returns>The configured <see cref="IA2AClientBuilder"/>.</returns>
    IA2AClientBuilder UseTransport<TTransport>()
        where TTransport : class, IA2AClientTransport;

    /// <summary>
    /// Builds the configured <see cref="IA2AClient"/>.
    /// </summary>
    void Build();

}
