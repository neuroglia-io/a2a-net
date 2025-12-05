namespace A2A;

/// <summary>
/// Enumerates the possible types of streaming responses.
/// </summary>
public static class StreamResponseType
{

    /// <summary>
    /// Indicates that the streaming response contains a task artifact update event.
    /// </summary>
    public const string ArtifactUpdate = "artifact-update";
    /// <summary>
    /// Indicates that the streaming response contains a message.
    /// </summary>
    public const string Message = "message";
    /// <summary>
    /// Indicates that the streaming response contains a task status update event.
    /// </summary>
    public const string StatusUpdate = "status-update";
    /// <summary>
    /// Indicates that the streaming response contains the current state of a task.
    /// </summary>
    public const string Task = "task";
    /// <summary>
    /// Indicates that the streaming response type is unknown.
    /// </summary>
    public const string Unknown = "unknown";

    /// <summary>
    /// Gets all possible types of streaming responses.
    /// </summary>
    public static readonly IEnumerable<string> All =
    [
        ArtifactUpdate,
        Message,
        StatusUpdate,
        Task,
        Unknown
    ];

}