namespace Neuroglia.A2A.Models;

/// <summary>
/// Represents the parameters of a task query
/// </summary>
[DataContract]
public record TaskQueryParameters
{

    /// <summary>
    /// Gets/sets the length, if any, of the task message history to get
    /// </summary>
    [DataMember(Name = "historyLength", Order = 1), JsonPropertyName("historyLength"), JsonPropertyOrder(1), YamlMember(Alias = "historyLength", Order = 1)]
    public virtual uint? HistoryLength { get; set; } = null!;

    /// <summary>
    /// Gets/sets a key/value mapping that contains the task's additional properties, if any
    /// </summary>
    [DataMember(Name = "metadata", Order = 2), JsonPropertyName("metadata"), JsonPropertyOrder(2), YamlMember(Alias = "metadata", Order = 2)]
    public virtual EquatableDictionary<string, object>? Metadata { get; set; }

}
