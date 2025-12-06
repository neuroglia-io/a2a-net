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

namespace A2A.UnitTests.Services;

internal static class SecuritySchemeFactory
{

    internal static ApiKeySecurityScheme CreateApiKeySecurityScheme() => new()
    {
        Description = "API Key Security Scheme",
        Name = "apiKey",
        In = ApiKeyLocation.Header
    };

    internal static HttpSecurityScheme CreateHttpSecurityScheme() => new()
    {
        Description = "HTTP Security Scheme",
        Scheme = "bearer",
        BearerFormat = "jwt"
    };

    internal static MutualTlsSecurityScheme CreateMutualTlsSecurityScheme() => new()
    {
        Description = "Mutual TLS Security Scheme"
    };

    internal static OAuth2SecurityScheme CreateOAuth2SecurityScheme() => new()
    {
        Description = "OAuth2 Security Scheme",
        Flows = OAuthFlowsFactory.Create()
    };

    internal static OpenIdConnectSecurityScheme CreateOpenIdConnectSecurityScheme() => new()
    {
        Description = "OpenID Connect Security Scheme",
        OpenIdConnectUrl = new Uri("https://example.com/.well-known/openid-configuration")
    };

}
