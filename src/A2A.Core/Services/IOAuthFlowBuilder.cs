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
/// Defines a builder for configuring an <see cref="OAuthFlow"/>.
/// </summary>
public interface IOAuthFlowBuilder
{

    /// <summary>
    /// Sets the authorization URL for the OAuth2 flow. Required for implicit and authorization code flows.
    /// </summary>
    /// <param name="url">The authorization endpoint URL.</param>
    /// <returns>The builder instance.</returns>
    IOAuthFlowBuilder WithAuthorizationUrl(Uri url);

    /// <summary>
    /// Sets the token URL for the OAuth2 flow. Required for password, client credentials, and authorization code flows.
    /// </summary>
    /// <param name="url">The token endpoint URL.</param>
    /// <returns>The builder instance.</returns>
    IOAuthFlowBuilder WithTokenUrl(Uri url);

    /// <summary>
    /// Sets the refresh URL for the OAuth2 flow.
    /// </summary>
    /// <param name="url">The refresh token endpoint URL.</param>
    /// <returns>The builder instance.</returns>
    IOAuthFlowBuilder WithRefreshUrl(Uri url);

    /// <summary>
    /// Adds a scope to the OAuth2 flow.
    /// </summary>
    /// <param name="name">The name of the scope.</param>
    /// <param name="description">A description of what the scope enables.</param>
    /// <returns>The builder instance.</returns>
    IOAuthFlowBuilder AddScope(string name, string? description = null);

    /// <summary>
    /// Builds and returns the configured <see cref="OAuthFlow"/>.
    /// </summary>
    /// <returns>A new <see cref="OAuthFlow"/>.</returns>
    OAuthFlow Build();

}