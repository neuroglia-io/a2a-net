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
/// Represents the base class for all A2A RPC requests.
/// </summary>
[Description("Represents the base class for all A2A RPC requests.")]
[DataContract]
public record RpcRequest
    : RpcMessage
{

    /// <summary>
    /// Initializes a new <see cref="RpcRequest"/>.
    /// </summary>
    public RpcRequest()
    {
        Id = Guid.NewGuid().ToString("N");
    }

    /// <summary>
    /// Initializes a new <see cref="RpcRequest"/>.
    /// </summary>
    /// <param name="method">The name of the RPC method to invoke.</param>
    public RpcRequest(string method)
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
    [DataMember(Name = "method", Order = 2), JsonInclude, JsonPropertyName("method"), JsonPropertyOrder(2), YamlMember(Alias = "method", Order = 2)]
    public virtual string Method { get; protected set; } = null!;

    /// <summary>
    /// Gets or sets a key/value mapping, if any, containing extension properties.
    /// </summary>
    [Description("A key/value mapping, if any, containing extension properties.")]
    [JsonExtensionData]
    public virtual IDictionary<string, object>? ExtensionData { get; set; }

}

/// <summary>
/// Represents the base class for all A2A RPC requests.
/// </summary>
/// <typeparam name="TParams">The type of the request's parameters.</typeparam>
[Description("Represents the base class for all A2A RPC requests with parameters.")]
[DataContract]
public record RpcRequest<TParams>
    : RpcRequest
    where TParams : RpcRequestParameters
{

    /// <summary>
    /// Initializes a new <see cref="RpcRequest"/>.
    /// </summary>
    public RpcRequest() : base() { }

    /// <summary>
    /// Initializes a new <see cref="RpcRequest"/>.
    /// </summary>
    /// <param name="method">The name of the RPC method to invoke.</param>
    public RpcRequest(string method)
        : base(method)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(method);
    }

    /// <summary>
    /// Initializes a new <see cref="RpcRequest"/>.
    /// </summary>
    /// <param name="method">The name of the RPC method to invoke.</param>
    /// <param name="parameters">The request's parameters.</param>
    public RpcRequest(string method, TParams parameters)
        : this(method) 
    { 
        ArgumentNullException.ThrowIfNull(parameters);
        Params = parameters;
    }

    /// <summary>
    /// Gets or sets the request's parameters.
    /// </summary>
    [Description("The request's parameters.")]
    [Required]
    [DataMember(Name = "params", Order = 3), JsonInclude, JsonPropertyName("params"), JsonPropertyOrder(3), YamlMember(Alias = "params", Order = 3)]
    public virtual TParams Params { get; set; } = null!;

}
