namespace Neuroglia.A2A;

/// <summary>
/// Enumerates all possible task states
/// </summary>
public static class TaskState
{

    /// <summary>
    /// Indicates that the task has been submitted and is pending executing
    /// </summary>
    public const string Submitted = "submitted";
    /// <summary>
    /// Indicates that the task is being executed
    /// </summary>
    public const string Working = "working";
    /// <summary>
    /// Indicates that the task requires input
    /// </summary>
    public const string InputRequired = "input-required";
    /// <summary>
    /// Indicates that the task ran to completion
    /// </summary>
    public const string Completed = "completed";
    /// <summary>
    /// Indicates that the task has been cancelled
    /// </summary>
    public const string Cancelled = "cancelled";
    /// <summary>
    /// Indicates that the task has failed
    /// </summary>
    public const string Failed = "failed";
    /// <summary>
    /// Indicates that the task is an unknown status
    /// </summary>
    public const string Unknown = "unknown";

    /// <summary>
    /// Gets a new <see cref="IEnumerable{T}"/> containing all supported values
    /// </summary>
    /// <returns>An <see cref="IEnumerable{T}"/> containing all supported values</returns>
    public static IEnumerable<string> AsEnumerable()
    {
        yield return Submitted;
        yield return Working;
        yield return InputRequired;
        yield return Completed;
        yield return Cancelled;
        yield return Failed;
        yield return Unknown;
    }

}
