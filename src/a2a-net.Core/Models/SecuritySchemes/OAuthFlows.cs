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

namespace A2A.Models.SecuritySchemes;

/// <summary>
/// Represents the OAuth2 flows available for a security scheme.
/// </summary>
[Description("Represents the OAuth2 flows available for a security scheme.")]
[DataContract]
public record OAuthFlows
{

    /// <summary>
    /// Gets or sets the configuration for the OAuth2 implicit flow, if any.
    /// </summary>
    [Description("The configuration for the OAuth2 implicit flow, if any.")]
    [DataMember(Name = "implicit", Order = 1), JsonPropertyName("implicit"), JsonPropertyOrder(1), YamlMember(Alias = "implicit", Order = 1)]
    public virtual OAuthFlow? Implicit { get; set; }

    /// <summary>
    /// Gets or sets the configuration for the OAuth2 password flow, if any.
    /// </summary>
    [Description("The configuration for the OAuth2 password flow, if any.")]
    [DataMember(Name = "password", Order = 2), JsonPropertyName("password"), JsonPropertyOrder(2), YamlMember(Alias = "password", Order = 2)]
    public virtual OAuthFlow? Password { get; set; }

    /// <summary>
    /// Gets or sets the configuration for the OAuth2 client credentials flow, if any.
    /// </summary>
    [Description("The configuration for the OAuth2 client credentials flow, if any.")]
    [DataMember(Name = "clientCredentials", Order = 3), JsonPropertyName("clientCredentials"), JsonPropertyOrder(3), YamlMember(Alias = "clientCredentials", Order = 3)]
    public virtual OAuthFlow? ClientCredentials { get; set; }

    /// <summary>
    /// Gets or sets the configuration for the OAuth2 authorization code flow, if any.
    /// </summary>
    [Description("The configuration for the OAuth2 authorization code flow, if any.")]
    [DataMember(Name = "authorizationCode", Order = 4), JsonPropertyName("authorizationCode"), JsonPropertyOrder(4), YamlMember(Alias = "authorizationCode", Order = 4)]
    public virtual OAuthFlow? AuthorizationCode { get; set; }

    /// <summary>
    /// Gets an <see cref="IEnumerable{T}"/> containing all defined flows.
    /// </summary>
    /// <returns>A new <see cref="IEnumerable{T}"/> containing all defined flows.</returns>
    public virtual IEnumerable<OAuthFlows> AsEnumerable()
    {
        if (Implicit is not null) yield return this;
        if (Password is not null) yield return this;
        if (ClientCredentials is not null) yield return this;
        if (AuthorizationCode is not null) yield return this;
    }

}
