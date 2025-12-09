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
/// Represents a security scheme.
/// </summary>
[Description("Represents a security scheme.")]
[JsonPolymorphic(TypeDiscriminatorPropertyName = "type")]
[JsonDerivedType(typeof(ApiKeySecurityScheme), SecuritySchemeType.ApiKey)]
[JsonDerivedType(typeof(HttpSecurityScheme), SecuritySchemeType.Http)]
[JsonDerivedType(typeof(MutualTlsSecurityScheme), SecuritySchemeType.MutualTls)]
[JsonDerivedType(typeof(OAuth2SecurityScheme), SecuritySchemeType.OAuth2)]
[JsonDerivedType(typeof(OpenIdConnectSecurityScheme), SecuritySchemeType.OpenIdConnect)]
[DataContract]
[KnownType(typeof(ApiKeySecurityScheme)), KnownType(typeof(HttpSecurityScheme)), KnownType(typeof(MutualTlsSecurityScheme)), KnownType(typeof(OAuth2SecurityScheme)), KnownType(typeof(OpenIdConnectSecurityScheme))]
public abstract record SecurityScheme
{

    /// <summary>
    /// Gets the security scheme's type.
    /// </summary>
    [IgnoreDataMember, JsonIgnore]
    public abstract string Type { get; }

    /// <summary>
    /// Gets the security scheme's description, if any.
    /// </summary>
    [Description("The security scheme's description, if any.")]
    [DataMember(Order = 0, Name = "description"), JsonPropertyOrder(0), JsonPropertyName("description")]
    public string? Description { get; init; }

}
