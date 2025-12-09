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
/// Enumerates all supported error codes.
/// </summary>
public static class ErrorCode
{

    /// <summary>
    /// Indicates the specified task ID does not correspond to an existing or accessible task.
    /// </summary>
    public const int TaskNotFound = -32001;

    /// <summary>
    /// Indicates an attempt was made to cancel a task that is not in a cancelable state.
    /// </summary>
    public const int TaskNotCancelable = -32002;

    /// <summary>
    /// Indicates the client attempted to use push notification features but the agent does not support them.
    /// </summary>
    public const int PushNotificationNotSupported = -32003;

    /// <summary>
    /// Indicates the requested operation (or a specific aspect of it) is not supported by the agent.
    /// </summary>
    public const int UnsupportedOperation = -32004;

    /// <summary>
    /// Indicates a media type/content type in message parts (or implied for an artifact) is not supported.
    /// </summary>
    public const int ContentTypeNotSupported = -32005;

    /// <summary>
    /// Indicates the agent returned a response that does not conform to the specification for the current method.
    /// </summary>
    public const int InvalidAgentResponse = -32006;

    /// <summary>
    /// Indicates the agent declares support for an extended agent card but does not have one configured when required.
    /// </summary>
    public const int ExtendedAgentCardNotConfigured = -32007;

    /// <summary>
    /// Indicates the client requested use of an extension marked as required, but did not declare support for it.
    /// </summary>
    public const int ExtensionSupportRequired = -32008;

    /// <summary>
    /// Indicates the A2A protocol version specified in the request is not supported by the agent.
    /// </summary>
    public const int VersionNotSupported = -32009;

    /// <summary>
    /// Gets all defined error codes.
    /// </summary>
    public static readonly IEnumerable<int> All =
    [
        TaskNotFound,
        TaskNotCancelable,
        PushNotificationNotSupported,
        UnsupportedOperation,
        ContentTypeNotSupported,
        InvalidAgentResponse,
        ExtendedAgentCardNotConfigured,
        ExtensionSupportRequired,
        VersionNotSupported
    ];

}
