namespace Neuroglia.A2A.Models;

/// <summary>
/// Represents the parameters of a task query
/// </summary>
[DataContract]
public record TaskQueryParameters
{

    /// <summary>
    /// Gets/sets the id of the task to query
    /// </summary>
    [Required, MinLength(1)]
    [DataMember(Name = "id", Order = 1), JsonPropertyName("id"), JsonPropertyOrder(1), YamlMember(Alias = "id", Order = 1)]
    public virtual string Id { get; set; } = null!;

    /// <summary>
    /// Gets/sets the length, if any, of the task message history to get
    /// </summary>
    [DataMember(Name = "historyLength", Order = 2), JsonPropertyName("historyLength"), JsonPropertyOrder(2), YamlMember(Alias = "historyLength", Order = 2)]
    public virtual uint? HistoryLength { get; set; } = null!;

    /// <summary>
    /// Gets/sets a key/value mapping that contains the task's additional properties, if any
    /// </summary>
    [DataMember(Name = "metadata", Order = 3), JsonPropertyName("metadata"), JsonPropertyOrder(3), YamlMember(Alias = "metadata", Order = 3)]
    public virtual EquatableDictionary<string, object>? Metadata { get; set; }

}
