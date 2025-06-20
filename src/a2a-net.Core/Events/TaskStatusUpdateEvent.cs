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

namespace A2A.Events;

/// <summary>
/// Represents the event used to notify about a status update
/// </summary>
[DataContract]
public record TaskStatusUpdateEvent
    : TaskEvent
{

    /// <summary>
    /// Gets or sets the task's status
    /// </summary>
    [Required]
    [DataMember(Name = "status", Order = 1), JsonPropertyName("status"), JsonPropertyOrder(1), YamlMember(Alias = "status", Order = 1)]
    public virtual Models.TaskStatus Status { get; set; } = null!;

    /// <summary>
    /// Gets or sets a boolean indicating whether or not the event is the last of the stream it belongs to
    /// </summary>
    [DataMember(Name = "final", Order = 2), JsonPropertyName("final"), JsonPropertyOrder(2), YamlMember(Alias = "final", Order = 2)]
    public virtual bool Final { get; set; }

}
