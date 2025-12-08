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

namespace A2A.Server.Transports;

/// <summary>
/// Represents the JSON-RPC implementation of the <see cref="IA2AServerTransport"/> interface.
/// </summary>
/// <param name="server">The A2A server.</param>
public sealed class A2AJsonRpcServerTransport(IA2AServer server)
    : IA2AServerTransport
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
        SendMessageRequest? parameters;
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
                Result = JsonSerializer.SerializeToNode(response, JsonSerializationContext.Default.Response)!.AsObject()
            });
        }
        catch (Exception ex)
        {
            return HandleException(ex, rpcRequest);
        }
    }

    async Task<IResult> SendStreamingMessageAsync(JsonRpcRequest rpcRequest, CancellationToken cancellationToken)
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
            var stream = server.SendStreamingMessageAsync(parameters!, cancellationToken);
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
            var task = await server.GetTaskAsync(parameters!.TaskId, parameters.HistoryLength, parameters.Tenant, cancellationToken).ConfigureAwait(false);
            return Results.Ok(new JsonRpcResponse()
            {
                Id = rpcRequest.Id,
                Version = rpcRequest.Version,
                Result = JsonSerializer.SerializeToNode(task, JsonSerializationContext.Default.Task)!.AsObject()
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
                Result = JsonSerializer.SerializeToNode(task, JsonSerializationContext.Default.TaskQueryResult)!.AsObject()
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
            var task = await server.CancelTaskAsync(parameters!.Id, parameters.Tenant, cancellationToken).ConfigureAwait(false);
            return Results.Ok(new JsonRpcResponse()
            {
                Id = rpcRequest.Id,
                Version = rpcRequest.Version,
                Result = JsonSerializer.SerializeToNode(task, JsonSerializationContext.Default.Task)!.AsObject()
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
            var stream = server.SubscribeToTaskAsync(parameters!.Id, parameters.Tenant, cancellationToken);
            return Results.ServerSentEvents(stream);
        }
        catch (Exception ex)
        {
            return HandleException(ex, rpcRequest);
        }
    }

    async Task<IResult> SetOrUpdatePushNotificationConfigAsync(JsonRpcRequest rpcRequest, CancellationToken cancellationToken)
    {
        SetTaskPushNotificationConfigRequest? parameters;
        try
        {
            parameters = JsonSerializer.Deserialize(rpcRequest.Params, JsonSerializationContext.Default.SetTaskPushNotificationConfigRequest);
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
            var config = await server.SetTaskPushNotificationConfigAsync(parameters!, cancellationToken).ConfigureAwait(false);
            return Results.Ok(new JsonRpcResponse()
            {
                Id = rpcRequest.Id,
                Version = rpcRequest.Version,
                Result = JsonSerializer.SerializeToNode(config, JsonSerializationContext.Default.TaskPushNotificationConfig)!.AsObject()
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
            var config = await server.GetTaskPushNotificationConfigAsync(parameters!.TaskId, parameters.ConfigId, parameters.Tenant, cancellationToken).ConfigureAwait(false);
            return Results.Ok(new JsonRpcResponse()
            {
                Id = rpcRequest.Id,
                Version = rpcRequest.Version,
                Result = JsonSerializer.SerializeToNode(config, JsonSerializationContext.Default.TaskPushNotificationConfig)!.AsObject()
            });
        }
        catch (Exception ex)
        {
            return HandleException(ex, rpcRequest);
        }
    }

    async Task<IResult> ListPushNotificationConfigsAsync(JsonRpcRequest rpcRequest, CancellationToken cancellationToken)
    {
        TaskPushNotificationConfigQueryOptions? parameters;
        try
        {
            parameters = JsonSerializer.Deserialize(rpcRequest.Params, JsonSerializationContext.Default.TaskPushNotificationConfigQueryOptions);
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
            var task = await server.ListTaskPushNotificationConfigAsync(parameters!, cancellationToken).ConfigureAwait(false);
            return Results.Ok(new JsonRpcResponse()
            {
                Id = rpcRequest.Id,
                Version = rpcRequest.Version,
                Result = JsonSerializer.SerializeToNode(task, JsonSerializationContext.Default.TaskPushNotificationConfigQueryResult)!.AsObject()
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
            await server.DeletePushNotificationConfigAsync(parameters!.TaskId, parameters.ConfigId, parameters.Tenant, cancellationToken).ConfigureAwait(false);
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
                var list = existing?.ToList() ?? [];
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
