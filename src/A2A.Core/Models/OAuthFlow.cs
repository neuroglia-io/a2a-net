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
/// Represents the configuration of an OAUTH2 flow.
/// </summary>
[Description("Represents the configuration of an OAUTH2 flow.")]
[DataContract]
public sealed record OAuthFlow
{

    /// <summary>
    /// Gets or sets the authorization URL to be used for this flow.<para></para>
    /// Required for flows: implicit, authorizationCode.
    /// </summary>
    [Description("The authorization URL to be used for this flow. Required for flows: implicit, authorizationCode.")]
    [DataMember(Name = "authorizationUrl", Order = 1), JsonPropertyOrder(1), JsonPropertyName("authorizationUrl")]
    public Uri? AuthorizationUrl { get; set; }

    /// <summary>
    /// Gets or sets the token URL to be used for this flow.<para></para>
    /// Required for flows: password, clientCredentials, authorizationCode.
    /// </summary>
    [Description("The token URL to be used for this flow. Required for flows: password, clientCredentials, authorizationCode.")]
    [DataMember(Name = "tokenUrl", Order = 2), JsonPropertyOrder(2), JsonPropertyName("tokenUrl")]
    public Uri? TokenUrl { get; set; }

    /// <summary>
    /// Gets or sets the URL to be used for obtaining refresh tokens.
    /// </summary>
    [Description("The URL to be used for obtaining refresh tokens.")]
    [DataMember(Name = "refreshUrl", Order = 3), JsonPropertyOrder(3), JsonPropertyName("refreshUrl")]
    public Uri? RefreshUrl { get; set; }

    /// <summary>
    /// Gets or sets the available scopes for the OAuth2 security scheme.
    /// </summary>
    [Description("The available scopes for the OAuth2 security scheme.")]
    [Required]
    [DataMember(Name = "scopes", Order = 4), JsonPropertyOrder(4), JsonPropertyName("scopes")]
    public IDictionary<string, string> Scopes { get; set; } = null!;

}