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

using A2A.Models;
using A2A.Server.Transports.Serialization;

namespace A2A.Server.Transports;

/// <summary>
/// Represents the JSON-RPC implementation of the <see cref="IA2ATransport"/> interface.
/// </summary>
/// <param name="server">The A2A server.</param>
public sealed class A2AJsonRpcTransport(IA2AServer server)
    : IA2ATransport
{

    /// <inheritdoc/>
    public string ProtocolBinding => A2A.ProtocolBinding.JsonRpc;

    /// <inheritdoc/>
    public async Task<IResult> HandleAsync(HttpContext httpContext)
    {
        ArgumentNullException.ThrowIfNull(httpContext);
        if (string.IsNullOrWhiteSpace(httpContext.Request.ContentType) || !httpContext.Request.ContentType.StartsWith(MediaTypeNames.Application.Json, StringComparison.OrdinalIgnoreCase))return TypedResults.StatusCode(StatusCodes.Status415UnsupportedMediaType);
        JsonRpcRequest? parameters;
        try
        {
            parameters = await httpContext.Request.ReadFromJsonAsync<JsonRpcRequest>(httpContext.RequestAborted).ConfigureAwait(false);
        }
        catch (JsonException ex)
        {
            return Results.Ok(new JsonRpcResponse()
            {
                Version = "2.0",
                Error = new()
                {
                    Code = JsonRpcErrorCode.ParseError,
                    Message = ex.Message
                }
            });
        }
        if (parameters is null) return Results.Ok(new JsonRpcResponse()
        {
            Version = "2.0",
            Error = new()
            {
                Code = JsonRpcErrorCode.ParseError,
                Message = "Request body is null or could not be deserialized."
            }
        });
        if (!string.Equals(parameters.Version, JsonRpcVersion.V2, StringComparison.Ordinal) || string.IsNullOrWhiteSpace(parameters.Method)) return Results.Ok(new JsonRpcResponse()
        {
            Id = parameters.Id,
            Version = parameters.Version,
            Error = new()
            {
                Code = JsonRpcErrorCode.InvalidRequest,
                Message = "Unsupported JSON-RPC version."
            }
        });
        return parameters.Method switch
        {
            A2AJsonRpcMethod.Message.Send => await SendMessageAsync(parameters, httpContext.RequestAborted).ConfigureAwait(false),
            A2AJsonRpcMethod.Message.SendStreaming => await SendStreamingMessageAsync(parameters, httpContext.RequestAborted).ConfigureAwait(false),
            A2AJsonRpcMethod.Task.Get => await GetTaskAsync(parameters, httpContext.RequestAborted).ConfigureAwait(false),
            A2AJsonRpcMethod.Task.List => await ListTasksAsync(parameters, httpContext.RequestAborted).ConfigureAwait(false),
            A2AJsonRpcMethod.Task.Cancel => await CancelTaskAsync(parameters, httpContext.RequestAborted).ConfigureAwait(false),
            A2AJsonRpcMethod.Task.Subscribe => await SubscribeToTaskAsync(parameters, httpContext.RequestAborted).ConfigureAwait(false),
            A2AJsonRpcMethod.Task.PushNotificationConfig.Set => await SetOrUpdatePushNotificationConfigAsync(parameters, httpContext.RequestAborted).ConfigureAwait(false),
            A2AJsonRpcMethod.Task.PushNotificationConfig.Get => await GetPushNotificationConfigAsync(parameters, httpContext.RequestAborted).ConfigureAwait(false),
            A2AJsonRpcMethod.Task.PushNotificationConfig.List => await ListPushNotificationConfigsAsync(parameters, httpContext.RequestAborted).ConfigureAwait(false),
            A2AJsonRpcMethod.Task.PushNotificationConfig.Delete => await DeletePushNotificationConfigAsync(parameters, httpContext.RequestAborted).ConfigureAwait(false),
            _ => Results.Ok(new JsonRpcResponse()
            {
                Id = parameters.Id,
                Version = parameters.Version,
                Error = new()
                {
                    Code = JsonRpcErrorCode.MethodNotFound,
                    Message = $"The requested method '{parameters.Method}' does not exist or is not available."
                }
            })
        };
    }

    async Task<IResult> SendMessageAsync(JsonRpcRequest rpcRequest, CancellationToken cancellationToken)
    {
        Models.SendMessageRequest? parameters;
        try
        {
            parameters = JsonSerializer.Deserialize(rpcRequest.Params, JsonSerializationContext.Default.SendMessageRequest);
        }
        catch (JsonException ex)
        {
            return Results.Ok(new JsonRpcResponse()
            {
                Id = rpcRequest.Id,
                Version = rpcRequest.Version,
                Error = new()
                {
                    Code = JsonRpcErrorCode.ParseError,
                    Message = ex.Message
                }
            });
        }
        if (!IsValid(parameters, out var message)) return Results.Ok(new JsonRpcResponse()
        {
            Id = rpcRequest.Id,
            Version = rpcRequest.Version,
            Error = new()
            {
                Code = JsonRpcErrorCode.InvalidParams,
                Message = message ?? "Invalid request parameters."
            }
        });
        try
        {
            var response = await server.SendMessageAsync(parameters!, cancellationToken).ConfigureAwait(false);
            return Results.Ok(new JsonRpcResponse()
            {
                Id = rpcRequest.Id,
                Version = rpcRequest.Version,
                Result = response
            });
        }
        catch (Exception ex)
        {
            return HandleException(ex, rpcRequest);
        }
    }

    async Task<IResult> SendStreamingMessageAsync(JsonRpcRequest rpcRequest, CancellationToken cancellationToken)
    {
        Models.SendMessageRequest? request;
        try
        {
            request = JsonSerializer.Deserialize(rpcRequest.Params, JsonSerializationContext.Default.SendMessageRequest);
        }
        catch (JsonException ex)
        {
            return Results.Ok(new JsonRpcResponse()
            {
                Id = rpcRequest.Id,
                Version = rpcRequest.Version,
                Error = new()
                {
                    Code = JsonRpcErrorCode.ParseError,
                    Message = ex.Message
                }
            });
        }
        if (!IsValid(request, out var message)) return Results.Ok(new JsonRpcResponse()
        {
            Id = rpcRequest.Id,
            Version = rpcRequest.Version,
            Error = new()
            {
                Code = JsonRpcErrorCode.InvalidParams,
                Message = message ?? "Invalid request parameters."
            }
        });
        try
        {
            var stream = server.SendStreamingMessageAsync(request!, cancellationToken);
            return Results.ServerSentEvents(stream);
        }
        catch (Exception ex)
        {
            return HandleException(ex, rpcRequest);
        }
    }

    async Task<IResult> GetTaskAsync(JsonRpcRequest rpcRequest, CancellationToken cancellationToken)
    {
        GetTaskMethodParameters? parameters;
        try
        {
            parameters = JsonSerializer.Deserialize(rpcRequest.Params, JsonRpcSerializationContext.Default.GetTaskMethodParameters);
        }
        catch (JsonException ex)
        {
            return Results.Ok(new JsonRpcResponse()
            {
                Id = rpcRequest.Id,
                Version = rpcRequest.Version,
                Error = new()
                {
                    Code = JsonRpcErrorCode.ParseError,
                    Message = ex.Message
                }
            });
        }
        if (!IsValid(parameters, out var message)) return Results.Ok(new JsonRpcResponse()
        {
            Id = rpcRequest.Id,
            Version = rpcRequest.Version,
            Error = new()
            {
                Code = JsonRpcErrorCode.InvalidParams,
                Message = message ?? "Invalid request parameters."
            }
        });
        try
        {
            var task = await server.GetTaskAsync(parameters!.TaskId, parameters.HistoryLength, cancellationToken).ConfigureAwait(false);
            return Results.Ok(new JsonRpcResponse()
            {
                Id = rpcRequest.Id,
                Version = rpcRequest.Version,
                Result = task
            });
        }
        catch (Exception ex)
        {
            return HandleException(ex, rpcRequest);
        }
    }

    async Task<IResult> ListTasksAsync(JsonRpcRequest rpcRequest, CancellationToken cancellationToken)
    {
        TaskQueryOptions? parameters;
        try
        {
            parameters = JsonSerializer.Deserialize(rpcRequest.Params, JsonSerializationContext.Default.TaskQueryOptions);
        }
        catch (JsonException ex)
        {
            return Results.Ok(new JsonRpcResponse()
            {
                Id = rpcRequest.Id,
                Version = rpcRequest.Version,
                Error = new()
                {
                    Code = JsonRpcErrorCode.ParseError,
                    Message = ex.Message
                }
            });
        }
        if (!IsValid(parameters, out var message)) return Results.Ok(new JsonRpcResponse()
        {
            Id = rpcRequest.Id,
            Version = rpcRequest.Version,
            Error = new()
            {
                Code = JsonRpcErrorCode.InvalidParams,
                Message = message ?? "Invalid request parameters."
            }
        });
        try
        {
            var task = await server.ListTasksAsync(parameters, cancellationToken).ConfigureAwait(false);
            return Results.Ok(new JsonRpcResponse()
            {
                Id = rpcRequest.Id,
                Version = rpcRequest.Version,
                Result = task
            });
        }
        catch (Exception ex)
        {
            return HandleException(ex, rpcRequest);
        }
    }

    async Task<IResult> CancelTaskAsync(JsonRpcRequest rpcRequest, CancellationToken cancellationToken)
    {
        CancelTaskMethodParameters? parameters;
        try
        {
            parameters = JsonSerializer.Deserialize(rpcRequest.Params, JsonRpcSerializationContext.Default.CancelTaskMethodParameters);
        }
        catch (JsonException ex)
        {
            return Results.Ok(new JsonRpcResponse()
            {
                Id = rpcRequest.Id,
                Version = rpcRequest.Version,
                Error = new()
                {
                    Code = JsonRpcErrorCode.ParseError,
                    Message = ex.Message
                }
            });
        }
        if (!IsValid(parameters, out var message)) return Results.Ok(new JsonRpcResponse()
        {
            Id = rpcRequest.Id,
            Version = rpcRequest.Version,
            Error = new()
            {
                Code = JsonRpcErrorCode.InvalidParams,
                Message = message ?? "Invalid request parameters."
            }
        });
        try
        {
            var task = await server.CancelTaskAsync(parameters!.Id, cancellationToken).ConfigureAwait(false);
            return Results.Ok(new JsonRpcResponse()
            {
                Id = rpcRequest.Id,
                Version = rpcRequest.Version,
                Result = task
            });
        }
        catch (Exception ex)
        {
            return HandleException(ex, rpcRequest);
        }
    }

    async Task<IResult> SubscribeToTaskAsync(JsonRpcRequest rpcRequest, CancellationToken cancellationToken)
    {
        SubscribeToTaskMethodParameters? parameters;
        try
        {
            parameters = JsonSerializer.Deserialize(rpcRequest.Params, JsonRpcSerializationContext.Default.SubscribeToTaskMethodParameters);
        }
        catch (JsonException ex)
        {
            return Results.Ok(new JsonRpcResponse()
            {
                Id = rpcRequest.Id,
                Version = rpcRequest.Version,
                Error = new()
                {
                    Code = JsonRpcErrorCode.ParseError,
                    Message = ex.Message
                }
            });
        }
        if (!IsValid(parameters, out var message)) return Results.Ok(new JsonRpcResponse()
        {
            Id = rpcRequest.Id,
            Version = rpcRequest.Version,
            Error = new()
            {
                Code = JsonRpcErrorCode.InvalidParams,
                Message = message ?? "Invalid request parameters."
            }
        });
        try
        {
            var stream = server.SubscribeToTaskAsync(parameters!.Id, cancellationToken);
            return Results.ServerSentEvents(stream);
        }
        catch (Exception ex)
        {
            return HandleException(ex, rpcRequest);
        }
    }

    async Task<IResult> SetOrUpdatePushNotificationConfigAsync(JsonRpcRequest rpcRequest, CancellationToken cancellationToken)
    {
        SetOrUpdatePushNotificationConfigRequest? parameters;
        try
        {
            parameters = JsonSerializer.Deserialize(rpcRequest.Params, JsonSerializationContext.Default.SetOrUpdatePushNotificationConfigRequest);
        }
        catch (JsonException ex)
        {
            return Results.Ok(new JsonRpcResponse()
            {
                Id = rpcRequest.Id,
                Version = rpcRequest.Version,
                Error = new()
                {
                    Code = JsonRpcErrorCode.ParseError,
                    Message = ex.Message
                }
            });
        }
        if (!IsValid(parameters, out var message)) return Results.Ok(new JsonRpcResponse()
        {
            Id = rpcRequest.Id,
            Version = rpcRequest.Version,
            Error = new()
            {
                Code = JsonRpcErrorCode.InvalidParams,
                Message = message ?? "Invalid request parameters."
            }
        });
        var segments = parameters!.Parent.Split('/', StringSplitOptions.RemoveEmptyEntries);
        if (segments.Length == 2) return Results.Ok(new JsonRpcResponse()
            {
                Id = rpcRequest.Id,
                Version = rpcRequest.Version,
                Error = new()
                {
                    Code = JsonRpcErrorCode.InvalidParams,
                    Message = "The parent field must be in the format 'tasks/{taskId}'."
                }
            });
        var taskId = segments[1];
        try
        {
            var config = await server.SetOrUpdatePushNotificationConfigAsync(taskId, new()
            {
                Name = parameters.Parent,
                PushNotificationConfig = parameters.Config with
                {
                    Id = parameters.ConfigId
                }
            }, cancellationToken).ConfigureAwait(false);
            return Results.Ok(new JsonRpcResponse()
            {
                Id = rpcRequest.Id,
                Version = rpcRequest.Version,
                Result = config
            });
        }
        catch (Exception ex)
        {
            return HandleException(ex, rpcRequest);
        }
    }

    async Task<IResult> GetPushNotificationConfigAsync(JsonRpcRequest rpcRequest, CancellationToken cancellationToken)
    {
        GetPushNotificationConfigMethodParameters? parameters;
        try
        {
            parameters = JsonSerializer.Deserialize(rpcRequest.Params, JsonRpcSerializationContext.Default.GetPushNotificationConfigMethodParameters);
        }
        catch (JsonException ex)
        {
            return Results.Ok(new JsonRpcResponse()
            {
                Id = rpcRequest.Id,
                Version = rpcRequest.Version,
                Error = new()
                {
                    Code = JsonRpcErrorCode.ParseError,
                    Message = ex.Message
                }
            });
        }
        if (!IsValid(parameters, out var message)) return Results.Ok(new JsonRpcResponse()
        {
            Id = rpcRequest.Id,
            Version = rpcRequest.Version,
            Error = new()
            {
                Code = JsonRpcErrorCode.InvalidParams,
                Message = message ?? "Invalid request parameters."
            }
        });
        try
        {
            var config = await server.GetPushNotificationConfigAsync(parameters!.TaskId, parameters.ConfigId, cancellationToken).ConfigureAwait(false);
            return Results.Ok(new JsonRpcResponse()
            {
                Id = rpcRequest.Id,
                Version = rpcRequest.Version,
                Result = config
            });
        }
        catch (Exception ex)
        {
            return HandleException(ex, rpcRequest);
        }
    }

    async Task<IResult> ListPushNotificationConfigsAsync(JsonRpcRequest rpcRequest, CancellationToken cancellationToken)
    {
        PushNotificationConfigQueryOptions? parameters;
        try
        {
            parameters = JsonSerializer.Deserialize(rpcRequest.Params, JsonSerializationContext.Default.PushNotificationConfigQueryOptions);
        }
        catch (JsonException ex)
        {
            return Results.Ok(new JsonRpcResponse()
            {
                Id = rpcRequest.Id,
                Version = rpcRequest.Version,
                Error = new()
                {
                    Code = JsonRpcErrorCode.ParseError,
                    Message = ex.Message
                }
            });
        }
        if (!IsValid(parameters, out var message)) return Results.Ok(new JsonRpcResponse()
        {
            Id = rpcRequest.Id,
            Version = rpcRequest.Version,
            Error = new()
            {
                Code = JsonRpcErrorCode.InvalidParams,
                Message = message ?? "Invalid request parameters."
            }
        });
        try
        {
            var task = await server.ListPushNotificationConfigAsync(parameters, cancellationToken).ConfigureAwait(false);
            return Results.Ok(new JsonRpcResponse()
            {
                Id = rpcRequest.Id,
                Version = rpcRequest.Version,
                Result = task
            });
        }
        catch (Exception ex)
        {
            return HandleException(ex, rpcRequest);
        }
    }

    async Task<IResult> DeletePushNotificationConfigAsync(JsonRpcRequest rpcRequest, CancellationToken cancellationToken)
    {
        DeletePushNotificationConfigMethodParameters? parameters;
        try
        {
            parameters = JsonSerializer.Deserialize(rpcRequest.Params, JsonRpcSerializationContext.Default.DeletePushNotificationConfigMethodParameters);
        }
        catch (JsonException ex)
        {
            return Results.Ok(new JsonRpcResponse()
            {
                Id = rpcRequest.Id,
                Version = rpcRequest.Version,
                Error = new()
                {
                    Code = JsonRpcErrorCode.ParseError,
                    Message = ex.Message
                }
            });
        }
        if (!IsValid(parameters, out var message)) return Results.Ok(new JsonRpcResponse()
        {
            Id = rpcRequest.Id,
            Version = rpcRequest.Version,
            Error = new()
            {
                Code = JsonRpcErrorCode.InvalidParams,
                Message = message ?? "Invalid request parameters."
            }
        });
        try
        {
            await server.DeletePushNotificationConfigAsync(parameters!.TaskId, parameters.ConfigId, cancellationToken).ConfigureAwait(false);
            return Results.Ok(new JsonRpcResponse()
            {
                Id = rpcRequest.Id,
                Version = rpcRequest.Version
            });
        }
        catch (Exception ex)
        {
            return HandleException(ex, rpcRequest);
        }
    }

    static bool IsValid(object? model, out string? message)
    {
        message = null;
        if (model == null)
        {
            message = "The request payload is missing or malformed.";
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
        message = $"Validation failed:\n{string.Join('\n', errors.Select(kvp => kvp.Value.Length == 1 ? $"- {kvp.Key}: {kvp.Value[0]}" : $"- {kvp.Key}:\n{string.Join('\n', kvp.Value.Select(v => $"  - {v}"))}"))}";
        return false;
    }

    static IResult HandleException(Exception ex, JsonRpcRequest rpcRequest)
    {
        return ex switch
        {
            A2AException a2aException => Results.Ok(new JsonRpcResponse()
            {
                Id = rpcRequest.Id,
                Version = rpcRequest.Version,
                Error = new()
                {
                    Code = a2aException.ErrorCode,
                    Message = a2aException.Message
                }
            }),
            Exception exception => Results.Ok(new JsonRpcResponse()
            {
                Id = rpcRequest.Id,
                Version = rpcRequest.Version,
                Error = new()
                {
                    Code = JsonRpcErrorCode.InternalError,
                    Message = exception.Message
                }
            }),
            _ => Results.Ok(new JsonRpcResponse()
            {
                Id = rpcRequest.Id,
                Version = rpcRequest.Version,
                Error = new()
                {
                    Code = JsonRpcErrorCode.InternalError
                }
            }),
        };
    }

}
