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
/// Represents a message
/// </summary>
[DataContract]
public record Message
{

    /// <summary>
    /// Gets or sets the message's role
    /// </summary>
    [Required, AllowedValues(MessageRole.Agent, MessageRole.User)]
    [DataMember(Name = "role", Order = 1), JsonPropertyName("role"), JsonPropertyOrder(1), YamlMember(Alias = "role", Order = 1)]
    public virtual string Role { get; set; } = null!;

    /// <summary>
    /// Gets or sets a list containing the message's parts, if any
    /// </summary>
    [DataMember(Name = "parts", Order = 2), JsonPropertyName("parts"), JsonPropertyOrder(2), YamlMember(Alias = "parts", Order = 2)]
    public virtual EquatableList<Part>? Parts { get; set; }

    /// <summary>
    /// Gets or sets a key/value mapping that contains the message's additional properties, if any
    /// </summary>
    [DataMember(Name = "metadata", Order = 3), JsonPropertyName("metadata"), JsonPropertyOrder(3), YamlMember(Alias = "metadata", Order = 3)]
    public virtual EquatableDictionary<string, object>? Metadata { get; set; }

}
