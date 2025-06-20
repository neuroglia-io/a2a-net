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

namespace A2A.Server.Infrastructure.Services;

/// <summary>
/// Defines the fundamentals of a service used to build security schemes.
/// </summary>
public interface IGenericSecuritySchemeBuilder
{

    /// <summary>
    /// Uses the API key security scheme.
    /// </summary>
    /// <returns>A new <see cref="IApiKeySecuritySchemeBuilder"/>.</returns>
    IApiKeySecuritySchemeBuilder UseApiKey();

    /// <summary>
    /// Uses the HTTP security scheme.
    /// </summary>
    /// <returns>A new <see cref="IHttpSecuritySchemeBuilder"/>.</returns>
    IHttpSecuritySchemeBuilder UseHttp();

    /// <summary>
    /// Uses the OAUTH2 security scheme.
    /// </summary>
    /// <returns>A new <see cref="IOAuth2SecuritySchemeBuilder"/>.</returns>
    IOAuth2SecuritySchemeBuilder UseOAuth2();

    /// <summary>
    /// Uses the OpenIdConnect security scheme.
    /// </summary>
    /// <returns>A new <see cref="IOpenIdConnectSecuritySchemeBuilder"/>.</returns>
    IOpenIdConnectSecuritySchemeBuilder UseOpenIdConnect();

    /// <summary>
    /// Builds the configured security scheme.
    /// </summary>
    /// <returns>A new <see cref="SecurityScheme"/>.</returns>
    SecurityScheme Build();

}
