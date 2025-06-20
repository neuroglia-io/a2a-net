// Copyright � 2025-Present the a2a-net Authors
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
[DataContract]
public record OAuthFlow
{

    /// <summary>
    /// Gets or sets the authorization URL to be used for this flow.<para></para>
    /// Required for flows: implicit, authorizationCode.
    /// </summary>
    [DataMember(Name = "authorizationUrl", Order = 1), JsonPropertyName("authorizationUrl"), JsonPropertyOrder(1), YamlMember(Alias = "authorizationUrl", Order = 1)]
    public virtual Uri? AuthorizationUrl { get; set; }

    /// <summary>
    /// Gets or sets the token URL to be used for this flow.<para></para>
    /// Required for flows: password, clientCredentials, authorizationCode.
    /// </summary>
    [DataMember(Name = "tokenUrl", Order = 2), JsonPropertyName("tokenUrl"), JsonPropertyOrder(2), YamlMember(Alias = "tokenUrl", Order = 2)]
    public virtual Uri? TokenUrl { get; set; }

    /// <summary>
    /// Gets or sets the URL to be used for obtaining refresh tokens.
    /// </summary>
    [DataMember(Name = "refreshUrl", Order = 3), JsonPropertyName("refreshUrl"), JsonPropertyOrder(3), YamlMember(Alias = "refreshUrl", Order = 3)]
    public virtual Uri? RefreshUrl { get; set; }

    /// <summary>
    /// Gets or sets the available scopes for the OAuth2 security scheme.
    /// </summary>
    [Required]
    [DataMember(Name = "scopes", Order = 4), JsonPropertyName("scopes"), JsonPropertyOrder(4), YamlMember(Alias = "scopes", Order = 4)]
    public virtual Dictionary<string, string> Scopes { get; set; } = [];

}