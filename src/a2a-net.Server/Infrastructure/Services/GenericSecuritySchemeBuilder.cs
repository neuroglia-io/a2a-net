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
/// Represents the default implementation of the <see cref="IGenericSecuritySchemeBuilder"/> interface.
/// </summary>
public class GenericSecuritySchemeBuilder
    : IGenericSecuritySchemeBuilder
{

    /// <summary>
    /// Gets the underlying service used to build and configure the security scheme.
    /// </summary>
    protected ISecuritySchemeBuilder? SecuritySchemeBuilder { get; set; }

    /// <inheritdoc/>
    public virtual IApiKeySecuritySchemeBuilder UseApiKey()
    {
        var builder = new ApiKeySecuritySchemeBuilder();
        SecuritySchemeBuilder = builder;
        return builder;
    }

    /// <inheritdoc/>
    public virtual IHttpSecuritySchemeBuilder UseHttp()
    {
        var builder = new HttpSecuritySchemeBuilder();
        SecuritySchemeBuilder = builder;
        return builder;
    }

    /// <inheritdoc/>
    public virtual IOAuth2SecuritySchemeBuilder UseOAuth2()
    {
        var builder = new OAuth2SecuritySchemeBuilder();
        SecuritySchemeBuilder = builder;
        return builder;
    }

    /// <inheritdoc/>
    public virtual IOpenIdConnectSecuritySchemeBuilder UseOpenIdConnect()
    {
        var builder = new OpenIdConnectSecuritySchemeBuilder();
        SecuritySchemeBuilder = builder;
        return builder;
    }

    /// <inheritdoc/>
    public virtual SecurityScheme Build()
    {
        if (SecuritySchemeBuilder == null) throw new NullReferenceException($"The underlying security scheme builder must be configured");
        return SecuritySchemeBuilder.Build();
    }

}