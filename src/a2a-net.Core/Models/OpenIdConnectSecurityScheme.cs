﻿// Copyright © 2025-Present the a2a-net Authors
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
/// Represents an OpenID Connect security scheme as defined by the OpenAPI Specification.
/// </summary>
[Description("An OpenID Connect security scheme as defined by the OpenAPI Specification.")]
[DataContract]
public record OpenIdConnectSecurityScheme 
    : SecurityScheme
{

    /// <inheritdoc/>
    [IgnoreDataMember, JsonIgnore, YamlIgnore]
    public override string Type => SecuritySchemeType.OpenIdConnect;

    /// <summary>
    /// Gets or sets the OpenID Connect URL to discover OAuth2 configuration values.
    /// </summary>
    [Description("The OpenID Connect URL to discover OAuth2 configuration values.")]
    [Required, MinLength(1)]
    [DataMember(Name = "openIdConnectUrl", Order = 1), JsonPropertyName("openIdConnectUrl"), JsonPropertyOrder(1), YamlMember(Alias = "openIdConnectUrl", Order = 1)]
    public virtual Uri OpenIdConnectUrl { get; set; } = null!;

}