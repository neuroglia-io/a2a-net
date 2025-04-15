// Copyright � 2025-Present Neuroglia SRL
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

namespace Neuroglia.A2A.Models;

/// <summary>
/// Represents a fully formed piece of content exchanged between a client and a remote agent as part of a message or an artifact
/// </summary>
[DataContract]
[JsonPolymorphic(TypeDiscriminatorPropertyName = "type")]
[JsonDerivedType(typeof(DataPart), PartType.Data)]
[JsonDerivedType(typeof(FilePart), PartType.File)]
[JsonDerivedType(typeof(TextPart), PartType.Text)]
public abstract record Part
{

    /// <summary>
    /// Gets the part's type
    /// </summary>
    public abstract string Type { get; }

    /// <summary>
    /// Gets/sets a key/value mapping that contains the message's additional properties, if any
    /// </summary>
    [DataMember(Name = "metadata", Order = 99), JsonPropertyName("metadata"), JsonPropertyOrder(99), YamlMember(Alias = "metadata", Order = 99)]
    public virtual EquatableDictionary<string, object>? Metadata { get; set; }

}
