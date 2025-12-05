namespace A2A;

/// <summary>
/// Enumerates the possible states of a task.
/// </summary>
public static class TaskState
{

    /// <summary>
    /// Indicates that the task requires authentication before it can proceed.
    /// </summary>
    public const string AuthRequired = "TASK_STATE_AUTH_REQUIRED";
    /// <summary>
    /// Indicates that the task has been cancelled.
    /// </summary>
    public const string Cancelled = "TASK_STATE_CANCELLED";
    /// <summary>
    /// Indicates that the task has been completed.
    /// </summary>
    public const string Completed = "TASK_STATE_COMPLETED";
    /// <summary>
    /// Indicates that the task has failed.
    /// </summary>
    public const string Failed = "TASK_STATE_FAILED";
    /// <summary>
    /// Indicates that the task requires additional input before it can proceed.
    /// </summary>
    public const string InputRequired = "TASK_STATE_INPUT_REQUIRED";
    /// <summary>
    /// Indicates that the task has been rejected.
    /// </summary>
    public const string Rejected = "TASK_STATE_REJECTED";
    /// <summary>
    /// Indicates that the task has been submitted and is awaiting processing.
    /// </summary>
    public const string Submitted = "TASK_STATE_SUBMITTED";
    /// <summary>
    /// Indicates that the task is in an unknown or indeterminate state.
    /// </summary>
    public const string Unspecified = "TASK_STATE_UNSPECIFIED";
    /// <summary>
    /// Indicates that the task is currently being processed.
    /// </summary>
    public const string Working = "TASK_STATE_WORKING";

    /// <summary>
    /// Gets all possible task states.
    /// </summary>
    public static readonly IEnumerable<string> All =
    [
        AuthRequired,
        Cancelled,
        Completed,
        Failed,
        InputRequired,
        Rejected,
        Submitted,
        Unspecified,
        Working
    ];

}
