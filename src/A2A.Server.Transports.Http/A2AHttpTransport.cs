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
            _ => Results.StatusCode(StatusCodes.Status404NotFound)
        };
    }

    async Task<IResult> SendMessageAsync(HttpContext httpContext)
    {
        if (string.IsNullOrWhiteSpace(httpContext.Request.ContentType) || !httpContext.Request.ContentType.StartsWith(MediaTypeNames.Application.Json, StringComparison.OrdinalIgnoreCase)) return Results.StatusCode(StatusCodes.Status415UnsupportedMediaType);
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
        catch (A2AException ex)
        {
            problem = A2AProblem.Map(ex.ErrorCode);
            if (!string.IsNullOrWhiteSpace(ex.Message)) problem.Detail = ex.Message;
            return Results.Problem(problem);
        }
    }

    async Task<IResult> SendStreamingMessageAsync(HttpContext httpContext)
    {
        if (string.IsNullOrWhiteSpace(httpContext.Request.ContentType) || !httpContext.Request.ContentType.StartsWith(MediaTypeNames.Application.Json, StringComparison.OrdinalIgnoreCase)) return Results.StatusCode(StatusCodes.Status415UnsupportedMediaType);
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
        catch (A2AException ex)
        {
            problem = A2AProblem.Map(ex.ErrorCode);
            if (!string.IsNullOrWhiteSpace(ex.Message)) problem.Detail = ex.Message;
            return Results.Problem(problem);
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

}
