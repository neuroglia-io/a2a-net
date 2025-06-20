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
/// Defines a builder for creating an <see cref="HttpSecurityScheme"/>.
/// </summary>
public interface IHttpSecuritySchemeBuilder
    : ISecuritySchemeBuilder
{

    /// <summary>
    /// Sets the HTTP authentication scheme (e.g., "basic", "bearer").
    /// </summary>
    /// <param name="scheme">The HTTP authentication scheme to use.</param>
    /// <returns>The builder instance.</returns>
    IHttpSecuritySchemeBuilder WithScheme(string scheme);

    /// <summary>
    /// Sets the bearer format hint (e.g., "JWT"). Applies only when the scheme is "bearer".
    /// </summary>
    /// <param name="bearerFormat">The format of the bearer token.</param>
    /// <returns>The builder instance.</returns>
    IHttpSecuritySchemeBuilder WithBearerFormat(string bearerFormat);

    /// <summary>
    /// Builds and returns the configured <see cref="HttpSecurityScheme"/>.
    /// </summary>
    /// <returns>A new <see cref="HttpSecurityScheme"/>.</returns>
    new HttpSecurityScheme Build();

}