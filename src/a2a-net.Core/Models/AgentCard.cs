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

namespace A2A.Models;

/// <summary>
/// Represents an object used to describe an agent’s capabilities/skills and authentication mechanism
/// </summary>
[DataContract]
public record AgentCard
{

    /// <summary>
    /// Gets/sets the agent's human readable name
    /// </summary>
    [Required, MinLength(1)]
    [DataMember(Name = "name", Order = 1), JsonPropertyName("name"), JsonPropertyOrder(1), YamlMember(Alias = "name", Order = 1)]
    public virtual string Name { get; set; } = null!;

    /// <summary>
    /// Gets/sets a human-readable description of the agent, if any<para></para>
    /// Used to assist users and other agents in understanding what the agent can do
    /// </summary>
    [DataMember(Name = "description", Order = 2), JsonPropertyName("description"), JsonPropertyOrder(2), YamlMember(Alias = "description", Order = 2)]
    public virtual string? Description { get; set; }

    /// <summary>
    /// Gets/sets the URL referencing the address the agent is hosted at
    /// </summary>
    [Required]
    [DataMember(Name = "url", Order = 3), JsonPropertyName("url"), JsonPropertyOrder(3), YamlMember(Alias = "url", Order = 3)]
    public virtual Uri Url { get; set; } = null!;

    /// <summary>
    /// Gets/sets the agent's service provider, if any
    /// </summary>
    [DataMember(Name = "provider", Order = 4), JsonPropertyName("provider"), JsonPropertyOrder(4), YamlMember(Alias = "provider", Order = 4)]
    public virtual AgentProvider? Provider { get; set; }

    /// <summary>
    /// Gets/sets the agent's version
    /// </summary>
    [Required, MinLength(1)]
    [DataMember(Name = "version", Order = 5), JsonPropertyName("version"), JsonPropertyOrder(5), YamlMember(Alias = "version", Order = 5)]
    public virtual string Version { get; set; } = null!;

    /// <summary>
    /// Gets/sets the URL, if any, referencing the agent's documentation
    /// </summary>
    [DataMember(Name = "documentationUrl", Order = 6), JsonPropertyName("documentationUrl"), JsonPropertyOrder(6), YamlMember(Alias = "documentationUrl", Order = 6)]
    public virtual Uri? DocumentationUrl { get; set; }

    /// <summary>
    /// Gets/sets the agent's capabilities
    /// </summary>
    [Required]
    [DataMember(Name = "capabilities", Order = 7), JsonPropertyName("capabilities"), JsonPropertyOrder(7), YamlMember(Alias = "capabilities", Order = 7)]
    public virtual AgentCapabilities Capabilities { get; set; } = null!;

    /// <summary>
    /// Gets/sets the authentication  requirements for the agent
    /// </summary>
    [DataMember(Name = "authentication", Order = 8), JsonPropertyName("authentication"), JsonPropertyOrder(8), YamlMember(Alias = "authentication", Order = 8)]
    public virtual AgentAuthentication? Authentication { get; set; }

    /// <summary>
    /// Gets/sets the set of supported mime types for input
    /// </summary>
    [Required, MinLength(1)]
    [DataMember(Name = "defaultInputModes", Order = 9), JsonPropertyName("defaultInputModes"), JsonPropertyOrder(9), YamlMember(Alias = "defaultInputModes", Order = 9)]
    public virtual EquatableList<string> DefaultInputModes { get; set; } = [MediaTypeNames.Text.Plain];

    /// <summary>
    /// Gets/sets the set of supported mime types for output
    /// </summary>
    [Required, MinLength(1)]
    [DataMember(Name = "defaultOutputModes", Order = 10), JsonPropertyName("defaultOutputModes"), JsonPropertyOrder(10), YamlMember(Alias = "defaultOutputModes", Order = 10)]
    public virtual EquatableList<string> DefaultOutputModes { get; set; } = [MediaTypeNames.Text.Plain];

    /// <summary>
    /// Gets/sets the set of the agent's skills
    /// </summary>
    [Required, MinLength(1)]
    [DataMember(Name = "skils", Order = 11), JsonPropertyName("skils"), JsonPropertyOrder(11), YamlMember(Alias = "skils", Order = 11)]
    public virtual EquatableList<AgentSkill> Skills { get; set; } = null!;

    /// <inheritdoc/>
    public override string ToString() => Name;

}
