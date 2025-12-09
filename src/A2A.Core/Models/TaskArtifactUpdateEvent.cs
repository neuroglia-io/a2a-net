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
/// Represents a task artifact update event.
/// </summary>
[Description("Represents a task artifact update event.")]
[DataContract]
public sealed record TaskArtifactUpdateEvent
    : TaskEvent
{

    /// <summary>
    /// Gets the unique identifier of the context associated with the task.
    /// </summary>
    [Description("The unique identifier of the context associated with the task.")]
    [Required, MinLength(1)]
    [DataMember(Order = 2, Name = "contextId"), JsonPropertyOrder(2), JsonPropertyName("contextId")]
    public required string ContextId { get; init; }

    /// <summary>
    /// Gets the artifact that was generated or updated.
    /// </summary>
    [Description("The artifact that was generated or updated.")]
    [Required]
    [DataMember(Order = 3, Name = "artifact"), JsonPropertyOrder(3), JsonPropertyName("artifact")]
    public required Artifact Artifact { get; init; }

    /// <summary>
    /// Gets a boolean indicating whether the artifact should be appended to a previously sent artifact with the same ID.
    /// </summary>
    [Description("A boolean indicating whether the artifact should be appended to a previously sent artifact with the same ID.")]
    [DataMember(Order = 4, Name = "append"), JsonPropertyOrder(4), JsonPropertyName("append")]
    public bool? Append { get; init; }

    /// <summary>
    /// Gets a boolean indicating whether this is the final chunk of the artifact.
    /// </summary>
    [Description("A boolean indicating whether this is the final chunk of the artifact.")]
    [DataMember(Order = 5, Name = "lastChunk"), JsonPropertyOrder(5), JsonPropertyName("lastChunk")]
    public bool? LastChunk { get; init; }

    /// <summary>
    /// Gets a key/value mapping, if any, containing additional metadata about the artifact update.
    /// </summary>
    [Description("A key/value mapping, if any, containing additional metadata about the artifact update.")]
    [DataMember(Order = 5, Name = "metadata"), JsonPropertyOrder(5), JsonPropertyName("metadata")]
    public JsonObject? Metadata { get; init; }

}
