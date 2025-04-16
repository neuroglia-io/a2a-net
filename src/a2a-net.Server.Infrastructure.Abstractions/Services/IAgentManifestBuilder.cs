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

namespace A2A.Server.Infrastructure.Services;

/// <summary>
/// Defines the fundamentals of a service used to build A2A agent manifests (agent cards)
/// </summary>
public interface IAgentManifestBuilder
{

    /// <summary>
    /// Configures the agent's name
    /// </summary>
    /// <param name="name">The agent's name</param>
    /// <returns>The configured <see cref="IAgentManifestBuilder"/></returns>
    IAgentManifestBuilder WithName(string name);

    /// <summary>
    /// Configures the agent's description
    /// </summary>
    /// <param name="description">The agent's description</param>
    /// <returns>The configured <see cref="IAgentManifestBuilder"/></returns>
    IAgentManifestBuilder WithDescription(string description);

    /// <summary>
    /// Configures the URL referencing the address the agent is hosted at
    /// </summary>
    /// <param name="url">The URL referencing the address the agent is hosted at</param>
    /// <returns>The configured <see cref="IAgentManifestBuilder"/></returns>
    IAgentManifestBuilder WithUrl(Uri url);

    /// <summary>
    /// Configures the agent's provider
    /// </summary>
    /// <param name="provider">The agent's provider</param>
    /// <returns>The configured <see cref="IAgentManifestBuilder"/></returns>
    IAgentManifestBuilder WithProvider(AgentProvider provider);

    /// <summary>
    /// Configures the agent's provider
    /// </summary>
    /// <param name="setup">An <see cref="Action{T}"/> used to setup the agent's provider</param>
    /// <returns>The configured <see cref="IAgentManifestBuilder"/></returns>
    IAgentManifestBuilder WithProvider(Action<IAgentProviderManifestBuilder> setup);

    /// <summary>
    /// Configures the agent's version
    /// </summary>
    /// <param name="version">The agent's version</param>
    /// <returns>The configured <see cref="IAgentManifestBuilder"/></returns>
    IAgentManifestBuilder WithVersion(string version);

    /// <summary>
    /// Configures the URL referencing the agent's documentation
    /// </summary>
    /// <param name="url">The URL referencing the agent's documentation</param>
    /// <returns>The configured <see cref="IAgentManifestBuilder"/></returns>
    IAgentManifestBuilder WithDocumentationUrl(Uri url);

    /// <summary>
    /// Configures the agent to support streaming
    /// </summary>
    /// <returns>The configured <see cref="IAgentManifestBuilder"/></returns>
    IAgentManifestBuilder SupportsStreaming();

    /// <summary>
    /// Configures the agent to support push notifications
    /// </summary>
    /// <returns>The configured <see cref="IAgentManifestBuilder"/></returns>
    IAgentManifestBuilder SupportsPushNotifications();

    /// <summary>
    /// Configures the agent to support state transition history
    /// </summary>
    /// <returns>The configured <see cref="IAgentManifestBuilder"/></returns>
    IAgentManifestBuilder SupportsStateTransitionHistory();

    /// <summary>
    /// Builds the configured <see cref="AgentCard"/>
    /// </summary>
    /// <returns>A new <see cref="AgentCard"/></returns>
    AgentCard Build();

}
