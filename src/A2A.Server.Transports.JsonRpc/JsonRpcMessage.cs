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
/// Represents the base class for all A2A JSON-RPC messages.
/// </summary>
[Description("Represents the base class for all A2A JSON-RPC messages.")]
[DataContract]
public abstract record JsonRpcMessage
{

    /// <summary>
    /// Gets or sets the JSON-RPC version to use.
    /// </summary>
    [Description("The JSON-RPC version to use.")]
    [Required, AllowedValues(JsonRpcVersion.V2), DefaultValue(JsonRpcVersion.V2)]
    [DataMember(Order = 0, Name = "jsonrpc"), JsonPropertyOrder(0), JsonPropertyName("jsonrpc")]
    public virtual string Version { get; set; } = JsonRpcVersion.V2;

    /// <summary>
    /// Gets or sets the message's unique identifier.
    /// </summary>
    [Description("The message's unique identifier.")]
    [Required, MinLength(1)]
    [DataMember(Order = 1, Name = "id"), JsonPropertyOrder(1), JsonPropertyName("id")]
    public virtual string Id { get; set; } = null!;

}
