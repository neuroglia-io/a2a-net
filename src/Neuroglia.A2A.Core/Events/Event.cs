namespace Neuroglia.A2A.Events;

/// <summary>
/// Represents the base class for all A2A events
/// </summary>
[DataContract]
public abstract record Event
{

    /// <summary>
    /// Gets/sets the event's unique identifier
    /// </summary>
    [Required, MinLength(1)]
    [DataMember(Name = "id", Order = 0), JsonPropertyName("id"), JsonPropertyOrder(0), YamlMember(Alias = "id", Order = 0)]
    public virtual string Id { get; set; } = null!;

    /// <summary>
    /// Gets/sets a key/value mapping that contains the event's additional properties, if any
    /// </summary>
    [DataMember(Name = "metadata", Order = 99), JsonPropertyName("metadata"), JsonPropertyOrder(99), YamlMember(Alias = "metadata", Order = 99)]
    public virtual EquatableDictionary<string, object>? Metadata { get; set; }

}
