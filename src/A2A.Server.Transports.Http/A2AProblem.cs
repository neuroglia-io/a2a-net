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

namespace A2A.Server.Transports;

/// <summary>
/// Exposes problem details for HTTP responses.
/// </summary>
public static class A2AProblem
{

    /// <summary>
    /// Creates a new <see cref="ProblemDetails"/> indicating that a task was not found.
    /// </summary>
    /// <param name="id">The unique identifier of the task that was not found.</param>
    /// <returns>A new <see cref="ProblemDetails"/> instance.</returns>
    public static ProblemDetails TaskNotFound(string id)
    {
        return new ProblemDetails
        {
            Title = "Task Not Found",
            Detail = $"Failed to find the task with ID '{id}'.",
            Status = StatusCodes.Status404NotFound,
            Type = "https://a2a-protocol.org/errors/task-not-found"
        };
    }

    /// <summary>
    /// Creates a new <see cref="ProblemDetails"/> indicating that a task cannot be canceled.
    /// </summary>
    /// <param name="id">The unique identifier of the task that cannot be canceled.</param>
    /// <param name="reason">An optional reason describing why the task cannot be canceled.</param>
    /// <returns>A new <see cref="ProblemDetails"/> instance.</returns>
    public static ProblemDetails TaskNotCancelable(string id, string? reason = null)
    {
        var detail = $"Failed to cancel the task with ID '{id}'.";
        if (!string.IsNullOrWhiteSpace(reason)) detail += $" {reason}";
        return new ProblemDetails
        {
            Title = "Task Not Cancelable",
            Detail = detail,
            Status = StatusCodes.Status409Conflict,
            Type = "https://a2a-protocol.org/errors/task-not-cancelable"
        };
    }

    /// <summary>
    /// Creates a new <see cref="ProblemDetails"/> indicating that push notifications are not supported.
    /// </summary>
    /// <param name="detail">An optional detail message.</param>
    /// <returns>A new <see cref="ProblemDetails"/> instance.</returns>
    public static ProblemDetails PushNotificationNotSupported(string? detail = null)
    {
        return new ProblemDetails
        {
            Title = "Push Notifications Not Supported",
            Detail = string.IsNullOrWhiteSpace(detail) ? "Push notifications are not supported by this server." : detail,
            Status = StatusCodes.Status400BadRequest,
            Type = "https://a2a-protocol.org/errors/push-notification-not-supported"
        };
    }

    /// <summary>
    /// Creates a new <see cref="ProblemDetails"/> indicating that the requested operation is unsupported.
    /// </summary>
    /// <param name="detail">An optional detail message.</param>
    /// <returns>A new <see cref="ProblemDetails"/> instance.</returns>
    public static ProblemDetails UnsupportedOperation(string? detail = null)
    {
        return new ProblemDetails
        {
            Title = "Unsupported Operation",
            Detail = string.IsNullOrWhiteSpace(detail) ? "The requested operation is not supported." : detail,
            Status = StatusCodes.Status400BadRequest,
            Type = "https://a2a-protocol.org/errors/unsupported-operation"
        };
    }

    /// <summary>
    /// Creates a new <see cref="ProblemDetails"/> indicating that the request content type is not supported.
    /// </summary>
    /// <param name="contentType">The request content type that is not supported.</param>
    /// <returns>A new <see cref="ProblemDetails"/> instance.</returns>
    public static ProblemDetails ContentTypeNotSupported(string? contentType = null)
    {
        return new ProblemDetails
        {
            Title = "Content Type Not Supported",
            Detail = string.IsNullOrWhiteSpace(contentType) ? "The request content type is not supported." : $"The request content type '{contentType}' is not supported.",
            Status = StatusCodes.Status415UnsupportedMediaType,
            Type = "https://a2a-protocol.org/errors/content-type-not-supported"
        };
    }

    /// <summary>
    /// Creates a new <see cref="ProblemDetails"/> indicating that the agent returned an invalid response.
    /// </summary>
    /// <param name="detail">An optional detail message.</param>
    /// <returns>A new <see cref="ProblemDetails"/> instance.</returns>
    public static ProblemDetails InvalidAgentResponse(string? detail = null)
    {
        return new ProblemDetails
        {
            Title = "Invalid Agent Response",
            Detail = string.IsNullOrWhiteSpace(detail) ? "The agent returned an invalid response." : detail,
            Status = StatusCodes.Status502BadGateway,
            Type = "https://a2a-protocol.org/errors/invalid-agent-response"
        };
    }

    /// <summary>
    /// Creates a new <see cref="ProblemDetails"/> indicating that an extended agent card is not configured.
    /// </summary>
    /// <param name="detail">An optional detail message.</param>
    /// <returns>A new <see cref="ProblemDetails"/> instance.</returns>
    public static ProblemDetails ExtendedAgentCardNotConfigured(string? detail = null)
    {
        return new ProblemDetails
        {
            Title = "Extended Agent Card Not Configured",
            Detail = string.IsNullOrWhiteSpace(detail) ? "The extended agent card is not configured for this server." : detail,
            Status = StatusCodes.Status400BadRequest,
            Type = "https://a2a-protocol.org/errors/extended-agent-card-not-configured"
        };
    }

    /// <summary>
    /// Creates a new <see cref="ProblemDetails"/> indicating that extension support is required.
    /// </summary>
    /// <param name="extension">The extension identifier or name.</param>
    /// <param name="detail">An optional detail message.</param>
    /// <returns>A new <see cref="ProblemDetails"/> instance.</returns>
    public static ProblemDetails ExtensionSupportRequired(string? extension = null, string? detail = null)
    {
        if (string.IsNullOrWhiteSpace(detail)) detail = string.IsNullOrWhiteSpace(extension) ? "Support for one or more required extensions is missing." : $"Support for the required extension '{extension}' is missing.";
        return new ProblemDetails
        {
            Title = "Extension Support Required",
            Detail = detail,
            Status = StatusCodes.Status400BadRequest,
            Type = "https://a2a-protocol.org/errors/extension-support-required"
        };
    }

    /// <summary>
    /// Creates a new <see cref="ProblemDetails"/> indicating that the requested protocol version is not supported.
    /// </summary>
    /// <param name="version">The requested protocol version.</param>
    /// <param name="detail">An optional detail message.</param>
    /// <returns>A new <see cref="ProblemDetails"/> instance.</returns>
    public static ProblemDetails VersionNotSupported(string? version = null, string? detail = null)
    {
        if (string.IsNullOrWhiteSpace(detail)) detail = string.IsNullOrWhiteSpace(version) ? "The requested protocol version is not supported." : $"The requested protocol version '{version}' is not supported.";
        return new ProblemDetails
        {
            Title = "Version Not Supported",
            Detail = detail,
            Status = StatusCodes.Status400BadRequest,
            Type = "https://a2a-protocol.org/errors/version-not-supported"
        };
    }

    /// <summary>
    /// Maps an error code to a corresponding <see cref="ProblemDetails"/> instance.
    /// </summary>
    /// <param name="errorCode">The error code to map.</param>
    /// <returns>A new <see cref="ProblemDetails"/>.</returns>
    public static ProblemDetails Map(int errorCode)
    {
        return errorCode switch
        {
            -32001 => TaskNotFound("unknown"),
            -32002 => TaskNotCancelable("unknown"),
            -32003 => PushNotificationNotSupported(),
            -32004 => UnsupportedOperation(),
            -32005 => ContentTypeNotSupported(),
            -32006 => InvalidAgentResponse(),
            -32007 => ExtendedAgentCardNotConfigured(),
            -32008 => ExtensionSupportRequired(),
            -32009 => VersionNotSupported(),
            _ => new ProblemDetails
            {
                Title = "Unknown Error",
                Detail = "An unknown error has occurred.",
                Status = StatusCodes.Status500InternalServerError,
                Type = "https://a2a-protocol.org/errors/unknown-error"
            },
        };
    }

}