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
