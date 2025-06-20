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
/// Defines the fundamentals of a service used to build API key security schemes.
/// </summary>
public interface IApiKeySecuritySchemeBuilder
    : ISecuritySchemeBuilder
{

    /// <summary>
    /// Sets the name of the header, query parameter, or cookie to be used for the API key.
    /// </summary>
    /// <param name="name">The name of the API key parameter.</param>
    /// <returns>The builder instance.</returns>
    IApiKeySecuritySchemeBuilder WithName(string name);

    /// <summary>
    /// Sets the location of the API key (e.g., header, query, or cookie).
    /// </summary>
    /// <param name="location">The location of the API key.</param>
    /// <returns>The builder instance.</returns>
    IApiKeySecuritySchemeBuilder WithLocation(string location);

    /// <summary>
    /// Builds the configured API key security scheme.
    /// </summary>
    /// <returns>A new <see cref="ApiKeySecurityScheme"/>.</returns>
    new ApiKeySecurityScheme Build();

}