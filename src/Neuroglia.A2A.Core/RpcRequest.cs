// Copyright � 2025-Present Neuroglia SRL
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

namespace Neuroglia.A2A;

/// <summary>
/// Represents the base class for all A2A RPC requests
/// </summary>
[DataContract]
public abstract record RpcRequest
    : RpcMessage
{

    /// <summary>
    /// Initializes a new <see cref="RpcRequest"/>
    /// </summary>
    public RpcRequest()
    {
        Id = Guid.NewGuid().ToString("N");
    }

    /// <summary>
    /// Gets/sets the name of the A2A Protocol method to use
    /// </summary>
    [Required, MinLength(1)]
    [DataMember(Name = "method", Order = 2), JsonInclude, JsonPropertyName("method"), JsonPropertyOrder(2), YamlMember(Alias = "method", Order = 2)]
    public abstract string Method { get; }

}

/// <summary>
/// Represents the base class for all A2A RPC requests
/// </summary>
/// <typeparam name="TParams">The type of the request's parameters</typeparam>
[DataContract]
public abstract record RpcRequest<TParams>
    : RpcRequest
    where TParams : class
{

    /// <summary>
    /// Gets/sets the request's parameters
    /// </summary>
    [Required]
    [DataMember(Name = "params", Order = 3), JsonInclude, JsonPropertyName("params"), JsonPropertyOrder(3), YamlMember(Alias = "params", Order = 3)]
    public virtual TParams Params { get; set; } = null!;

}
