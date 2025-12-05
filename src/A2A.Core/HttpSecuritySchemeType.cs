namespace A2A;

/// <summary>
/// Enumerates all supported HTTP security scheme types.
/// </summary>
public static class HttpSecuritySchemeType
{

    /// <summary>
    /// The HTTP Basic authentication scheme.
    /// </summary>
    public const string Basic = "basic";
    /// <summary>
    /// The HTTP Bearer authentication scheme.
    /// </summary>
    public const string Bearer = "bearer";

    /// <summary>
    /// Gets all possible HTTP security scheme types.
    /// </summary>
    public static readonly IEnumerable<string> All =
    [
        Basic,
        Bearer
    ];

}