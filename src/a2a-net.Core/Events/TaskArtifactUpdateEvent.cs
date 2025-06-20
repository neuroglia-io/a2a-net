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

namespace A2A.Events;

/// <summary>
/// Represents an event used to notify about an artifact update.
/// </summary>
[Description("Represents an event used to notify about an artifact update.")]
[DataContract]
public record TaskArtifactUpdateEvent
    : TaskEvent
{

    /// <summary>
    /// Gets or sets the updated artifact.
    /// </summary>
    [Description("The updated artifact.")]
    [Required]
    [DataMember(Name = "artifact", Order = 2), JsonPropertyName("artifact"), JsonPropertyOrder(2), YamlMember(Alias = "artifact", Order = 2)]
    public virtual Artifact Artifact { get; set; } = null!;

    /// <summary>
    /// Gets or sets a value indicating whether the artifact update should be appended to the existing artifact.
    /// </summary>
    [Description("Indicates whether the artifact update should be appended to the existing artifact.")]
    [DataMember(Name = "append", Order = 3), JsonPropertyName("append"), JsonPropertyOrder(3), YamlMember(Alias = "append", Order = 3)]
    public virtual bool Append { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether this is the last chunk of the artifact update.
    /// </summary>
    [Description("Indicates whether this is the last chunk of the artifact update.")]
    [DataMember(Name = "lastChunk", Order = 4), JsonPropertyName("lastChunk"), JsonPropertyOrder(4), YamlMember(Alias = "lastChunk", Order = 4)]
    public virtual bool LastChunk { get; set; }

}

