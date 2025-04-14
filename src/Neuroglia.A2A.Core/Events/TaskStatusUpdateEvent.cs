namespace Neuroglia.A2A.Events;

/// <summary>
/// Represents the event used to notify about a status update
/// </summary>
[DataContract]
public record TaskStatusUpdateEvent
    : Event
{

    /// <summary>
    /// Gets/sets the task's status
    /// </summary>
    [Required]
    [DataMember(Name = "status", Order = 1), JsonPropertyName("status"), JsonPropertyOrder(1), YamlMember(Alias = "status", Order = 1)]
    public virtual Models.TaskStatus Status { get; set; } = null!;

    /// <summary>
    /// Gets/sets a boolean indicating whether or not the event is the last of the stream it belongs to
    /// </summary>
    [DataMember(Name = "final", Order = 2), JsonPropertyName("final"), JsonPropertyOrder(2), YamlMember(Alias = "final", Order = 2)]
    public virtual bool Final { get; set; }

}
