using Neuroglia.A2A.Serialization.Json;

namespace Neuroglia.A2A;

/// <summary>
/// Represents the base class for all A2A events
/// </summary>
[DataContract]
[JsonConverter(typeof(RpcEventJsonConverter))]
public abstract record RpcEvent
{

    /// <summary>
    /// Gets/sets a key/value mapping that contains the event's additional properties, if any
    /// </summary>
    [DataMember(Name = "metadata", Order = 99), JsonPropertyName("metadata"), JsonPropertyOrder(99), YamlMember(Alias = "metadata", Order = 99)]
    public virtual EquatableDictionary<string, object>? Metadata { get; set; }

}
