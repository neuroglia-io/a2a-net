namespace A2A.UnitTests.Services;

internal static class OAuthFlowFactory
{

    internal static OAuthFlow CreateAuthorizationCodeFlow() => new()
    {
        AuthorizationUrl = new Uri("https://example.com/oauth2/authorize"),
        TokenUrl = new Uri("https://example.com/oauth2/token"),
        Scopes = new Dictionary<string, string>
        {
            ["read"] = "Read access",
            ["write"] = "Write access"
        }
    };

    internal static OAuthFlow CreateClientCredentialsFlow() => new()
    {
        TokenUrl = new Uri("https://example.com/oauth2/token"),
        Scopes = new Dictionary<string, string>
        {
            ["admin"] = "Admin access"
        }
    };

    internal static OAuthFlow CreateImplicitFlow() => new()
    {
        AuthorizationUrl = new Uri("https://example.com/oauth2/authorize"),
        Scopes = new Dictionary<string, string>
        {
            ["user"] = "User access"
        }
    };

    internal static OAuthFlow CreatePasswordFlow() => new()
    {
        TokenUrl = new Uri("https://example.com/oauth2/token"),
        Scopes = new Dictionary<string, string>
        {
            ["basic"] = "Basic access"
        }
    };

}