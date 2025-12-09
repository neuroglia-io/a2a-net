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
/// Represents an agent's signature.
/// </summary>
[Description("Represents an agent's signature.")]
[DataContract]
public sealed record AgentCardSignature
{

    /// <summary>
    /// Gets the protected JWS header for the signature. It always is a base64url-encoded JSON object.
    /// </summary>
    [Description("The protected JWS header for the signature. It always is a base64url-encoded JSON object.")]
    [Required, MinLength(1)]
    [DataMember(Order = 1, Name = "protected"), JsonPropertyOrder(1), JsonPropertyName("protected")]
    public required string Protected { get; init; }

    /// <summary>
    /// Gets the computed signature value. It always is a base64url-encoded string.
    /// </summary>
    [Description("The computed signature value. It always is a base64url-encoded string.")]
    [Required, MinLength(1)]
    [DataMember(Order = 2, Name = "signature"), JsonPropertyOrder(2), JsonPropertyName("signature")]
    public required string Signature { get; init; }

    /// <summary>
    /// Gets a key/value mapping, if any, containing the unprotected JWS header values.
    /// </summary>
    [Description("A key/value mapping, if any, containing the unprotected JWS header values.")]
    [DataMember(Order = 3, Name = "header"), JsonPropertyOrder(3), JsonPropertyName("header")]
    public IReadOnlyDictionary<string, JsonNode>? Header { get; set; }

}
