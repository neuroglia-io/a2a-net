namespace A2A;

/// <summary>
/// Enumerates all supported API key locations.
/// </summary>
public static class ApiKeyLocation
{

    /// <summary>
    /// The API key is passed in a cookie.
    /// </summary>
    public const string Cookie = "cookie";
    /// <summary>
    /// The API key is passed in the request header.
    /// </summary>
    public const string Header = "header";
    /// <summary>
    /// The API key is passed in the query string.
    /// </summary>
    public const string Query = "query";

    /// <summary>
    /// Gets all possible API key locations.
    /// </summary>
    public static readonly IEnumerable<string> All =
    [
        Cookie,
        Header,
        Query
    ];

}
