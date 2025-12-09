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
/// Represents a task-related event.
/// </summary>
[Description("Represents a task-related event.")]
[DataContract]
[KnownType(typeof(TaskArtifactUpdateEvent)), KnownType(typeof(TaskStatusUpdateEvent))]
public abstract record TaskEvent
{

    /// <summary>
    /// Gets the unique identifier of the task the event is related to.
    /// </summary>
    [Description("The unique identifier of the task the event is related to.")]
    [Required, MinLength(1)]
    [DataMember(Order = 1, Name = "taskId"), JsonPropertyOrder(1), JsonPropertyName("taskId")]
    public required string TaskId { get; init; }

}