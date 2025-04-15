namespace Neuroglia.A2A.Events;

/// <summary>
/// Represents the base class for all task-related RPC events
/// </summary>
[DataContract]
public abstract record TaskEvent
    : RpcEvent
{

    /// <summary>
    /// Gets/sets the task's unique identifier
    /// </summary>
    [Required, MinLength(1)]
    [DataMember(Name = "id", Order = 0), JsonPropertyName("id"), JsonPropertyOrder(0), YamlMember(Alias = "id", Order = 0)]
    public virtual string Id { get; set; } = null!;

}
