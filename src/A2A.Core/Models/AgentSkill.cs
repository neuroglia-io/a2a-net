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
/// Represents a distinct capability or function that an agent can perform.
/// </summary>
[Description("Represents a distinct capability or function that an agent can perform.")]
[DataContract]
public sealed record AgentSkill
{

    /// <summary>
    /// Gets the skill's unique identifier.
    /// </summary>
    [Description("The skill's unique identifier.")]
    [Required, MinLength(1)]
    [DataMember(Order = 1, Name = "id"), JsonPropertyOrder(1), JsonPropertyName("id")]
    public string Id { get; set; } = Guid.NewGuid().ToString("N");

    /// <summary>
    /// Gets the skill's human readable name.
    /// </summary>
    [Description("The skill's human readable name.")]
    [Required, MinLength(1)]
    [DataMember(Order = 2, Name = "name"), JsonPropertyOrder(2), JsonPropertyName("name")]
    public string Name { get; set; } = null!;

    /// <summary>
    /// Gets the skill's human readable description.
    /// </summary>
    [Description("The skill's human readable description.")]
    [Required, MinLength(1)]
    [DataMember(Order = 3, Name = "description"), JsonPropertyOrder(3), JsonPropertyName("description")]
    public string Description { get; set; } = null!;

    /// <summary>
    /// Gets a collection containing the tags associated with the skill.
    /// </summary>
    [Description("A collection containing the tags associated with the skill.")]
    [Required, MinLength(1)]
    [DataMember(Order = 4, Name = "tags"), JsonPropertyOrder(4), JsonPropertyName("tags")]
    public ICollection<string> Tags { get; set; } = null!;

    /// <summary>
    /// Gets a collection containing example prompts or scenarios that this skill can handle.
    /// </summary>
    [Description("A collection containing example prompts or scenarios that this skill can handle.")]
    [DataMember(Order = 5, Name = "examples"), JsonPropertyOrder(5), JsonPropertyName("examples")]
    public ICollection<string>? Examples { get; set; }

    /// <summary>
    /// Gets a collection containing the input modes supported by the skill.
    /// </summary>
    [Description("A collection containing the input modes supported by the skill.")]
    [DataMember(Order = 6, Name = "inputModes"), JsonPropertyOrder(6), JsonPropertyName("inputModes")]
    public ICollection<string>? InputModes { get; set; }

    /// <summary>
    /// Gets a collection containing the output modes supported by the skill.
    /// </summary>
    [Description("A collection containing the output modes supported by the skill.")]
    [DataMember(Order = 7, Name = "outputModes"), JsonPropertyOrder(7), JsonPropertyName("outputModes")]
    public ICollection<string>? OutputModes { get; set; }

    /// <summary>
    /// Gets a collection containing the security requirements, if any, needed to utilize the skill.
    /// </summary>
    [Description("A collection containing the security requirements, if any, needed to utilize the skill.")]
    [DataMember(Order = 8, Name = "security"), JsonPropertyOrder(8), JsonPropertyName("security")]
    public ICollection<IDictionary<string, string[]>>? Security { get; set; }

}
