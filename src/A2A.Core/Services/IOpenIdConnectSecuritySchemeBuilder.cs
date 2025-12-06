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

namespace A2A.Services;

/// <summary>
/// Defines a builder for creating an <see cref="OpenIdConnectSecurityScheme"/>.
/// </summary>
public interface IOpenIdConnectSecuritySchemeBuilder
    : ISecuritySchemeBuilder
{

    /// <summary>
    /// Sets the OpenID Connect discovery URL.
    /// </summary>
    /// <param name="url">The OpenID Connect discovery endpoint URI.</param>
    /// <returns>The builder instance.</returns>
    IOpenIdConnectSecuritySchemeBuilder WithUrl(Uri url);

    /// <summary>
    /// Builds and returns the configured <see cref="OpenIdConnectSecurityScheme"/>.
    /// </summary>
    /// <returns>A new <see cref="OpenIdConnectSecurityScheme"/>.</returns>
    new OpenIdConnectSecurityScheme Build();

}