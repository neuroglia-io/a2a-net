namespace A2A;

/// <summary>
/// Enumerates the supported protocol bindings.
/// </summary>
public static class ProtocolBinding
{

    /// <summary>
    /// Indicates the GRPC protocol binding.
    /// </summary>
    public const string Grpc = "GRPC";
    /// <summary>
    /// Indicates the HTTP+JSON protocol binding.
    /// </summary>
    public const string Http = "HTTP+JSON";
    /// <summary>
    /// Indicates the JSON-RPC protocol binding.
    /// </summary>
    public const string JsonRpc = "JSONRPC";

    /// <summary>
    /// Gets all possible protocol bindings.
    /// </summary>
    public static readonly IEnumerable<string> All =
    [
        Grpc,
        Http,
        JsonRpc
    ];

}