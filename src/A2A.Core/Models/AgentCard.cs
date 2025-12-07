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
/// Represents an agent's manifest.
/// </summary>
[Description("Represents an agent's manifest.")]
[DataContract]
public sealed record AgentCard
{

    /// <summary>
    /// Gets the A2A protocol version the agent complies with, if any.
    /// </summary>
    [Description("The A2A protocol version the agent complies with, if any.")]
    [DataMember(Order = 1, Name = "protocolVersion"), JsonPropertyOrder(1), JsonPropertyName("protocolVersion")]
    public string? ProtocolVersion { get; set; } = "1.0";

    /// <summary>
    /// Gets the agent's human readable name.
    /// </summary>
    [Description("The agent's human readable name.")]
    [Required, MinLength(1)]
    [DataMember(Order = 2, Name = "name"), JsonPropertyOrder(2), JsonPropertyName("name")]
    public string Name { get; set; } = null!;

    /// <summary>
    /// Gets the agent's human readable description.
    /// </summary>
    [Description("The agent's human readable description.")]
    [Required, MinLength(1)]
    [DataMember(Order = 3, Name = "description"), JsonPropertyOrder(3), JsonPropertyName("description")]
    public string Description { get; set; } = null!;

    /// <summary>
    /// Gets the agent's version.
    /// </summary>
    [Description("The agent's version.")]
    [Required, MinLength(1)]
    [DataMember(Order = 4, Name = "version"), JsonPropertyOrder(4), JsonPropertyName("version")]
    public string Version { get; set; } = null!;

    /// <summary>
    /// Gets the agent's provider information, if any.
    /// </summary>
    [Description("The agent's provider information, if any.")]
    [DataMember(Order = 5, Name = "provider"), JsonPropertyOrder(5), JsonPropertyName("provider")]
    public AgentProvider? Provider { get; set; }

    /// <summary>
    /// Gets the agent's icon URL, if any.
    /// </summary>
    [Description("The agent's icon URL, if any.")]
    [DataMember(Order = 6, Name = "iconUrl"), JsonPropertyOrder(6), JsonPropertyName("iconUrl")]
    public Uri? IconUrl { get; set; }

    /// <summary>
    /// Gets the agent's documentation URL, if any.
    /// </summary>
    [Description("The agent's documentation URL, if any.")]
    [DataMember(Order = 7, Name = "documentationUrl"), JsonPropertyOrder(7), JsonPropertyName("documentationUrl")]
    public Uri? DocumentationUrl { get; set; }

    /// <summary>
    /// Gets an ordered collection containing the interfaces the agent exposes, if any.
    /// </summary>
    [Description("An ordered collection containing the interfaces the agent exposes, if any.")]
    [DataMember(Order = 8, Name = "supportedInterfaces"), JsonPropertyOrder(8), JsonPropertyName("supportedInterfaces")]
    public IReadOnlyCollection<AgentInterface>? Interfaces { get; set; }

    /// <summary>
    /// Gets the agent's URL, if any.
    /// </summary>
    [Description("The agent's URL, if any.")]
    [Obsolete("This property is deprecated and will be removed in future versions. Use 'supportedInterfaces' instead.")]
    [DataMember(Order = 9, Name = "url"), JsonPropertyOrder(9), JsonPropertyName("url")]
    public string? Url { get; set; } = string.Empty;

    /// <summary>
    /// Gets the agent's preferred transport mechanism, if any.
    /// </summary>
    [Description("The agent's preferred transport mechanism, if any.")]
    [Obsolete("This property is deprecated and will be removed in future versions. Use 'supportedInterfaces' instead.")]
    [DataMember(Order = 10, Name = "preferredTransport"), JsonPropertyOrder(10), JsonPropertyName("preferredTransport")]
    public string? PreferredTransport { get; set; }

    /// <summary>
    /// Gets the agent's additional interfaces, if any.
    /// </summary>
    [Description("The agent's additional interfaces, if any.")]
    [Obsolete("This property is deprecated and will be removed in future versions. Use 'supportedInterfaces' instead.")]
    [DataMember(Order = 11, Name = "additionalInterfaces"), JsonPropertyOrder(11), JsonPropertyName("additionalInterfaces")]
    public ICollection<AgentInterface>? AdditionalInterfaces { get; set; }

    /// <summary>
    /// Gets an object, if any, used to describe the agent's capabilities.
    /// </summary>
    [Description("An object, if any, used to describe the agent's capabilities.")]
    [DataMember(Order = 12, Name = "capabilities"), JsonPropertyOrder(12), JsonPropertyName("capabilities")]
    public AgentCapabilities? Capabilities { get; set; }

    /// <summary>
    /// Gets a collection containing the agent's skills.
    /// </summary>
    [Description("A collection containing the agent's skills.")]
    [Required, MinLength(1)]
    [DataMember(Order = 13, Name = "skills"), JsonPropertyOrder(13), JsonPropertyName("skills")]
    public ICollection<AgentSkill> Skills { get; set; } = null!;

    /// <summary>
    /// Gets a collection containing the agent's default input modes, if any.
    /// </summary>
    [Description("A collection containing the agent's default input modes, if any.")]
    [Required, MinLength(1)]
    [DataMember(Order = 14, Name = "defaultInputModes"), JsonPropertyOrder(14), JsonPropertyName("defaultInputModes")]
    public ICollection<string> DefaultInputModes { get; set; } = ["text"];

    /// <summary>
    /// Gets a collection containing the agent's default output modes, if any.
    /// </summary>
    [Description("A collection containing the agent's default output modes, if any.")]
    [Required, MinLength(1)]
    [DataMember(Order = 15, Name = "defaultOutputModes"), JsonPropertyOrder(15), JsonPropertyName("defaultOutputModes")]
    public ICollection<string> DefaultOutputModes { get; set; } = ["text"];

    /// <summary>
    /// Gets a key/value mapping, if any, containing the security schemes supported by the agent.
    /// </summary>
    [Description("A key/value mapping, if any, containing the security schemes supported by the agent.")]
    [DataMember(Order = 16, Name = "securitySchemes"), JsonPropertyOrder(16), JsonPropertyName("securitySchemes")]
    public IDictionary<string, SecurityScheme>? SecuritySchemes { get; set; }

    /// <summary>
    /// Gets a collection containing the security requirements, if any, needed to interact with the agent.
    /// </summary>
    [Description("A collection containing the security requirements, if any, needed to interact with the agent.")]
    [DataMember(Order = 17, Name = "security"), JsonPropertyOrder(17), JsonPropertyName("security")]
    public ICollection<IDictionary<string, string[]>>? Security { get; set; }

    /// <summary>
    /// Gets a collection containing the agent's signatures, if any.
    /// </summary>
    [Description("A collection containing the agent's signatures, if any.")]
    [DataMember(Order = 18, Name = "signatures"), JsonPropertyOrder(18), JsonPropertyName("signatures")]
    public ICollection<AgentCardSignature>? Signatures { get; set; }

    /// <summary>
    /// Gets a boolean indicating whether the agent supports authenticated extended card.
    /// </summary>
    [Description("A boolean indicating whether the agent supports authenticated extended card.")]
    [DataMember(Order = 19, Name = "supportsAuthenticatedExtendedCard"), JsonPropertyOrder(19), JsonPropertyName("supportsAuthenticatedExtendedCard")]
    public bool? SupportsAuthenticatedExtendedCard { get; set; }

}
