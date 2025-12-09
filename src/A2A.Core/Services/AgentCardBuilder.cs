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
/// Represents the default implementation of the <see cref="IAgentCardBuilder"/> interface.
/// </summary>
public sealed class AgentCardBuilder
    : IAgentCardBuilder
{

    readonly AgentCard card = new()
    {
        Capabilities = new()
    };

    /// <inheritdoc/>
    public IAgentCardBuilder WithName(string name)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        card.Name = name;
        return this;
    }

    /// <inheritdoc/>
    public IAgentCardBuilder WithDescription(string description)
    {
        card.Description = description;
        return this;
    }

    /// <inheritdoc/>
    public IAgentCardBuilder WithVersion(string version)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(version);
        card.Version = version;
        return this;
    }

    /// <inheritdoc/>
    public IAgentCardBuilder WithIconUrl(Uri url)
    {
        ArgumentNullException.ThrowIfNull(url);
        card.IconUrl = url;
        return this;
    }

    /// <inheritdoc/>
    public IAgentCardBuilder WithDocumentationUrl(Uri url)
    {
        card.DocumentationUrl = url;
        return this;
    }

    /// <inheritdoc/>
    public IAgentCardBuilder WithProvider(AgentProvider provider)
    {
        card.Provider = provider;
        return this;
    }

    /// <inheritdoc/>
    public IAgentCardBuilder WithProvider(Action<IAgentProviderBuilder> setup)
    {
        ArgumentNullException.ThrowIfNull(setup);
        var builder = new AgentProviderBuilder();
        setup(builder);
        card.Provider = builder.Build();
        return this;
    }

    /// <inheritdoc/>
    public IAgentCardBuilder WithSecurityScheme(string name, SecurityScheme scheme)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        ArgumentNullException.ThrowIfNull(scheme);
        card.SecuritySchemes ??= new Dictionary<string, SecurityScheme>();
        card.SecuritySchemes[name] = scheme;
        return this;
    }

    /// <inheritdoc/>
    public IAgentCardBuilder WithSecurityScheme(string name, Action<IGenericSecuritySchemeBuilder> setup)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        ArgumentNullException.ThrowIfNull(setup);
        var builder = new GenericSecuritySchemeBuilder();
        return WithSecurityScheme(name, builder.Build());
    }

    /// <inheritdoc/>
    public IAgentCardBuilder WithSecurityRequirement(string schemeName, IEnumerable<string> scopes)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(schemeName);
        ArgumentNullException.ThrowIfNull(scopes);
        card.Security ??= [];
        var existing = card.Security.FirstOrDefault(e => e.ContainsKey(schemeName));
        if (existing != null) card.Security.Remove(existing);
        card.Security.Add(new Dictionary<string, string[]>()
        {
            [schemeName] = [.. scopes]
        });
        return this;
    }

    /// <inheritdoc/>
    public IAgentCardBuilder SupportsPushNotifications()
    {
        card.Capabilities ??= new();
        card.Capabilities.PushNotifications = true;
        return this;
    }

    /// <inheritdoc/>
    public IAgentCardBuilder SupportsStateTransitionHistory()
    {
        card.Capabilities ??= new();
        card.Capabilities.StateTransitionHistory = true;
        return this;
    }

    /// <inheritdoc/>
    public IAgentCardBuilder SupportsStreaming()
    {
        card.Capabilities ??= new();
        card.Capabilities.Streaming = true;
        return this;
    }

    /// <inheritdoc/>
    public IAgentCardBuilder WithExtension(AgentExtension extension)
    {
        ArgumentNullException.ThrowIfNull(extension);
        card.Capabilities ??= new();
        card.Capabilities.Extensions ??= [];
        card.Capabilities.Extensions.Add(extension);
        return this;
    }

    /// <inheritdoc/>
    public IAgentCardBuilder WithExtension(Action<IAgentExtensionBuilder> setup)
    {
        ArgumentNullException.ThrowIfNull(setup);
        var builder = new AgentExtensionBuilder();
        setup(builder);
        return WithExtension(builder.Build());
    }

    /// <inheritdoc/>
    public IAgentCardBuilder WithDefaultInputMode(string contentType)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(contentType);
        card.DefaultInputModes ??= [];
        if (!card.DefaultInputModes.Contains(contentType)) card.DefaultInputModes.Add(contentType);
        return this;
    }

    /// <inheritdoc/>
    public IAgentCardBuilder WithDefaultOutputMode(string contentType)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(contentType);
        card.DefaultOutputModes ??= [];
        if (!card.DefaultOutputModes.Contains(contentType)) card.DefaultOutputModes.Add(contentType);
        return this;
    }

    /// <inheritdoc/>
    public IAgentCardBuilder WithSkill(Action<IAgentSkillBuilder> setup)
    {
        ArgumentNullException.ThrowIfNull(setup);
        var builder = new AgentSkillBuilder();
        setup(builder);
        card.Skills ??= [];
        card.Skills.Add(builder.Build());
        return this;
    }

    /// <inheritdoc/>
    public AgentCard Build() => card;

}