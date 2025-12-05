namespace A2A.Models;

/// <summary>
/// Represents the status of a task.
/// </summary>
[Description("Represents the status of a task.")]
[DataContract]
public sealed record TaskStatus
{

    /// <summary>
    /// Gets the current state of the task.
    /// </summary>
    [Description("The current state of the task.")]
    [Required, AllowedValues(TaskState.AuthRequired, TaskState.Cancelled, TaskState.Completed, TaskState.Failed, TaskState.InputRequired, TaskState.Rejected, TaskState.Submitted, TaskState.Unspecified, TaskState.Working)]
    [DataMember(Order = 1, Name = "state"), JsonPropertyOrder(1), JsonPropertyName("state")]
    public string State { get; init; } = TaskState.Unspecified;

    /// <summary>
    /// Gets the date and time when the task's status was recorded.
    /// </summary>
    [Description("The date and time when the task's status was recorded.")]
    [DataMember(Order = 2, Name = "timestamp"), JsonPropertyOrder(2), JsonPropertyName("timestamp")]
    public DateTime? Timestamp { get; init; }

    /// <summary>
    /// Gets the message, if any, associated with the task's status.
    /// </summary>
    [Description("The message, if any, associated with the task's status.")]
    [DataMember(Order = 3, Name = "message"), JsonPropertyOrder(3), JsonPropertyName("message")]
    public Message? Message { get; init; }

}