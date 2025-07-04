﻿// Copyright © 2025-Present the a2a-net Authors
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
/// Represents the default implementation of the <see cref="IAgentExtensionBuilder"/> interface.
/// </summary>
public class AgentExtensionBuilder
    : IAgentExtensionBuilder
{

    /// <summary>
    /// Gets the <see cref="AgentExtension"/> to build and configure.
    /// </summary>
    protected AgentExtension Extension { get; } = new();

    /// <inheritdoc/>
    public virtual IAgentExtensionBuilder WithUri(Uri uri)
    {
        ArgumentNullException.ThrowIfNull(uri);
        Extension.Uri = uri;
        return this;
    }

    /// <inheritdoc/>
    public virtual IAgentExtensionBuilder IsRequired(bool required = true)
    {
        Extension.Required = required;
        return this;
    }

    /// <inheritdoc/>
    public virtual IAgentExtensionBuilder WithDescription(string description)
    {
        Extension.Description = description;
        return this;
    }

    /// <inheritdoc/>
    public virtual IAgentExtensionBuilder WithParameters(object parameters)
    {
        ArgumentNullException.ThrowIfNull(parameters);
        Extension.Params = parameters;
        return this;
    }

    /// <inheritdoc/>
    public virtual AgentExtension Build() => Extension;
}