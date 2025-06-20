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
/// Defines a builder for creating an <see cref="OAuth2SecurityScheme"/> with one or more OAuth2 flows.
/// </summary>
public interface IOAuth2SecuritySchemeBuilder
    : ISecuritySchemeBuilder
{

    /// <summary>
    /// Configures the OAuth2 scheme with an implicit flow.
    /// </summary>
    /// <param name="flow">The configuration for the implicit flow.</param>
    /// <returns>The configured <see cref="IOAuth2SecuritySchemeBuilder"/>.</returns>
    IOAuth2SecuritySchemeBuilder WithImplicitFlow(OAuthFlow flow);

    /// <summary>
    /// Configures the OAuth2 scheme with an implicit flow using a nested builder.
    /// </summary>
    /// <param name="setup">An action that configures the implicit flow via an <see cref="IOAuthFlowBuilder"/>.</param>
    /// <returns>The configured <see cref="IOAuth2SecuritySchemeBuilder"/>.</returns>
    IOAuth2SecuritySchemeBuilder WithImplicitFlow(Action<IOAuthFlowBuilder> setup);

    /// <summary>
    /// Configures the OAuth2 scheme with a password flow.
    /// </summary>
    /// <param name="flow">The configuration for the password flow.</param>
    /// <returns>The configured <see cref="IOAuth2SecuritySchemeBuilder"/>.</returns>
    IOAuth2SecuritySchemeBuilder WithPasswordFlow(OAuthFlow flow);

    /// <summary>
    /// Configures the OAuth2 scheme with a password flow using a nested builder.
    /// </summary>
    /// <param name="setup">An action that configures the password flow via an <see cref="IOAuthFlowBuilder"/>.</param>
    /// <returns>The configured <see cref="IOAuth2SecuritySchemeBuilder"/>.</returns>
    IOAuth2SecuritySchemeBuilder WithPasswordFlow(Action<IOAuthFlowBuilder> setup);

    /// <summary>
    /// Configures the OAuth2 scheme with a client credentials flow.
    /// </summary>
    /// <param name="flow">The configuration for the client credentials flow.</param>
    /// <returns>The configured <see cref="IOAuth2SecuritySchemeBuilder"/>.</returns>
    IOAuth2SecuritySchemeBuilder WithClientCredentialsFlow(OAuthFlow flow);

    /// <summary>
    /// Configures the OAuth2 scheme with a client credentials flow using a nested builder.
    /// </summary>
    /// <param name="setup">An action that configures the flow via an <see cref="IOAuthFlowBuilder"/>.</param>
    /// <returns>The configured <see cref="IOAuth2SecuritySchemeBuilder"/>.</returns>
    IOAuth2SecuritySchemeBuilder WithClientCredentialsFlow(Action<IOAuthFlowBuilder> setup);

    /// <summary>
    /// Configures the OAuth2 scheme with an authorization code flow.
    /// </summary>
    /// <param name="flow">The configuration for the authorization code flow.</param>
    /// <returns>The configured <see cref="IOAuth2SecuritySchemeBuilder"/>.</returns>
    IOAuth2SecuritySchemeBuilder WithAuthorizationCodeFlow(OAuthFlow flow);

    /// <summary>
    /// Configures the OAuth2 scheme with an authorization code flow using a nested builder.
    /// </summary>
    /// <param name="setup">An action that configures the flow via an <see cref="IOAuthFlowBuilder"/>.</param>
    /// <returns>The configured <see cref="IOAuth2SecuritySchemeBuilder"/>.</returns>
    IOAuth2SecuritySchemeBuilder WithAuthorizationCodeFlow(Action<IOAuthFlowBuilder> setup);

    /// <summary>
    /// Builds and returns the configured <see cref="OAuth2SecurityScheme"/>.
    /// </summary>
    /// <returns>The constructed <see cref="OAuth2SecurityScheme"/>.</returns>
    new OAuth2SecurityScheme Build();

}
