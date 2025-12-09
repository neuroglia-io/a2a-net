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

namespace A2A;

/// <summary>
/// Enumerates the possible types of streaming responses.
/// </summary>
public static class StreamResponseType
{

    /// <summary>
    /// Indicates that the streaming response contains a task artifact update event.
    /// </summary>
    public const string ArtifactUpdate = "artifact-update";
    /// <summary>
    /// Indicates that the streaming response contains a message.
    /// </summary>
    public const string Message = "message";
    /// <summary>
    /// Indicates that the streaming response contains a task status update event.
    /// </summary>
    public const string StatusUpdate = "status-update";
    /// <summary>
    /// Indicates that the streaming response contains the current state of a task.
    /// </summary>
    public const string Task = "task";
    /// <summary>
    /// Indicates that the streaming response type is unknown.
    /// </summary>
    public const string Unknown = "unknown";

    /// <summary>
    /// Gets all possible types of streaming responses.
    /// </summary>
    public static readonly IEnumerable<string> All =
    [
        ArtifactUpdate,
        Message,
        StatusUpdate,
        Task,
        Unknown
    ];

}