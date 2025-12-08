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
/// Represents the HTTP implementation of the <see cref="IA2AServerTransport"/> interface.
/// </summary>
/// <param name="server">The A2A server.</param>
public sealed class A2AHttpServerTransport(IA2AServer server)
    : IA2AServerTransport
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
            var templatedPath when templatedPath?.StartsWith("/v1/tasks/", StringComparison.OrdinalIgnoreCase) == true && httpContext.Request.Method == HttpMethods.Get => await GetTaskAsync(httpContext).ConfigureAwait(false),
            var templatedPath when templatedPath?.StartsWith("/v1/tasks", StringComparison.OrdinalIgnoreCase) == true && httpContext.Request.Method == HttpMethods.Get => await ListTasksAsync(httpContext).ConfigureAwait(false),
            var templatedPath when templatedPath?.StartsWith("/v1/tasks/", StringComparison.OrdinalIgnoreCase) == true && templatedPath.EndsWith(":cancel", StringComparison.OrdinalIgnoreCase) => await CancelTaskAsync(httpContext).ConfigureAwait(false),
            var templatedPath when templatedPath?.StartsWith("/v1/tasks/", StringComparison.OrdinalIgnoreCase) == true && templatedPath.EndsWith(":subscribe", StringComparison.OrdinalIgnoreCase) => await SubscribeToTaskAsync(httpContext).ConfigureAwait(false),
            var templatedPath when templatedPath?.StartsWith("/v1/tasks/", StringComparison.OrdinalIgnoreCase) == true && templatedPath.EndsWith("/pushnotificationconfigs", StringComparison.OrdinalIgnoreCase) && httpContext.Request.Method == HttpMethods.Post => await SetTaskPushNotificationConfigAsync(httpContext).ConfigureAwait(false),
            var templatedPath when templatedPath?.StartsWith("/v1/tasks/", StringComparison.OrdinalIgnoreCase) == true && templatedPath.Contains("/pushnotificationconfigs/") && httpContext.Request.Method == HttpMethods.Get => await GetTaskPushNotificationConfigAsync(httpContext).ConfigureAwait(false),
            var templatedPath when templatedPath?.StartsWith("/v1/tasks/", StringComparison.OrdinalIgnoreCase) == true && templatedPath.EndsWith("/pushnotificationconfigs", StringComparison.OrdinalIgnoreCase) && httpContext.Request.Method == HttpMethods.Get => await ListTaskPushNotificationConfigsAsync(httpContext).ConfigureAwait(false),
            var templatedPath when templatedPath?.StartsWith("/v1/tasks/", StringComparison.OrdinalIgnoreCase) == true && templatedPath.EndsWith("/pushnotificationconfigs/", StringComparison.OrdinalIgnoreCase) && httpContext.Request.Method == HttpMethods.Delete => await DeleteTaskPushNotificationConfigAsync(httpContext).ConfigureAwait(false),
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
        string? tenant = null;
        if (httpContext.Request.Query.TryGetValue(QueryParameters.HistoryLength, out var raw) && uint.TryParse(raw.ToString(), out var parsed)) historyLength = parsed;
        if (httpContext.Request.Query.TryGetValue("tenant", out raw)) tenant = raw.ToString();
        try
        {
            var task = await server.GetTaskAsync(taskId, historyLength, tenant, httpContext.RequestAborted).ConfigureAwait(false);
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
        string? tenant = null;
        if (httpContext.Request.Query.TryGetValue(QueryParameters.ContextId, out var raw)) contextId = raw.ToString();
        if (httpContext.Request.Query.TryGetValue(QueryParameters.Status, out raw)) status = raw.ToString();
        if (httpContext.Request.Query.TryGetValue(QueryParameters.PageSize, out raw) && uint.TryParse(raw.ToString(), out var parsedUInt)) pageSize = parsedUInt;
        if (httpContext.Request.Query.TryGetValue(QueryParameters.PageToken, out raw)) pageToken = raw.ToString();
        if (httpContext.Request.Query.TryGetValue(QueryParameters.HistoryLength, out raw) && uint.TryParse(raw.ToString(), out parsedUInt)) historyLength = parsedUInt;
        if (httpContext.Request.Query.TryGetValue(QueryParameters.LastUpdatedAfter, out raw) && uint.TryParse(raw.ToString(), out parsedUInt)) lastUpdateAfter = parsedUInt;
        if (httpContext.Request.Query.TryGetValue(QueryParameters.IncludeArtifacts, out raw) && bool.TryParse(raw.ToString(), out var parsedBoolean)) includeArtifacts = parsedBoolean;
        if (httpContext.Request.Query.TryGetValue("tenant", out raw)) tenant = raw.ToString();
        var queryOptions = new Models.TaskQueryOptions()
        {
            ContextId = contextId,
            Status = status,
            PageSize = pageSize,
            PageToken = pageToken,
            HistoryLength = historyLength,
            LastUpdateAfter = lastUpdateAfter,
            IncludeArtifacts = includeArtifacts,
            Metadata = ParseMetadataFromQuery(httpContext),
            Tenant = tenant
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
        var taskId = segments[2].Split(':').First();
        string? tenant = null;
        if (httpContext.Request.Query.TryGetValue("tenant", out var raw)) tenant = raw.ToString();
        try
        {
            var result = await server.CancelTaskAsync(taskId, tenant, httpContext.RequestAborted).ConfigureAwait(false);
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
        var taskId = segments[2].Split(':').First();
        string? tenant = null;
        if (httpContext.Request.Query.TryGetValue("tenant", out var raw)) tenant = raw.ToString();
        try
        {
            var stream = server.SubscribeToTaskAsync(taskId, tenant, httpContext.RequestAborted);
            return Results.ServerSentEvents(stream);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    async Task<IResult> SetTaskPushNotificationConfigAsync(HttpContext httpContext)
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
        Models.SetTaskPushNotificationConfigRequest? request;
        try
        {
            request = await httpContext.Request.ReadFromJsonAsync(JsonSerializationContext.Default.SetTaskPushNotificationConfigRequest, httpContext.RequestAborted).ConfigureAwait(false);
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
            var pushNotificationConfig = server.SetTaskPushNotificationConfigAsync(request, httpContext.RequestAborted);
            return Results.Ok(pushNotificationConfig);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    async Task<IResult> GetTaskPushNotificationConfigAsync(HttpContext httpContext)
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
        string? tenant = null;
        if (httpContext.Request.Query.TryGetValue("tenant", out var raw)) tenant = raw.ToString();
        try
        {
            var pushNotificationConfig = await server.GetTaskPushNotificationConfigAsync(taskId, configId, tenant, httpContext.RequestAborted).ConfigureAwait(false);
            return Results.Ok(pushNotificationConfig);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    async Task<IResult> ListTaskPushNotificationConfigsAsync(HttpContext httpContext)
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
        string? tenant = null;
        if (httpContext.Request.Query.TryGetValue(QueryParameters.PageSize, out var raw) && uint.TryParse(raw.ToString(), out var parsedUInt)) pageSize = parsedUInt;
        if (httpContext.Request.Query.TryGetValue(QueryParameters.PageToken, out raw)) pageToken = raw.ToString();
        if (httpContext.Request.Query.TryGetValue("tenant", out raw)) tenant = raw.ToString();
        var queryOptions = new Models.TaskPushNotificationConfigQueryOptions()
        {
            TaskId = taskId,
            PageSize = pageSize,
            PageToken = pageToken,
            Tenant = tenant
        };
        try
        {
            var pushNotificationConfigs = await server.ListTaskPushNotificationConfigAsync(queryOptions, httpContext.RequestAborted).ConfigureAwait(false);
            return Results.Ok(pushNotificationConfigs);
        }
        catch (Exception ex)
        {
            return HandleException(ex);
        }
    }

    async Task<IResult> DeleteTaskPushNotificationConfigAsync(HttpContext httpContext)
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
        string? tenant = null;
        if (httpContext.Request.Query.TryGetValue("tenant", out var raw)) tenant = raw.ToString();
        try
        {
            await server.DeletePushNotificationConfigAsync(taskId, configId, tenant, httpContext.RequestAborted).ConfigureAwait(false);
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
