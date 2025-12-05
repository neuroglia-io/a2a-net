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
