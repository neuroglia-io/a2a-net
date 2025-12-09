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
/// Represents a streaming response from an A2A service.
/// </summary>
[Description("Represents a streaming response from an A2A service.")]
[DataContract]
public sealed record StreamResponse
{

    /// <summary>
    /// Gets the task, if any, returned as part of the streaming response.
    /// </summary>
    [Description("The task, if any, returned as part of the streaming response.")]
    [DataMember(Order = 1, Name = "task"), JsonPropertyOrder(1), JsonPropertyName("task")]
    public Task? Task { get; init; }

    /// <summary>
    /// Gets the message, if any, returned as part of the streaming response.
    /// </summary>
    [Description("The message, if any, returned as part of the streaming response.")]
    [DataMember(Order = 2, Name = "message"), JsonPropertyOrder(2), JsonPropertyName("message")]
    public Message? Message { get; init; }

    /// <summary>
    /// Gets the task status update event, if any, returned as part of the streaming response.
    /// </summary>
    [Description("The task status update event, if any, returned as part of the streaming response.")]
    [DataMember(Order = 3, Name = "statusUpdate"), JsonPropertyOrder(3), JsonPropertyName("statusUpdate")]
    public TaskStatusUpdateEvent? StatusUpdate { get; init; }

    /// <summary>
    /// Gets the task artifact update event, if any, returned as part of the streaming response.
    /// </summary>
    [Description("The task artifact update event, if any, returned as part of the streaming response.")]
    [DataMember(Order = 4, Name = "artifactUpdate"), JsonPropertyOrder(4), JsonPropertyName("artifactUpdate")]
    public TaskArtifactUpdateEvent? ArtifactUpdate { get; init; }

    /// <summary>
    /// Gets the streaming response type.
    /// </summary>
    [IgnoreDataMember, JsonIgnore]
    public string Type => Task is not null ? StreamResponseType.Task : Message is not null ? StreamResponseType.Message : StatusUpdate is not null ? StreamResponseType.StatusUpdate : ArtifactUpdate is not null ? StreamResponseType.ArtifactUpdate : StreamResponseType.Unknown;

}