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
/// Represents the default implementation of the <see cref="IGenericSecuritySchemeBuilder"/> interface.
/// </summary>
public sealed class GenericSecuritySchemeBuilder
    : IGenericSecuritySchemeBuilder
{

    ISecuritySchemeBuilder? securitySchemeBuilder;

    /// <inheritdoc/>
    public IApiKeySecuritySchemeBuilder UseApiKey()
    {
        var builder = new ApiKeySecuritySchemeBuilder();
        securitySchemeBuilder = builder;
        return builder;
    }

    /// <inheritdoc/>
    public IHttpSecuritySchemeBuilder UseHttp()
    {
        var builder = new HttpSecuritySchemeBuilder();
        securitySchemeBuilder = builder;
        return builder;
    }

    /// <inheritdoc/>
    public IMutualTlsSecuritySchemeBuilder UseMutualTls()
    {
        var builder = new MutualTlsSecuritySchemeBuilder();
        securitySchemeBuilder = builder;
        return builder;
    }

    /// <inheritdoc/>
    public IOAuth2SecuritySchemeBuilder UseOAuth2()
    {
        var builder = new OAuth2SecuritySchemeBuilder();
        securitySchemeBuilder = builder;
        return builder;
    }

    /// <inheritdoc/>
    public IOpenIdConnectSecuritySchemeBuilder UseOpenIdConnect()
    {
        var builder = new OpenIdConnectSecuritySchemeBuilder();
        securitySchemeBuilder = builder;
        return builder;
    }

    /// <inheritdoc/>
    public SecurityScheme Build()
    {
        if (securitySchemeBuilder == null) throw new NullReferenceException($"The underlying security scheme builder must be configured");
        return securitySchemeBuilder.Build();
    }

}
