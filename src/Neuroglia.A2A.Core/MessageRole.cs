namespace Neuroglia.A2A;

/// <summary>
/// Enumerates all possible message roles
/// </summary>
public static class MessageRole
{

    /// <summary>
    /// Indicates an agent message
    /// </summary>
    public const string Agent = "agent";
    /// <summary>
    /// Indicates a user message
    /// </summary>
    public const string User = "user";

    /// <summary>
    /// Gets a new <see cref="IEnumerable{T}"/> containing all supported values
    /// </summary>
    /// <returns>An <see cref="IEnumerable{T}"/> containing all supported values</returns>
    public static IEnumerable<string> AsEnumerable()
    {
        yield return Agent;
        yield return User;
    }

}
