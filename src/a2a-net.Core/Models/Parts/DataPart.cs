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

namespace A2A.Models.Parts;

/// <summary>
/// Represents a structured data segment within a message part.
/// </summary>
[Description("A structured data segment within a message part.")]
[DataContract]
public record DataPart
    : Part
{

    /// <inheritdoc/>
    [IgnoreDataMember, JsonIgnore, YamlIgnore]
    public override string Kind => PartType.Data;

    /// <summary>
    /// Gets or sets the part's data.
    /// </summary>
    [Description("The part's data.")]
    [Required]
    [DataMember(Name = "data", Order = 1), JsonPropertyName("data"), JsonPropertyOrder(1), YamlMember(Alias = "data", Order = 1)]
    public virtual EquatableDictionary<string, object> Data { get; set; } = null!;

}
