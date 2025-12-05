namespace A2A;

/// <summary>
/// Enumerates the possible roles for a message sender.
/// </summary>
public static class Role
{

    /// <summary>
    /// Indicates the message was sent by an agent. In other words, the message has been sent by the server to the client.
    /// </summary>
    public const string Agent = "ROLE_AGENT";
    /// <summary>
    /// Indicates the message role is unspecified.
    /// </summary>
    public const string Unspecified = "ROLE_UNSPECIFIED";
    /// <summary>
    /// Indicates the message was sent by a user. In other words, the message has been sent by the client to the server.
    /// </summary>
    public const string User = "ROLE_USER";

    /// <summary>
    /// Gets all possible roles.
    /// </summary>
    public static readonly IEnumerable<string> All =
    [
        Agent,
        Unspecified,
        User
    ];

}
