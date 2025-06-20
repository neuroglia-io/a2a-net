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
/// Defines the fundamentals of a service used to build the manifest of an agent's provider
/// </summary>
public interface IAgentProviderBuilder
{

    /// <summary>
    /// Configures the agent's provider name
    /// </summary>
    /// <param name="name">The agent's provider name</param>
    /// <returns>The configured <see cref="IAgentProviderBuilder"/></returns>
    IAgentProviderBuilder WithOrganization(string name);

    /// <summary>
    /// Configures the url referencing the official website of the agent's provider
    /// </summary>
    /// <param name="url">The url referencing the official website of the agent's provider</param>
    /// <returns>The configured <see cref="IAgentProviderBuilder"/></returns>
    IAgentProviderBuilder WithUrl(Uri url);

    /// <summary>
    /// Builds the configured <see cref="AgentProvider"/>
    /// </summary>
    /// <returns>A new <see cref="AgentProvider"/></returns>
    AgentProvider Build();

}