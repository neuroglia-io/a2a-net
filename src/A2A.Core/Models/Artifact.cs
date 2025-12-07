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
/// Represents an artifact produced as output of a task.
/// </summary>
[Description("Represents an artifact produced as output of a task.")]
[DataContract]
public sealed record Artifact
{

    /// <summary>
    /// Gets the artifact's unique identifier.
    /// </summary>
    [Description("The artifact's unique identifier.")]
    [Required, MinLength(1)]
    [DataMember(Order = 1, Name = "artifactId"), JsonPropertyOrder(1), JsonPropertyName("artifactId")]
    public string ArtifactId { get; init; } = Guid.NewGuid().ToString("N");

    /// <summary>
    /// Gets the artifact's name, if any.
    /// </summary>
    [Description("The artifact's name, if any.")]
    [DataMember(Order = 2, Name = "name"), JsonPropertyOrder(2), JsonPropertyName("name")]
    public string? Name { get; init; }

    /// <summary>
    /// Gets the artifact's description, if any.
    /// </summary>
    [Description("The artifact's description, if any.")]
    [DataMember(Order = 3, Name = "description"), JsonPropertyOrder(3), JsonPropertyName("description")]
    public string? Description { get; init; }

    /// <summary>
    /// Gets a collection containing the parts the artifact is composed of.
    /// </summary>
    [Description("A collection containing the parts the artifact is composed of.")]
    [Required, MinLength(1)]
    [DataMember(Order = 4, Name = "parts"), JsonPropertyOrder(4), JsonPropertyName("parts")]
    public required IReadOnlyCollection<Part> Parts { get; set; }

    /// <summary>
    /// Gets a key/value mapping, if any, containing metadata associated with the artifact.
    /// </summary>
    [Description("A key/value mapping, if any, containing metadata associated with the artifact.")]
    [DataMember(Order = 5, Name = "metadata"), JsonPropertyOrder(5), JsonPropertyName("metadata")]
    public JsonObject? Metadata { get; set; }

    /// <summary>
    /// Gets a collection containing the URIs of the extensions, if any, used to generate the artifact.
    /// </summary>
    [Description("A collection containing the URIs of the extensions, if any, used to generate the artifact.")]
    [DataMember(Order = 6, Name = "extensions"), JsonPropertyOrder(6), JsonPropertyName("extensions")]
    public IReadOnlyCollection<Uri>? Extensions { get; init; }

}
