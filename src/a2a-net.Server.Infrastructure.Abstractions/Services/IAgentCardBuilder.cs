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
/// Defines the fundamentals of a service used to build A2A agent cards
/// </summary>
public interface IAgentCardBuilder
{

    /// <summary>
    /// Configures the agent's name
    /// </summary>
    /// <param name="name">The agent's name</param>
    /// <returns>The configured <see cref="IAgentCardBuilder"/></returns>
    IAgentCardBuilder WithName(string name);

    /// <summary>
    /// Configures the agent's description
    /// </summary>
    /// <param name="description">The agent's description</param>
    /// <returns>The configured <see cref="IAgentCardBuilder"/></returns>
    IAgentCardBuilder WithDescription(string description);

    /// <summary>
    /// Configures the URL referencing the address the agent is hosted at
    /// </summary>
    /// <param name="url">The URL referencing the address the agent is hosted at</param>
    /// <returns>The configured <see cref="IAgentCardBuilder"/></returns>
    IAgentCardBuilder WithUrl(Uri url);

    /// <summary>
    /// Configures the URL referencing the agent's icon.
    /// </summary>
    /// <param name="url">The URL referencing the agent's icon</param>
    /// <returns>The configured <see cref="IAgentCardBuilder"/>.</returns>
    IAgentCardBuilder WithIconUrl(Uri url);

    /// <summary>
    /// Configures the agent's provider
    /// </summary>
    /// <param name="provider">The agent's provider</param>
    /// <returns>The configured <see cref="IAgentCardBuilder"/></returns>
    IAgentCardBuilder WithProvider(AgentProvider provider);

    /// <summary>
    /// Configures the agent's provider
    /// </summary>
    /// <param name="setup">An <see cref="Action{T}"/> used to setup the agent's provider</param>
    /// <returns>The configured <see cref="IAgentCardBuilder"/></returns>
    IAgentCardBuilder WithProvider(Action<IAgentProviderBuilder> setup);

    /// <summary>
    /// Configures the agent's version
    /// </summary>
    /// <param name="version">The agent's version</param>
    /// <returns>The configured <see cref="IAgentCardBuilder"/></returns>
    IAgentCardBuilder WithVersion(string version);

    /// <summary>
    /// Configures the URL referencing the agent's documentation
    /// </summary>
    /// <param name="url">The URL referencing the agent's documentation</param>
    /// <returns>The configured <see cref="IAgentCardBuilder"/></returns>
    IAgentCardBuilder WithDocumentationUrl(Uri url);

    /// <summary>
    /// Configures the agent to support the specified security scheme.
    /// </summary>
    /// <param name="name"> The name of the security scheme to add</param>
    /// <param name="scheme"> The security scheme to add</param>
    /// <returns>The configured <see cref="IAgentCardBuilder"/></returns>
    IAgentCardBuilder WithSecurityScheme(string name, SecurityScheme scheme);

    /// <summary>
    /// Configures the agent to support the specified security scheme.
    /// </summary>
    /// <param name="name">The name of the security scheme to add</param>
    /// <param name="setup">An <see cref="Action{T}"/> used to build the security scheme to add</param>
    /// <returns>The configured <see cref="IAgentCardBuilder"/></returns>
    IAgentCardBuilder WithSecurityScheme(string name, Action<IGenericSecuritySchemeBuilder> setup);

    /// <summary>
    /// Adds a security requirement that must be satisfied to access the agent's resources.
    /// </summary>
    /// <param name="schemeName">The name of the security scheme as defined in the agent's security schemes.</param>
    /// <param name="scopes">The scopes required for this scheme. May be empty if the scheme does not require scopes.</param>
    /// <returns>The configured <see cref="IAgentCardBuilder"/>.</returns>
    IAgentCardBuilder WithSecurityRequirement(string schemeName, IEnumerable<string> scopes);

    /// <summary>
    /// Configures the agent to support streaming
    /// </summary>
    /// <returns>The configured <see cref="IAgentCardBuilder"/></returns>
    IAgentCardBuilder SupportsStreaming();

    /// <summary>
    /// Configures the agent to support push notifications
    /// </summary>
    /// <returns>The configured <see cref="IAgentCardBuilder"/></returns>
    IAgentCardBuilder SupportsPushNotifications();

    /// <summary>
    /// Configures the agent to support state transition history
    /// </summary>
    /// <returns>The configured <see cref="IAgentCardBuilder"/></returns>
    IAgentCardBuilder SupportsStateTransitionHistory();

    /// <summary>
    /// Configures the agent to support the specified extension.
    /// </summary>
    /// <param name="extension">The extension to support.</param>
    /// <returns>The configured <see cref="IAgentCardBuilder"/></returns>
    IAgentCardBuilder WithExtension(AgentExtension extension);

    /// <summary>
    /// Configures the agent to support the specified extension.
    /// </summary>
    /// <param name="setup">An <see cref="Action{T}"/> used to configure the extension to support.</param>
    /// <returns>The configured <see cref="IAgentCardBuilder"/></returns>
    IAgentCardBuilder WithExtension(Action<IAgentExtensionBuilder> setup);

    /// <summary>
    /// Configures the agent to support the specified MIME type as input
    /// </summary>
    /// <param name="contentType">The MIME type to support</param>
    /// <returns>The configured <see cref="IAgentCardBuilder"/></returns>
    IAgentCardBuilder WithDefaultInputMode(string contentType);

    /// <summary>
    /// Configures the agent to support the specified MIME type as output
    /// </summary>
    /// <param name="contentType">The MIME type to support</param>
    /// <returns>The configured <see cref="IAgentCardBuilder"/></returns>
    IAgentCardBuilder WithDefaultOutputMode(string contentType);

    /// <summary>
    /// Configures the agent by granting it the specified skill
    /// </summary>
    /// <param name="setup">An <see cref="Action{T}"/> used to configure the skill to add</param>
    /// <returns>The configured <see cref="IAgentCardBuilder"/></returns>
    IAgentCardBuilder WithSkill(Action<IAgentSkillBuilder> setup);

    /// <summary>
    /// Builds the configured <see cref="AgentCard"/>
    /// </summary>
    /// <returns>A new <see cref="AgentCard"/></returns>
    AgentCard Build();

}
