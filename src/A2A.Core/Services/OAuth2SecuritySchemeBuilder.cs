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
/// Represents the default implementation of the <see cref="IOAuth2SecuritySchemeBuilder"/> interface.
/// </summary>
public sealed class OAuth2SecuritySchemeBuilder
    : IOAuth2SecuritySchemeBuilder
{

    readonly OAuth2SecurityScheme securityScheme = new();

    /// <inheritdoc/>
    public IOAuth2SecuritySchemeBuilder WithAuthorizationCodeFlow(OAuthFlow flow)
    {
        ArgumentNullException.ThrowIfNull(flow);
        securityScheme.Flows ??= new();
        securityScheme.Flows.AuthorizationCode = flow;
        return this;
    }

    /// <inheritdoc/>
    public IOAuth2SecuritySchemeBuilder WithAuthorizationCodeFlow(Action<IOAuthFlowBuilder> setup)
    {
        ArgumentNullException.ThrowIfNull(setup);
        var builder = new OAuthFlowBuilder();
        setup(builder);
        return WithAuthorizationCodeFlow(builder.Build());
    }

    /// <inheritdoc/>
    public IOAuth2SecuritySchemeBuilder WithClientCredentialsFlow(OAuthFlow flow)
    {
        ArgumentNullException.ThrowIfNull(flow);
        securityScheme.Flows ??= new();
        securityScheme.Flows.ClientCredentials = flow;
        return this;
    }

    /// <inheritdoc/>
    public IOAuth2SecuritySchemeBuilder WithClientCredentialsFlow(Action<IOAuthFlowBuilder> setup)
    {
        ArgumentNullException.ThrowIfNull(setup);
        var builder = new OAuthFlowBuilder();
        setup(builder);
        return WithClientCredentialsFlow(builder.Build());
    }

    /// <inheritdoc/>
    public IOAuth2SecuritySchemeBuilder WithImplicitFlow(OAuthFlow flow)
    {
        ArgumentNullException.ThrowIfNull(flow);
        securityScheme.Flows ??= new();
        securityScheme.Flows.Implicit = flow;
        return this;
    }

    /// <inheritdoc/>
    public IOAuth2SecuritySchemeBuilder WithImplicitFlow(Action<IOAuthFlowBuilder> setup)
    {
        ArgumentNullException.ThrowIfNull(setup);
        var builder = new OAuthFlowBuilder();
        setup(builder);
        return WithImplicitFlow(builder.Build());
    }

    /// <inheritdoc/>
    public IOAuth2SecuritySchemeBuilder WithPasswordFlow(OAuthFlow flow)
    {
        ArgumentNullException.ThrowIfNull(flow);
        securityScheme.Flows ??= new();
        securityScheme.Flows.Password = flow;
        return this;
    }

    /// <inheritdoc/>
    public IOAuth2SecuritySchemeBuilder WithPasswordFlow(Action<IOAuthFlowBuilder> setup)
    {
        ArgumentNullException.ThrowIfNull(setup);
        var builder = new OAuthFlowBuilder();
        setup(builder);
        return WithPasswordFlow(builder.Build());
    }

    /// <inheritdoc/>
    public OAuth2SecurityScheme Build() => securityScheme;

    SecurityScheme ISecuritySchemeBuilder.Build() => Build();

}