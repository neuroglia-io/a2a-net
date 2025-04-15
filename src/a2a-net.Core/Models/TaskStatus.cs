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
/// Represents an object used to describe the status of a task
/// </summary>
[DataContract]
public record TaskStatus
{

    /// <summary>
    /// Gets/sets the task's state
    /// </summary>
    [Required, AllowedValues(TaskState.Submitted, TaskState.Working, TaskState.InputRequired, TaskState.Completed, TaskState.Cancelled, TaskState.Failed, TaskState.Unknown)]
    [DataMember(Name = "state", Order = 1), JsonPropertyName("state"), JsonPropertyOrder(1), YamlMember(Alias = "state", Order = 1)]
    public virtual string State { get; set; } = null!;

    /// <summary>
    /// Gets/sets additional status updates, if any, for the client
    /// </summary>
    [DataMember(Name = "message", Order = 2), JsonPropertyName("message"), JsonPropertyOrder(2), YamlMember(Alias = "message", Order = 2)]
    public virtual Message? Message { get; set; }

    /// <summary>
    /// Gets/sets the task's timestamp
    /// </summary>
    [DataMember(Name = "timestamp", Order = 3), JsonPropertyName("timestamp"), JsonPropertyOrder(3), YamlMember(Alias = "timestamp", Order = 3)]
    public virtual DateTimeOffset Timestamp { get; set; }

}
