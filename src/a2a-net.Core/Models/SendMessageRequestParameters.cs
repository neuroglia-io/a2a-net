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
/// Represents the parameters used to configure a <see cref="SendMessageRequest"/>.
/// </summary>
[Description("Represents the parameters used to configure a SendMessageRequest.")]
[DataContract]
public record SendMessageRequestParameters
    : RpcRequestParameters
{

    /// <summary>
    /// Gets or sets the message to send to the server.
    /// </summary>
    [Required]
    [Description("The message to send to the server.")]
    [DataMember(Name = "message", Order = 1), JsonPropertyName("message"), JsonPropertyOrder(1), YamlMember(Alias = "message", Order = 1)]
    public virtual Message Message { get; set; } = null!;

    /// <summary>
    /// Gets or sets the message's configuration, if any.
    /// </summary>
    [Description("The message's configuration, if any.")]
    [DataMember(Name = "configuration", Order = 2), JsonPropertyName("configuration"), JsonPropertyOrder(2), YamlMember(Alias = "configuration", Order = 2)]
    public virtual SendMessageRequestConfiguration? Configuration { get; set; }

}
