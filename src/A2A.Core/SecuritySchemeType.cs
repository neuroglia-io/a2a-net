using static System.Net.WebRequestMethods;

namespace A2A;

/// <summary>
/// Enumerates all supported security scheme types.
/// </summary>
public static class SecuritySchemeType
{

    /// <summary>
    /// The API key security scheme type.
    /// </summary>
    public const string ApiKey = "apiKey";
    /// <summary>
    /// The HTTP authentication security scheme type.
    /// </summary>
    public const string Http = "http";
    /// <summary>
    /// The Mutual TLS security scheme type.
    /// </summary>
    public const string MutualTls = "mutualTLS";
    /// <summary>
    /// The OAuth 2.0 security scheme type.
    /// </summary>
    public const string OAuth2 = "oauth2";
    /// <summary>
    /// The OpenID Connect security scheme type.
    /// </summary>
    public const string OpenIdConnect = "openIdConnect";

    /// <summary>
    /// Gets all possible security scheme types.
    /// </summary>
    public static readonly IEnumerable<string> All =
    [
        ApiKey,
        Http,
        MutualTls,
        OAuth2,
        OpenIdConnect
    ];

}
