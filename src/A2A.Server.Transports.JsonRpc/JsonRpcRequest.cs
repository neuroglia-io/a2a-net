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
