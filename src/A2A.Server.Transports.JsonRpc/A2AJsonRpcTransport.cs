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
/// Represents the JSON-RPC implementation of the <see cref="IA2ATransport"/> interface.
/// </summary>
public sealed class A2AJsonRpcTransport
    : IA2ATransport
{

    /// <inheritdoc/>
    public string ProtocolBinding => A2A.ProtocolBinding.JsonRpc;

    /// <inheritdoc/>
    public async Task<IResult> HandleAsync(HttpContext httpContext)
    {
        ArgumentNullException.ThrowIfNull(httpContext);
        if (string.IsNullOrWhiteSpace(httpContext.Request.ContentType) || !httpContext.Request.ContentType.StartsWith(MediaTypeNames.Application.Json, StringComparison.OrdinalIgnoreCase))return TypedResults.StatusCode(StatusCodes.Status415UnsupportedMediaType);
        JsonRpcRequest? request;
        try
        {
            request = await httpContext.Request.ReadFromJsonAsync<JsonRpcRequest>(httpContext.RequestAborted).ConfigureAwait(false);
        }
        catch (JsonException ex)
        {
            return Results.Ok(new JsonRpcResponse()
            {
                JsonRpc = "2.0",
                Error = new()
                {
                    Code = JsonRpcErrorCode.ParseError,
                    Message = ex.Message
                }
            });
        }
        if (request is null) return Results.Ok(new JsonRpcResponse()
        {
            JsonRpc = "2.0",
            Error = new()
            {
                Code = JsonRpcErrorCode.ParseError,
                Message = "Request body is null or could not be deserialized."
            }
        });
        if (!string.Equals(request.JsonRpc, "2.0", StringComparison.Ordinal) || string.IsNullOrWhiteSpace(request.Method)) return Results.Ok(new JsonRpcResponse()
        {
            Id = request.Id,
            JsonRpc = request.JsonRpc,
            Error = new()
            {
                Code = JsonRpcErrorCode.InvalidRequest,
                Message = "Unsupported JSON-RPC version."
            }
        });
        return request.Method switch
        {
            A2AJsonRpcMethod.Message.Send => await SendMessageAsync(request).ConfigureAwait(false),
            A2AJsonRpcMethod.Message.SendStreaming => await SendMessageStreamingAsync(request).ConfigureAwait(false),
            _ => Results.Ok(new JsonRpcResponse()
            {
                Id = request.Id,
                JsonRpc = request.JsonRpc,
                Error = new()
                {
                    Code = JsonRpcErrorCode.MethodNotFound,
                    Message = $"The requested method '{request.Method}' does not exist or is not available."
                }
            })
        };
    }

    async Task<IResult> SendMessageAsync(JsonRpcRequest rpcRequest)
    {
        var request = rpcRequest.Params.Deserialize(JsonSerializationContext.Default.SendMessageRequest) ?? throw new NullReferenceException(); //todo: replace
        return Results.Ok(new JsonRpcResponse()
        {
            Id = rpcRequest.Id,
            JsonRpc = rpcRequest.JsonRpc,
            Result = request.Message
        });
    }

    async Task<IResult> SendMessageStreamingAsync(JsonRpcRequest rpcRequest)
    {
        var request = rpcRequest.Params.Deserialize(JsonSerializationContext.Default.SendMessageRequest) ?? throw new NullReferenceException(); //todo: replace
        return Results.ServerSentEvents(new List<Models.Response>()
        {
            request.Message
        }.ToAsyncEnumerable());
    }

}


/// <summary>
/// Represents the base class for all A2A RPC messages.
/// </summary>
[Description("Represents the base class for all A2A RPC messages.")]
[DataContract]
public abstract record JsonRpcMessage
{

    /// <summary>
    /// Gets or sets the JSON RPC version to use.
    /// </summary>
    [Description("The JSON RPC version to use.")]
    [Required, AllowedValues(JsonRpcVersion.V2), DefaultValue(JsonRpcVersion.V2)]
    [DataMember(Name = "jsonrpc", Order = 0), JsonPropertyName("jsonrpc"), JsonPropertyOrder(0)]
    public virtual string JsonRpc { get; set; } = JsonRpcVersion.V2;

    /// <summary>
    /// Gets or sets the message's unique identifier.
    /// </summary>
    [Description("The message's unique identifier.")]
    [Required, MinLength(1)]
    [DataMember(Name = "id", Order = 1), JsonPropertyName("id"), JsonPropertyOrder(1)]
    public virtual string Id { get; set; } = null!;

}


/// <summary>
/// Represents the base class for all A2A RPC requests.
/// </summary>
[Description("Represents the base class for all A2A RPC requests.")]
[DataContract]
public sealed record JsonRpcRequest
    : JsonRpcMessage
{

    /// <summary>
    /// Initializes a new <see cref="JsonRpcRequest"/>.
    /// </summary>
    public JsonRpcRequest()
    {
        Id = Guid.NewGuid().ToString("N");
    }

    /// <summary>
    /// Initializes a new <see cref="JsonRpcRequest"/>.
    /// </summary>
    /// <param name="method">The name of the RPC method to invoke.</param>
    public JsonRpcRequest(string method)
        : this()
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(method);
        Method = method;
    }

    /// <summary>
    /// Gets or sets the name of the RPC method to invoke.
    /// </summary>
    [Description("The name of the RPC method to invoke.")]
    [Required, MinLength(1)]
    [DataMember(Name = "method", Order = 2), JsonInclude, JsonPropertyName("method"), JsonPropertyOrder(2)]
    public string Method { get; set; } = null!;

    /// <summary>
    /// Gets or sets the request's parameters.
    /// </summary>
    [Description("The request's parameters.")]
    [Required]
    [DataMember(Name = "params", Order = 3), JsonInclude, JsonPropertyName("params"), JsonPropertyOrder(3)]
    public JsonObject Params { get; set; } = null!;

}


/// <summary>
/// Represents an object used to describe an error that has occurred during an RPC call.
/// </summary>
[Description("Represents an object used to describe an error that has occurred during an RPC call.")]
[DataContract]
public sealed record JsonRpcError
{

    /// <summary>
    /// Initializes a new <see cref="JsonRpcError"/>
    /// </summary>
    public JsonRpcError() { }

    /// <summary>
    /// Initializes a new <see cref="JsonRpcError"/>
    /// </summary>
    /// <param name="code">The error code</param>
    /// <param name="message">The error message</param>
    /// <param name="data">Data, if any, associated to the error</param>
    public JsonRpcError(int code, string message, object? data = null)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(message);
        Code = code;
        Message = message;
        Data = data;
    }

    /// <summary>
    /// Gets or sets the error code.
    /// </summary>
    [Description("The error code.")]
    [DataMember(Name = "code", Order = 1), JsonPropertyName("code"), JsonPropertyOrder(1)]
    public int Code { get; set; }

    /// <summary>
    /// Gets or sets the error message.
    /// </summary>
    [Description("The error message.")]
    [Required, MinLength(1)]
    [DataMember(Name = "message", Order = 2), JsonPropertyName("message"), JsonPropertyOrder(2)]
    public string Message { get; set; } = null!;

    /// <summary>
    /// Gets or sets data, if any, associated to the error.
    /// </summary>
    [Description("Data, if any, associated to the error.")]
    [DataMember(Name = "data", Order = 3), JsonPropertyName("data"), JsonPropertyOrder(3)]
    public object? Data { get; set; }

}


/// <summary>
/// Represents the base class for all A2A responses.
/// </summary>
[Description("Represents the base class for all A2A RPC responses.")]
[DataContract]
public sealed record JsonRpcResponse
    : JsonRpcMessage
{

    /// <summary>
    /// Gets or sets the error, if any, that has occurred during the request's execution.
    /// </summary>
    [Description("The error, if any, that has occurred during the request's execution.")]
    [DataMember(Name = "error", Order = 2), JsonPropertyName("error"), JsonPropertyOrder(2)]
    public JsonRpcError? Error { get; set; } = null!;

    /// <summary>
    /// Gets or sets the response's content.
    /// </summary>
    [Description("The response's content.")]
    [Required]
    [DataMember(Name = "result", Order = 2), JsonInclude, JsonPropertyName("result"), JsonPropertyOrder(2)]
    public object? Result { get; set; } = null!;

}


/// <summary>
/// Enumerates all supported versions of the JSON RPC specification
/// </summary>
public static class JsonRpcVersion
{

    /// <summary>
    /// Indicates JSON RPC 2.0
    /// </summary>
    public const string V2 = "2.0";

    /// <summary>
    /// Gets a new <see cref="IEnumerable{T}"/> containing all supported values
    /// </summary>
    /// <returns>An <see cref="IEnumerable{T}"/> containing all supported values</returns>
    public static IEnumerable<string> AsEnumerable()
    {
        yield return V2;
    }

}

/// <summary>
/// Enumerates all supported JSON-RPC error codes.
/// </summary>
public static class JsonRpcErrorCode
{

    /// <summary>
    /// Indicates an invalid request.
    /// </summary>
    public const int InvalidRequest = -32600;
    /// <summary>
    /// Indicates that the method does not exist.
    /// </summary>
    public const int MethodNotFound = -32601;
    /// <summary>
    /// Indicates that the invalid parameters were provided.
    /// </summary>
    public const int InvalidParams = -32602;
    /// <summary>
    /// Indicates an internal error.
    /// </summary>
    public const int InternalError = -32603;
    /// <summary>
    /// Indicates a parse error.
    /// </summary>
    public const int ParseError = -32700;

}