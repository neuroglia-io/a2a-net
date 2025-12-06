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

namespace A2A.Services;

/// <summary>
/// Represents the default implementation of the <see cref="IAgentExtensionBuilder"/> interface.
/// </summary>
public sealed class AgentExtensionBuilder
    : IAgentExtensionBuilder
{

    readonly AgentExtension extension = new();

    /// <inheritdoc/>
    public IAgentExtensionBuilder WithUri(Uri uri)
    {
        ArgumentNullException.ThrowIfNull(uri);
        extension.Uri = uri;
        return this;
    }

    /// <inheritdoc/>
    public IAgentExtensionBuilder IsRequired(bool required = true)
    {
        extension.Required = required;
        return this;
    }

    /// <inheritdoc/>
    public IAgentExtensionBuilder WithDescription(string description)
    {
        extension.Description = description;
        return this;
    }

    /// <inheritdoc/>
    public IAgentExtensionBuilder WithParameters(IDictionary<string, JsonNode> parameters)
    {
        ArgumentNullException.ThrowIfNull(parameters);
        extension.Params = parameters;
        return this;
    }

    /// <inheritdoc/>
    public AgentExtension Build() => extension;
}