namespace Neuroglia.A2A.Server.Infrastructure;

/// <summary>
/// Enumerates all supported types of contents returned by an agent as part of its response to a task's execution
/// </summary>
public static class AgentResponseContentType
{

    /// <summary>
    /// Indicates an artifact content
    /// </summary>
    public const string Artifact = "artifact";
    /// <summary>
    /// Indicates a message content
    /// </summary>
    public const string Message = "message";

    /// <summary>
    /// Gets a new <see cref="IEnumerable{T}"/> containing all supported values
    /// </summary>
    /// <returns>An <see cref="IEnumerable{T}"/> containing all supported values</returns>
    public static IEnumerable<string> AsEnumerable()
    {
        yield return Artifact;
        yield return Message;
    }

}