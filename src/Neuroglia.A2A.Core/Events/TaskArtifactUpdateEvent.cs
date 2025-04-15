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

namespace Neuroglia.A2A.Events;

/// <summary>
/// Represents the event used to notify about an artifact update
/// </summary>
[DataContract]
public record TaskArtifactUpdateEvent
    : TaskEvent
{

    /// <summary>
    /// Gets/sets the updated artifact
    /// </summary>
    [Required]
    [DataMember(Name = "artifact", Order = 1), JsonPropertyName("artifact"), JsonPropertyOrder(1), YamlMember(Alias = "artifact", Order = 1)]
    public virtual Artifact Artifact { get; set; } = null!;

}