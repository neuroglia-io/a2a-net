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
/// Defines the fundamentals of a service used to build <see cref="AgentExtension"/>.
/// </summary>
public interface IAgentExtensionBuilder
{

    /// <summary>
    /// Sets the URI that identifies the extension.
    /// </summary>
    /// <param name="uri">The URI of the extension.</param>
    /// <returns>The builder instance.</returns>
    IAgentExtensionBuilder WithUri(Uri uri);

    /// <summary>
    /// Indicates whether the extension is required by the agent.
    /// </summary>
    /// <param name="required">True if the extension is required; otherwise, false.</param>
    /// <returns>The builder instance.</returns>
    IAgentExtensionBuilder IsRequired(bool required = true);

    /// <summary>
    /// Sets a human-readable description of how the extension is used by the agent.
    /// </summary>
    /// <param name="description">The description of the extension's usage.</param>
    /// <returns>The builder instance.</returns>
    IAgentExtensionBuilder WithDescription(string description);

    /// <summary>
    /// Sets optional configuration parameters specific to the extension.
    /// </summary>
    /// <param name="parameters">An object representing the extension's configuration parameters.</param>
    /// <returns>The builder instance.</returns>
    IAgentExtensionBuilder WithParameters(object parameters);

    /// <summary>
    /// Builds and returns the configured <see cref="AgentExtension"/>.
    /// </summary>
    /// <returns>The constructed <see cref="AgentExtension"/>.</returns>
    AgentExtension Build();

}
