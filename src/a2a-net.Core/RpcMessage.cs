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
/// Represents the base class for all A2A RPC messages.
/// </summary>
[Description("Represents the base class for all A2A RPC messages.")]
[DataContract]
public abstract record RpcMessage
{

    /// <summary>
    /// Gets or sets the JSON RPC version to use.
    /// </summary>
    [Description("The JSON RPC version to use.")]
    [Required, AllowedValues(JsonRpcVersion.V2), DefaultValue(JsonRpcVersion.V2)]
    [DataMember(Name = "jsonrpc", Order = 0), JsonPropertyName("jsonrpc"), JsonPropertyOrder(0), YamlMember(Alias = "jsonrpc", Order = 0)]
    public virtual string JsonRpc { get; set; } = JsonRpcVersion.V2;

    /// <summary>
    /// Gets or sets the message's unique identifier.
    /// </summary>
    [Description("The message's unique identifier.")]
    [Required, MinLength(1)]
    [DataMember(Name = "id", Order = 1), JsonPropertyName("id"), JsonPropertyOrder(1), YamlMember(Alias = "id", Order = 1)]
    public virtual string Id { get; set; } = null!;

}