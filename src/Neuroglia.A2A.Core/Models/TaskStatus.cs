namespace Neuroglia.A2A.Models;

/// <summary>
/// Represents an object used to describe the status of a task
/// </summary>
[DataContract]
public record TaskStatus
{

    /// <summary>
    /// Gets/sets the task's state
    /// </summary>
    [Required, AllowedValues(TaskState.Submitted, TaskState.Working, TaskState.InputRequired, TaskState.Completed, TaskState.Cancelled, TaskState.Failed, TaskState.Unknown)]
    [DataMember(Name = "state", Order = 1), JsonPropertyName("state"), JsonPropertyOrder(1), YamlMember(Alias = "state", Order = 1)]
    public virtual string State { get; set; } = null!;

    /// <summary>
    /// Gets/sets additional status updates, if any, for the client
    /// </summary>
    [DataMember(Name = "message", Order = 2), JsonPropertyName("message"), JsonPropertyOrder(2), YamlMember(Alias = "message", Order = 2)]
    public virtual Message? Message { get; set; }

    /// <summary>
    /// Gets/sets the task's timestamp
    /// </summary>
    [DataMember(Name = "timestamp", Order = 3), JsonPropertyName("timestamp"), JsonPropertyOrder(3), YamlMember(Alias = "timestamp", Order = 3)]
    public virtual DateTimeOffset Timestamp { get; set; }

}
