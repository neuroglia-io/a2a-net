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
/// Represents a request to send a message.
/// </summary>
[Description("Represents a request to send a message.")]
[DataContract]
public sealed record SendMessageRequest
{

    /// <summary>
    /// Gets the message to send.
    /// </summary>
    [Description("The message to send.")]
    [Required]
    [DataMember(Order = 1, Name = "message"), JsonPropertyOrder(1), JsonPropertyName("message")]
    public required Message Message { get; init; }

    /// <summary>
    /// Gets the request's configuration, if any.
    /// </summary>
    [Description("The request's configuration, if any.")]
    [DataMember(Order = 2, Name = "configuration"), JsonPropertyOrder(2), JsonPropertyName("configuration")]
    public SendMessageConfiguration? Configuration { get; init; }

    /// <summary>
    /// Gets the identifier of the tenant, if any, on whose behalf the request is made.
    /// </summary>
    [Description("The identifier of the tenant, if any, on whose behalf the request is made.")]
    [DataMember(Order = 3, Name = "tenant"), JsonPropertyOrder(3), JsonPropertyName("tenant")]
    public string? Tenant { get; init; }

    /// <summary>
    /// Gets a key/value mapping, if any, containing additional context or parameters for the request.
    /// </summary>
    [Description("A key/value mapping, if any, containing additional context or parameters for the request.")]
    [DataMember(Order = 4, Name = "metadata"), JsonPropertyOrder(4), JsonPropertyName("metadata")]
    public JsonObject? Metadata { get; init; }

}
