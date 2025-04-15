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
/// Represents a text part
/// </summary>
[DataContract]
public record TextPart
    : Part
{

    /// <summary>
    /// Initializes a new <see cref="TextPart"/>
    /// </summary>
    public TextPart() { }

    /// <summary>
    /// Initializes a new <see cref="TextPart"/>
    /// </summary>
    /// <param name="text">The part's text</param>
    public TextPart(string text) 
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(text);
        Text = text;
    }

    /// <inheritdoc/>
    [IgnoreDataMember, JsonIgnore, YamlIgnore]
    public override string Type => PartType.Text;

    /// <summary>
    /// Gets/sets the part's text
    /// </summary>
    [Required]
    [DataMember(Name = "text", Order = 1), JsonPropertyName("text"), JsonPropertyOrder(1), YamlMember(Alias = "text", Order = 1)]
    public virtual string Text { get; set; } = null!;

}
