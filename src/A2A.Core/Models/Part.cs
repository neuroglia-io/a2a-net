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

using A2A.Serialization.Json;

namespace A2A.Models;

/// <summary>
/// Represents a container for a section of communication content.
/// </summary>
[Description("Represents a container for a section of communication content.")]
[JsonConverter(typeof(JsonPartConverter))]
[DataContract]
[KnownType(typeof(DataPart)), KnownType(typeof(FilePart)), KnownType(typeof(TextPart))]
public abstract record Part
{

    /// <summary>
    /// Gets the part content's media type, if any.
    /// </summary>
    [Description("The part content's media type, if any.")]
    [DataMember(Order = 0, Name = "mediaType"), JsonPropertyOrder(0), JsonPropertyName("mediaType")]
    public string? MediaType { get; init; }

    /// <summary>
    /// Gets a key/value mapping, if any, containing metadata associated with the part.
    /// </summary>
    [Description("A key/value mapping, if any, containing metadata associated with the part.")]
    [DataMember(Order = 99, Name = "metadata"), JsonPropertyOrder(99), JsonPropertyName("metadata")]
    public IReadOnlyDictionary<string, JsonNode>? Metadata { get; init; }

}
