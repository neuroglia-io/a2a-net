namespace A2A.Models;

/// <summary>
/// Represents a task-related event.
/// </summary>
[Description("Represents a task-related event.")]
[DataContract]
[KnownType(typeof(TaskArtifactUpdateEvent)), KnownType(typeof(TaskStatusUpdateEvent))]
public abstract record TaskEvent
{

    /// <summary>
    /// Gets the unique identifier of the task the event is related to.
    /// </summary>
    [Description("The unique identifier of the task the event is related to.")]
    [Required, MinLength(1)]
    [DataMember(Order = 1, Name = "taskId"), JsonPropertyOrder(1), JsonPropertyName("taskId")]
    public required string TaskId { get; init; }

}