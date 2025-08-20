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

namespace A2A.Models;

/// <summary>
/// Represents an object used to describe an agent’s capabilities/skills and authentication mechanism.
/// </summary>
[Description("An object used to describe an agent's capabilities/skills and authentication mechanism.")]
[DataContract]
public record AgentCard
{

    /// <summary>
    /// Gets or sets the agent's human readable name.
    /// </summary>
    [Description("The agent's human readable name.")]
    [Required, MinLength(1)]
    [DataMember(Name = "name", Order = 1), JsonPropertyName("name"), JsonPropertyOrder(1), YamlMember(Alias = "name", Order = 1)]
    public virtual string Name { get; set; } = null!;

    /// <summary>
    /// Gets or sets a human-readable description of the agent, if any.<para></para>
    /// Used to assist users and other agents in understanding what the agent can do.
    /// </summary>
    [Description("A human-readable description of the agent, if any. Used to assist users and other agents in understanding what the agent can do.")]
    [DataMember(Name = "description", Order = 2), JsonPropertyName("description"), JsonPropertyOrder(2), YamlMember(Alias = "description", Order = 2)]
    public virtual string? Description { get; set; }

    /// <summary>
    /// Gets or sets the URL referencing the address the agent is hosted at.
    /// </summary>
    [Description("The URL referencing the address the agent is hosted at.")]
    [Required]
    [DataMember(Name = "url", Order = 3), JsonPropertyName("url"), JsonPropertyOrder(3), YamlMember(Alias = "url", Order = 3)]
    public virtual Uri Url { get; set; } = null!;

    /// <summary>
    /// Gets or sets an url, if any, referencing an icon representing the agent.
    /// </summary>
    [Description("An url, if any, referencing an icon representing the agent.")]
    [DataMember(Name = "iconUrl", Order = 4), JsonPropertyName("iconUrl"), JsonPropertyOrder(4), YamlMember(Alias = "iconUrl", Order = 4)]
    public virtual Uri? IconUrl { get; set; }

    /// <summary>
    /// Gets or sets the agent's service provider, if any.
    /// </summary>
    [Description("The agent's service provider, if any.")]
    [DataMember(Name = "provider", Order = 5), JsonPropertyName("provider"), JsonPropertyOrder(5), YamlMember(Alias = "provider", Order = 5)]
    public virtual AgentProvider? Provider { get; set; }

    /// <summary>
    /// Gets or sets the agent's version.
    /// </summary>
    [Description("The agent's version.")]
    [Required, MinLength(1)]
    [DataMember(Name = "version", Order = 6), JsonPropertyName("version"), JsonPropertyOrder(6), YamlMember(Alias = "version", Order = 6)]
    public virtual string Version { get; set; } = null!;

    /// <summary>
    /// Gets or sets the version of the A2A protocol this agent supports.
    /// </summary>
    [Description("The version of the A2A protocol this agent supports")]
    [DataMember(Name = "protocolVersion", Order = 6), JsonPropertyName("protocolVersion"), JsonPropertyOrder(6), YamlMember(Alias = "protocolVersion", Order = 7)]
    [Required]
    public virtual string ProtocolVersion { get; set; } = "0.2.6";

    /// <summary>
    /// Gets or sets the URL, if any, referencing the agent's documentation.
    /// </summary>
    [Description("The URL, if any, referencing the agent's documentation.")]
    [DataMember(Name = "documentationUrl", Order = 8), JsonPropertyName("documentationUrl"), JsonPropertyOrder(8), YamlMember(Alias = "documentationUrl", Order = 8)]
    public virtual Uri? DocumentationUrl { get; set; }

    /// <summary>
    /// Gets or sets the agent's capabilities.
    /// </summary>
    [Description("The agent's capabilities.")]
    [Required]
    [DataMember(Name = "capabilities", Order = 9), JsonPropertyName("capabilities"), JsonPropertyOrder(9), YamlMember(Alias = "capabilities", Order = 9)]
    public virtual AgentCapabilities Capabilities { get; set; } = null!;

    /// <summary>
    /// Gets or sets the security scheme details, if any, used for authenticating with the agent.
    /// </summary>
    [Description("The security scheme details, if any, used for authenticating with the agent.")]
    [DataMember(Name = "securitySchemes", Order = 10), JsonPropertyName("securitySchemes"), JsonPropertyOrder(10), YamlMember(Alias = "securitySchemes", Order = 10)]
    public virtual EquatableDictionary<string, SecurityScheme>? SecuritySchemes { get; set; }

    /// <summary>
    /// Gets or sets security requirements, if any, that the agent requires to be met in order to access its resources.
    /// </summary>
    [Description("Security requirements, if any, that the agent requires to be met in order to access its resources.")]
    [DataMember(Name = "security", Order = 11), JsonPropertyName("security"), JsonPropertyOrder(11), YamlMember(Alias = "security", Order = 11)]
    public virtual EquatableList<EquatableDictionary<string, List<string>>>? Security { get; set; }

    /// <summary>
    /// Gets or sets the set of supported mime types for input.
    /// </summary>
    [Description("The set of supported mime types for input.")]
    [Required, MinLength(1)]
    [DataMember(Name = "defaultInputModes", Order = 12), JsonPropertyName("defaultInputModes"), JsonPropertyOrder(12), YamlMember(Alias = "defaultInputModes", Order = 12)]
    public virtual EquatableList<string> DefaultInputModes { get; set; } = [MediaTypeNames.Text.Plain];

    /// <summary>
    /// Gets or sets the set of supported mime types for output.
    /// </summary>
    [Description("The set of supported mime types for output.")]
    [Required, MinLength(1)]
    [DataMember(Name = "defaultOutputModes", Order = 13), JsonPropertyName("defaultOutputModes"), JsonPropertyOrder(13), YamlMember(Alias = "defaultOutputModes", Order = 13)]
    public virtual EquatableList<string> DefaultOutputModes { get; set; } = [MediaTypeNames.Text.Plain];

    /// <summary>
    /// Gets or sets the set of the agent's skills.
    /// </summary>
    [Description("The set of the agent's skills.")]
    [Required, MinLength(1)]
    [DataMember(Name = "skills", Order = 14), JsonPropertyName("skills"), JsonPropertyOrder(14), YamlMember(Alias = "skills", Order = 14)]
    public virtual EquatableList<AgentSkill> Skills { get; set; } = null!;

    /// <summary>
    /// Gets or sets a value indicating whether the agent supports retrieving a detailed Agent Card via an authenticated endpoint.
    /// </summary>
    [Description("A value indicating whether the agent supports retrieving a detailed Agent Card via an authenticated endpoint.")]
    [DataMember(Name = "supportsAuthenticatedExtendedCard", Order = 15), JsonPropertyName("supportsAuthenticatedExtendedCard"), JsonPropertyOrder(15), YamlMember(Alias = "supportsAuthenticatedExtendedCard", Order = 15)]
    public virtual bool SupportsAuthenticatedExtendedCard { get; set; }

    /// <inheritdoc/>
    public override string ToString() => Name;

}
