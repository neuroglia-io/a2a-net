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
/// Represents a structured artifact
/// </summary>
[DataContract]
public record Artifact
{

    /// <summary>
    /// Gets or sets the artifact's name, if any
    /// </summary>
    [DataMember(Name = "name", Order = 1), JsonPropertyName("name"), JsonPropertyOrder(1), YamlMember(Alias = "name", Order = 1)]
    public virtual string? Name { get; set; }

    /// <summary>
    /// Gets or sets the artifact's description, if any
    /// </summary>
    [DataMember(Name = "description", Order = 2), JsonPropertyName("description"), JsonPropertyOrder(2), YamlMember(Alias = "description", Order = 2)]
    public virtual string? Description { get; set; }

    /// <summary>
    /// Gets or sets a list containing the artifact's parts, if any
    /// </summary>
    [DataMember(Name = "parts", Order = 3), JsonPropertyName("parts"), JsonPropertyOrder(3), YamlMember(Alias = "parts", Order = 3)]
    public virtual EquatableList<Part>? Parts { get; set; }

    /// <summary>
    /// Gets or sets the artifact's index
    /// </summary>
    [DataMember(Name = "index", Order = 4), JsonPropertyName("index"), JsonPropertyOrder(4), YamlMember(Alias = "index", Order = 4)]
    public virtual uint Index { get; set; }

    /// <summary>
    /// Gets or sets a boolean indicating whether or not the artifact should be appended to the existing content
    /// </summary>
    [DataMember(Name = "append", Order = 5), JsonPropertyName("append"), JsonPropertyOrder(5), YamlMember(Alias = "append", Order = 5)]
    public virtual bool? Append { get; set; }

    /// <summary>
    /// Gets or sets a boolean indicating whether or not the artifact is the last chunk in a sequence
    /// </summary>
    [DataMember(Name = "lastChunk", Order = 6), JsonPropertyName("lastChunk"), JsonPropertyOrder(6), YamlMember(Alias = "lastChunk", Order = 6)]
    public virtual bool? LastChunk { get; set; }

    /// <summary>
    /// Gets or sets a key/value mapping that contains the message's additional properties, if any
    /// </summary>
    [DataMember(Name = "metadata", Order = 7), JsonPropertyName("metadata"), JsonPropertyOrder(7), YamlMember(Alias = "metadata", Order = 7)]
    public virtual EquatableDictionary<string, object>? Metadata { get; set; }

}
