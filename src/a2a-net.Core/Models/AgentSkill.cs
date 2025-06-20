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
/// Represents an object used to describe an agent's skill.
/// </summary>
[Description("An object used to describe an agent's skill.")]
[DataContract]
public record AgentSkill
{

    /// <summary>
    /// Gets or sets the skill's unique identifier.
    /// </summary>
    [Description("The skill's unique identifier.")]
    [Required, MinLength(1)]
    [DataMember(Name = "id", Order = 1), JsonPropertyName("id"), JsonPropertyOrder(1), YamlMember(Alias = "id", Order = 1)]
    public virtual string Id { get; set; } = null!;

    /// <summary>
    /// Gets or sets the skill's human readable name.
    /// </summary>
    [Description("The skill's human readable name.")]
    [Required, MinLength(1)]
    [DataMember(Name = "name", Order = 2), JsonPropertyName("name"), JsonPropertyOrder(2), YamlMember(Alias = "name", Order = 2)]
    public virtual string Name { get; set; } = null!;

    /// <summary>
    /// Gets or sets a human-readable description of the skill, if any.<para></para>
    /// Used by the client or a human as a hint to understand what the skill does.
    /// </summary>
    [Description("A human-readable description of the skill, if any.")]
    [DataMember(Name = "description", Order = 3), JsonPropertyName("description"), JsonPropertyOrder(3), YamlMember(Alias = "description", Order = 3)]
    public virtual string? Description { get; set; }

    /// <summary>
    /// Gets or sets a set of tag words, if any, used to describe classes of capabilities for this specific skill.
    /// </summary>
    [Description("A set of tag words, if any, used to describe classes of capabilities for this specific skill.")]
    [DataMember(Name = "tags", Order = 4), JsonPropertyName("tags"), JsonPropertyOrder(4), YamlMember(Alias = "tags", Order = 4)]
    public virtual EquatableList<string>? Tags { get; set; }

    /// <summary>
    /// Gets or sets a set of example scenarios, if any, that the skill can perform.<para></para>
    /// Used by the client as a hint to understand how the skill can be used.
    /// </summary>
    [Description("A set of example scenarios, if any, that the skill can perform.")]
    [DataMember(Name = "examples", Order = 5), JsonPropertyName("examples"), JsonPropertyOrder(5), YamlMember(Alias = "examples", Order = 5)]
    public virtual EquatableList<string>? Examples { get; set; }

    /// <summary>
    /// Gets or sets the set of supported mime types for input.
    /// </summary>
    [Description("The set of supported mime types for input.")]
    [DataMember(Name = "inputModes", Order = 6), JsonPropertyName("inputModes"), JsonPropertyOrder(6), YamlMember(Alias = "inputModes", Order = 6)]
    public virtual EquatableList<string> InputModes { get; set; } = [MediaTypeNames.Text.Plain];

    /// <summary>
    /// Gets or sets the set of supported mime types for output.
    /// </summary>
    [Description("The set of supported mime types for output.")]
    [DataMember(Name = "outputModes", Order = 7), JsonPropertyName("outputModes"), JsonPropertyOrder(7), YamlMember(Alias = "outputModes", Order = 7)]
    public virtual EquatableList<string> OutputModes { get; set; } = [MediaTypeNames.Text.Plain];

    /// <inheritdoc/>
    public override string ToString() => Name;

}