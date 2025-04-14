namespace Neuroglia.A2A;

/// <summary>
/// Enumerates all supported versions of the JSON RPC specification
/// </summary>
public static class JsonRpcVersion
{

    /// <summary>
    /// Indicates JSON RPC 2.0
    /// </summary>
    public const string V2 = "2.0";

    /// <summary>
    /// Gets a new <see cref="IEnumerable{T}"/> containing all supported values
    /// </summary>
    /// <returns>An <see cref="IEnumerable{T}"/> containing all supported values</returns>
    public static IEnumerable<string> AsEnumerable()
    {
        yield return V2;
    }

}