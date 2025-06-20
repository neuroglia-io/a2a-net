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
/// Represents the base class for all task-related RPC events.
/// </summary>
[Description("Represents the base class for all task-related RPC events.")]
[DataContract]
[JsonPolymorphic(TypeDiscriminatorPropertyName = "kind")]
[JsonDerivedType(typeof(TaskArtifactUpdateEvent), TaskEventKind.ArtifactUpdate)]
[JsonDerivedType(typeof(TaskStatusUpdateEvent), TaskEventKind.StatusUpdate)]
public abstract record TaskEvent
    : RpcEvent
{

    /// <summary>
    /// Gets or sets the unique identifier of the task to which the event pertains.
    /// </summary>
    [Description("The unique identifier of the task to which the event pertains.")]
    [Required, MinLength(1)]
    [DataMember(Name = "taskId", Order = 0), JsonPropertyName("taskId"), JsonPropertyOrder(0), YamlMember(Alias = "taskId", Order = 0)]
    public virtual string TaskId { get; set; } = null!;

    /// <summary>
    /// Gets or sets the unique identifier of the context the task is associated with.
    /// </summary>
    [Description("The unique identifier of the context the task is associated with.")]
    [Required, MinLength(1)]
    [DataMember(Name = "contextId", Order = 1), JsonPropertyName("contextId"), JsonPropertyOrder(1), YamlMember(Alias = "contextId", Order = 1)]
    public virtual string ContextId { get; set; } = null!;

}
