namespace A2A.Models;

/// <summary>
/// Represents a task status update event.
/// </summary>
[Description("Represents a task status update event.")]
[DataContract]
public sealed record TaskStatusUpdateEvent
    : TaskEvent
{

    /// <summary>
    /// Gets the unique identifier of the context associated with the task.
    /// </summary>
    [Description("The unique identifier of the context associated with the task.")]
    [Required, MinLength(1)]
    [DataMember(Order = 2, Name = "contextId"), JsonPropertyOrder(2), JsonPropertyName("contextId")]
    public required string ContextId { get; init; }

    /// <summary>
    /// Gets the new status of the task.
    /// </summary>
    [Description("The new status of the task.")]
    [Required]
    [DataMember(Order = 3, Name = "status"), JsonPropertyOrder(3), JsonPropertyName("status")]
    public required TaskStatus Status { get; init; }

    /// <summary>
    /// Gets a boolean indicating whether the task has reached a final state.
    /// </summary>
    [Description("A boolean indicating whether the task has reached a final state.")]
    [DataMember(Order = 4, Name = "final"), JsonPropertyOrder(4), JsonPropertyName("final")]
    public bool Final { get; init; }

    /// <summary>
    /// Gets a key/value mapping, if any, containing additional metadata about the status update.
    /// </summary>
    [Description("A key/value mapping, if any, containing additional metadata about the status update.")]
    [DataMember(Order = 5, Name = "metadata"), JsonPropertyOrder(5), JsonPropertyName("metadata")]
    public IReadOnlyDictionary<string, JsonNode>? Metadata { get; init; }

}
