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
/// Represents the HTTP implementation of the <see cref="IA2ATransport"/> interface.
/// </summary>
/// <param name="server">The A2A server.</param>
public sealed class A2AHttpTransport(IA2AServer server)
    : IA2ATransport
{

    /// <inheritdoc/>
    public string ProtocolBinding => A2A.ProtocolBinding.Http;

    /// <inheritdoc/>
    public async Task<IResult> HandleAsync(HttpContext httpContext)
    {
        var path = httpContext.Request.Path.Value?.TrimEnd('/').ToLowerInvariant();
        return path switch
        {
            "/v1/message:send" => await SendMessageAsync(httpContext).ConfigureAwait(false),
            "/v1/message:stream" => await SendStreamingMessageAsync(httpContext).ConfigureAwait(false),
            var templatedPath when templatedPath?.StartsWith("/v1/tasks/", StringComparison.OrdinalIgnoreCase) == true => await GetTaskAsync(httpContext).ConfigureAwait(false),
            var templatedPath when templatedPath?.StartsWith("/v1/tasks", StringComparison.OrdinalIgnoreCase) == true => await ListTasksAsync(httpContext).ConfigureAwait(false),
            var templatedPath when templatedPath?.StartsWith("/v1/tasks/", StringComparison.OrdinalIgnoreCase) == true && templatedPath.EndsWith(":cancel", StringComparison.OrdinalIgnoreCase) => await CancelTaskAsync(httpContext).ConfigureAwait(false),
            var templatedPath when templatedPath?.StartsWith("/v1/tasks/", StringComparison.OrdinalIgnoreCase) == true && templatedPath.EndsWith(":subscribe", StringComparison.OrdinalIgnoreCase) => await SubscribeToTaskAsync(httpContext).ConfigureAwait(false),
            var templatedPath when templatedPath?.StartsWith("/v1/tasks/", StringComparison.OrdinalIgnoreCase) == true && templatedPath.EndsWith("/pushnotificationconfigs", StringComparison.OrdinalIgnoreCase) && httpContext.Request.Method == HttpMethods.Post => await SetOrUpdatePushNotificationConfigAsync(httpContext).ConfigureAwait(false),
            var templatedPath when templatedPath?.StartsWith("/v1/tasks/", StringComparison.OrdinalIgnoreCase) == true && templatedPath.Contains("/pushnotificationconfigs/") && httpContext.Request.Method == HttpMethods.Get => await GetPushNotificationConfigAsync(httpContext).ConfigureAwait(false),
            var templatedPath when templatedPath?.StartsWith("/v1/tasks/", StringComparison.OrdinalIgnoreCase) == true && templatedPath.EndsWith("/pushnotificationconfigs", StringComparison.OrdinalIgnoreCase) && httpContext.Request.Method == HttpMethods.Get => await ListPushNotificationConfigsAsync(httpContext).ConfigureAwait(false),
            var templatedPath when templatedPath?.StartsWith("/v1/tasks/", StringComparison.OrdinalIgnoreCase) == true && templatedPath.EndsWith("/pushnotificationconfigs/", StringComparison.OrdinalIgnoreCase) && httpContext.Request.Method == HttpMethods.Delete => await DeletePushNotificationConfigAsync(httpContext).ConfigureAwait(false),
            _ => Results.StatusCode(StatusCodes.Status404NotFound)
        };
    }

    async Task<IResult> SendMessageAsync(HttpContext httpContext)
    {
        if (string.IsNullOrWhiteSpace(httpContext.Request.ContentType) || !httpContext.Request.ContentType.StartsWith(MediaTypeNames.Application.Json, StringComparison.OrdinalIgnoreCase)) return Results.StatusCode(StatusCodes.Status415UnsupportedMediaType);
        if (httpContext.Request.Method != HttpMethods.Post) return Results.StatusCode(StatusCodes.Status405MethodNotAllowed);
        Models.SendMessageRequest? request;
        try
        {
            request = await httpContext.Request.ReadFromJsonAsync(JsonSerializationContext.Default.SendMessageRequest, httpContext.RequestAborted).ConfigureAwait(false);
        }
        catch (JsonException ex)
        {
            return Results.Problem(new()
            {
                Title = "Malformed JSON",
                Detail = ex.Message,
                Status = StatusCodes.Status400BadRequest
            });
        }
        if (!IsValid(request, out var problem)) return Results.Problem(problem ?? new()
        {
            Title = "Invalid request payload.",
            Status = StatusCodes.Status400BadRequest,
            Detail = "The request payload is invalid."
        });
        try
        {
            var response = await server.SendMessageAsync(request!, httpContext.RequestAborted).ConfigureAwait(false);
            return Results.Ok(response);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    async Task<IResult> SendStreamingMessageAsync(HttpContext httpContext)
    {
        if (string.IsNullOrWhiteSpace(httpContext.Request.ContentType) || !httpContext.Request.ContentType.StartsWith(MediaTypeNames.Application.Json, StringComparison.OrdinalIgnoreCase)) return Results.StatusCode(StatusCodes.Status415UnsupportedMediaType);
        if (httpContext.Request.Method != HttpMethods.Post) return Results.StatusCode(StatusCodes.Status405MethodNotAllowed);
        Models.SendMessageRequest? request;
        try
        {
            request = await httpContext.Request.ReadFromJsonAsync(JsonSerializationContext.Default.SendMessageRequest, httpContext.RequestAborted).ConfigureAwait(false);
        }
        catch (JsonException ex)
        {
            return Results.Problem(new()
            {
                Title = "Malformed JSON",
                Detail = ex.Message,
                Status = StatusCodes.Status400BadRequest
            });
        }
        if (!IsValid(request, out var problem)) return Results.Problem(problem ?? new()
        {
            Title = "Invalid request payload.",
            Status = StatusCodes.Status400BadRequest,
            Detail = "The request payload is invalid."
        });
        try
        {
            var stream = server.SendStreamingMessageAsync(request!, httpContext.RequestAborted);
            return Results.ServerSentEvents(stream);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    async Task<IResult> GetTaskAsync(HttpContext httpContext)
    {
        if (httpContext.Request.Method != HttpMethods.Get) return Results.StatusCode(StatusCodes.Status405MethodNotAllowed);
        var path = httpContext.Request.Path.Value ?? string.Empty;
        var segments = path.Trim('/').Split('/', StringSplitOptions.RemoveEmptyEntries);
        if (segments.Length < 3 || !string.Equals(segments[0], "v1", StringComparison.OrdinalIgnoreCase) || !string.Equals(segments[1], "tasks", StringComparison.OrdinalIgnoreCase)) return Results.Problem(new()
        {
            Title = "Invalid path.",
            Status = StatusCodes.Status400BadRequest,
            Detail = "The request path is invalid."
        });
        var taskId = segments[2];
        uint? historyLength = null;
        if (httpContext.Request.Query.TryGetValue(QueryParameters.HistoryLength, out var raw) && uint.TryParse(raw.ToString(), out var parsed)) historyLength = parsed;
        try
        {
            var task = await server.GetTaskAsync(taskId, historyLength, httpContext.RequestAborted).ConfigureAwait(false);
            return Results.Ok(task);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    async Task<IResult> ListTasksAsync(HttpContext httpContext)
    {
        if (httpContext.Request.Method != HttpMethods.Get) return Results.StatusCode(StatusCodes.Status405MethodNotAllowed);
        string? contextId = null;
        string? status = null;
        uint? pageSize = null;
        string? pageToken = null;
        uint? historyLength = null;
        uint? lastUpdateAfter = null;
        bool? includeArtifacts = null;
        if (httpContext.Request.Query.TryGetValue(QueryParameters.ContextId, out var raw)) contextId = raw.ToString();
        if (httpContext.Request.Query.TryGetValue(QueryParameters.Status, out raw)) status = raw.ToString();
        if (httpContext.Request.Query.TryGetValue(QueryParameters.PageSize, out raw) && uint.TryParse(raw.ToString(), out var parsedUInt)) pageSize = parsedUInt;
        if (httpContext.Request.Query.TryGetValue(QueryParameters.PageToken, out raw)) pageToken = raw.ToString();
        if (httpContext.Request.Query.TryGetValue(QueryParameters.HistoryLength, out raw) && uint.TryParse(raw.ToString(), out parsedUInt)) historyLength = parsedUInt;
        if (httpContext.Request.Query.TryGetValue(QueryParameters.LastUpdatedAfter, out raw) && uint.TryParse(raw.ToString(), out parsedUInt)) lastUpdateAfter = parsedUInt;
        if (httpContext.Request.Query.TryGetValue(QueryParameters.IncludeArtifacts, out raw) && bool.TryParse(raw.ToString(), out var parsedBoolean)) includeArtifacts = parsedBoolean;
        var queryOptions = new Models.TaskQueryOptions()
        {
            ContextId = contextId,
            Status = status,
            PageSize = pageSize,
            PageToken = pageToken,
            HistoryLength = historyLength,
            LastUpdateAfter = lastUpdateAfter,
            IncludeArtifacts = includeArtifacts,
            Metadata = ParseMetadataFromQuery(httpContext)
        };
        try
        {
            var result = await server.ListTasksAsync(queryOptions, httpContext.RequestAborted).ConfigureAwait(false);
            return Results.Ok(result);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    async Task<IResult> CancelTaskAsync(HttpContext httpContext)
    {
        if (httpContext.Request.Method != HttpMethods.Post) return Results.StatusCode(StatusCodes.Status405MethodNotAllowed);
        var path = httpContext.Request.Path.Value ?? string.Empty;
        var segments = path.Trim('/').Split('/', StringSplitOptions.RemoveEmptyEntries);
        if (segments.Length < 3 || !string.Equals(segments[0], "v1", StringComparison.OrdinalIgnoreCase) || !string.Equals(segments[1], "tasks", StringComparison.OrdinalIgnoreCase)) return Results.Problem(new()
        {
            Title = "Invalid path.",
            Status = StatusCodes.Status400BadRequest,
            Detail = "The request path is invalid."
        });
        var taskId = segments[2];
        try
        {
            var result = await server.CancelTaskAsync(taskId, httpContext.RequestAborted).ConfigureAwait(false);
            return Results.Ok(result);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    async Task<IResult> SubscribeToTaskAsync(HttpContext httpContext)
    {
        if (httpContext.Request.Method != HttpMethods.Post) return Results.StatusCode(StatusCodes.Status405MethodNotAllowed);
        var path = httpContext.Request.Path.Value ?? string.Empty;
        var segments = path.Trim('/').Split('/', StringSplitOptions.RemoveEmptyEntries);
        if (segments.Length < 3 || !string.Equals(segments[0], "v1", StringComparison.OrdinalIgnoreCase) || !string.Equals(segments[1], "tasks", StringComparison.OrdinalIgnoreCase)) return Results.Problem(new()
        {
            Title = "Invalid path.",
            Status = StatusCodes.Status400BadRequest,
            Detail = "The request path is invalid."
        });
        var taskId = segments[2];
        try
        {
            var stream = server.SubscribeToTaskAsync(taskId, httpContext.RequestAborted);
            return Results.ServerSentEvents(stream);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    async Task<IResult> SetOrUpdatePushNotificationConfigAsync(HttpContext httpContext)
    {
        if (httpContext.Request.Method != HttpMethods.Post) return Results.StatusCode(StatusCodes.Status405MethodNotAllowed);
        var path = httpContext.Request.Path.Value ?? string.Empty;
        var segments = path.Trim('/').Split('/', StringSplitOptions.RemoveEmptyEntries);
        if (segments.Length < 3 || !string.Equals(segments[0], "v1", StringComparison.OrdinalIgnoreCase) || !string.Equals(segments[1], "tasks", StringComparison.OrdinalIgnoreCase)) return Results.Problem(new()
        {
            Title = "Invalid path.",
            Status = StatusCodes.Status400BadRequest,
            Detail = "The request path is invalid."
        });
        var taskId = segments[2];
        Models.SetOrUpdatePushNotificationConfigRequest? request;
        try
        {
            request = await httpContext.Request.ReadFromJsonAsync(JsonSerializationContext.Default.SetOrUpdatePushNotificationConfigRequest, httpContext.RequestAborted).ConfigureAwait(false);
        }
        catch (JsonException ex)
        {
            return Results.Problem(new()
            {
                Title = "Malformed JSON",
                Detail = ex.Message,
                Status = StatusCodes.Status400BadRequest
            });
        }
        if (!IsValid(request, out var problem)) return Results.Problem(problem ?? new()
        {
            Title = "Invalid request payload.",
            Status = StatusCodes.Status400BadRequest,
            Detail = "The request payload is invalid."
        });
        if (request!.Parent.Split('/', StringSplitOptions.RemoveEmptyEntries).Length == 2) return Results.Problem(new()
        {
            Title = "Invalid parent format.",
            Status = StatusCodes.Status400BadRequest,
            Detail = "The parent field must be in the format 'tasks/{taskId}'."
        });
        try
        {
            var pushNotificationConfig = server.SetOrUpdatePushNotificationConfigAsync(taskId, new()
            {
                Name = $"tasks/{taskId}/pushNotificationConfigs/{request.ConfigId}",
                PushNotificationConfig = request.Config with
                {
                    Id = request.ConfigId
                }
            }, httpContext.RequestAborted);
            return Results.Ok(pushNotificationConfig);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    async Task<IResult> GetPushNotificationConfigAsync(HttpContext httpContext)
    {
        if (httpContext.Request.Method != HttpMethods.Get) return Results.StatusCode(StatusCodes.Status405MethodNotAllowed);
        var path = httpContext.Request.Path.Value ?? string.Empty;
        var segments = path.Trim('/').Split('/', StringSplitOptions.RemoveEmptyEntries);
        if (segments.Length < 5 || !string.Equals(segments[0], "v1", StringComparison.OrdinalIgnoreCase) || !string.Equals(segments[1], "tasks", StringComparison.OrdinalIgnoreCase) || !string.Equals(segments[3], "pushNotificationConfigs", StringComparison.OrdinalIgnoreCase)) return Results.Problem(new()
        {
            Title = "Invalid path.",
            Status = StatusCodes.Status400BadRequest,
            Detail = "The request path is invalid."
        });
        var taskId = segments[2];
        var configId = segments[4];
        try
        {
            var pushNotificationConfig = await server.GetPushNotificationConfigAsync(taskId, configId, httpContext.RequestAborted).ConfigureAwait(false);
            return Results.Ok(pushNotificationConfig);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    async Task<IResult> ListPushNotificationConfigsAsync(HttpContext httpContext)
    {
        if (httpContext.Request.Method != HttpMethods.Get) return Results.StatusCode(StatusCodes.Status405MethodNotAllowed);
        var path = httpContext.Request.Path.Value ?? string.Empty;
        var segments = path.Trim('/').Split('/', StringSplitOptions.RemoveEmptyEntries);
        if (segments.Length < 4 || !string.Equals(segments[0], "v1", StringComparison.OrdinalIgnoreCase) || !string.Equals(segments[1], "tasks", StringComparison.OrdinalIgnoreCase) || !string.Equals(segments[3], "pushNotificationConfigs", StringComparison.OrdinalIgnoreCase)) return Results.Problem(new()
        {
            Title = "Invalid path.",
            Status = StatusCodes.Status400BadRequest,
            Detail = "The request path is invalid."
        });
        var taskId = segments[2];
        uint? pageSize = null;
        string? pageToken = null;
        if (httpContext.Request.Query.TryGetValue(QueryParameters.PageSize, out var raw) && uint.TryParse(raw.ToString(), out var parsedUInt)) pageSize = parsedUInt;
        if (httpContext.Request.Query.TryGetValue(QueryParameters.PageToken, out raw)) pageToken = raw.ToString();
        var queryOptions = new Models.PushNotificationConfigQueryOptions()
        {
            TaskId = taskId,
            PageSize = pageSize,
            PageToken = pageToken
        };
        try
        {
            var pushNotificationConfigs = await server.ListPushNotificationConfigAsync(queryOptions, httpContext.RequestAborted).ConfigureAwait(false);
            return Results.Ok(pushNotificationConfigs);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    async Task<IResult> DeletePushNotificationConfigAsync(HttpContext httpContext)
    {
        if (httpContext.Request.Method != HttpMethods.Delete) return Results.StatusCode(StatusCodes.Status405MethodNotAllowed);
        var path = httpContext.Request.Path.Value ?? string.Empty;
        var segments = path.Trim('/').Split('/', StringSplitOptions.RemoveEmptyEntries);
        if (segments.Length < 5 || !string.Equals(segments[0], "v1", StringComparison.OrdinalIgnoreCase) || !string.Equals(segments[1], "tasks", StringComparison.OrdinalIgnoreCase) || !string.Equals(segments[3], "pushNotificationConfigs", StringComparison.OrdinalIgnoreCase)) return Results.Problem(new()
        {
            Title = "Invalid path.",
            Status = StatusCodes.Status400BadRequest,
            Detail = "The request path is invalid."
        });
        var taskId = segments[2];
        var configId = segments[4];
        try
        {
            await server.DeletePushNotificationConfigAsync(taskId, configId, httpContext.RequestAborted).ConfigureAwait(false);
            return Results.Ok();
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    static bool IsValid(object? model, out ProblemDetails? problem)
    {
        problem = null;
        if (model == null)
        {
            problem = new()
            {
                Title = "Invalid request payload.",
                Status = StatusCodes.Status400BadRequest,
                Detail = "The request payload is missing or malformed."
            };
            return false;
        }
        var context = new ValidationContext(model);
        var results = new List<ValidationResult>();
        if (Validator.TryValidateObject(model, context, results, validateAllProperties: true)) return true;
        var errors = new Dictionary<string, string[]>(StringComparer.Ordinal);
        foreach (var result in results)
        {
            var members = (result.MemberNames?.Any() == true) ? result.MemberNames : [string.Empty];
            foreach (var member in members)
            {
                errors.TryGetValue(member, out var existing);
                var list = existing?.ToList() ?? new List<string>();
                list.Add(result.ErrorMessage ?? "Validation error.");
                errors[member] = [.. list];
            }
        }
        problem = new ValidationProblemDetails(errors);
        return false;
    }

    static JsonObject? ParseMetadataFromQuery(HttpContext httpContext, string key = "metadata")
    {
        if (!httpContext.Request.Query.TryGetValue(key, out var raw) || raw.Count == 0) return null;
        var json = raw.ToString();
        if (string.IsNullOrWhiteSpace(json)) return null;
        try
        {
            var node = JsonNode.Parse(json);
            if (node is not JsonObject jsonObject) return null;
            return jsonObject;
        }
        catch (JsonException)
        {
            return null;
        }
    }

    static IResult HandleException(Exception ex)
    {
        ProblemDetails problem;
        switch (ex)
        {
            case A2AException a2aException:
                problem = A2AProblem.Map(a2aException.ErrorCode);
                if (!string.IsNullOrWhiteSpace(ex.Message)) problem.Detail = ex.Message;
                return Results.Problem(problem);
            default:
                problem = new ProblemDetails
                {
                    Title = "Internal Server Error",
                    Status = StatusCodes.Status500InternalServerError,
                    Detail = ex.Message
                };
                return Results.Problem(problem);
        }
        
    }

    static class QueryParameters
    {

        public const string ContextId = "contextId";
        public const string Status = "status";
        public const string PageSize = "pageSize";
        public const string PageToken = "pageToken";
        public const string HistoryLength = "historyLength";
        public const string LastUpdatedAfter = "lastUpdatedAfter";
        public const string IncludeArtifacts = "includeArtifacts";
        public const string Metadata = "metadata";

    }

}
