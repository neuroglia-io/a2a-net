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

namespace A2A.Models;

/// <summary>
/// Represents an interface exposed by an agent.
/// </summary>
[Description("Represents an interface exposed by an agent.")]
[DataContract]
public sealed record AgentInterface
{

    /// <summary>
    /// Gets the protocol binding used by the interface.
    /// </summary>
    [Description("The protocol binding used by the interface.")]
    [Required, AllowedValues(A2A.ProtocolBinding.Grpc, A2A.ProtocolBinding.Http, A2A.ProtocolBinding.JsonRpc)]
    [DataMember(Order = 1, Name = "protocolBinding"), JsonPropertyOrder(1), JsonPropertyName("protocolBinding")]
    public required string ProtocolBinding { get; init; }

    /// <summary>
    /// Gets the url where the interface is available.
    /// </summary>
    [Description("The url where the interface is available.")]
    [Required]
    [DataMember(Order = 2, Name = "url"), JsonPropertyOrder(2), JsonPropertyName("url")]
    public required Uri Url { get; init; }

}
