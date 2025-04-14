namespace Neuroglia.A2A.Events;

/// <summary>
/// Represents the event used to notify about an artifact update
/// </summary>
[DataContract]
public record TaskArtifactUpdateEvent
    : RpcEvent
{

    /// <summary>
    /// Gets/sets the updated artifact
    /// </summary>
    [Required]
    [DataMember(Name = "artifact", Order = 1), JsonPropertyName("artifact"), JsonPropertyOrder(1), YamlMember(Alias = "artifact", Order = 1)]
    public virtual Artifact Artifact { get; set; } = null!;

}