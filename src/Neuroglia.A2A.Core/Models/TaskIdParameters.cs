namespace Neuroglia.A2A.Models;

/// <summary>
/// Represents the parameters of a task query
/// </summary>
[DataContract]
public record TaskIdParameters
{

    /// <summary>
    /// Initializes a new <see cref="TaskIdParameters"/>
    /// </summary>
    public TaskIdParameters() { }

    /// <summary>
    /// Initializes a new <see cref="TaskIdParameters"/>
    /// </summary>
    /// <param name="id">The task's id</param>
    /// <param name="metadata">A key/value mapping that contains the task's additional properties, if any</param>
    public TaskIdParameters(string id, IDictionary<string, object>? metadata = null)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(id);
        Id = id;
        Metadata = metadata == null ? null : new(metadata);
    }

    /// <summary>
    /// Gets/sets the task's id
    /// </summary>
    [Required, MinLength(1)]
    [DataMember(Name = "id", Order = 1), JsonPropertyName("id"), JsonPropertyOrder(1), YamlMember(Alias = "id", Order = 1)]
    public virtual string Id { get; set; } = null!;

    /// <summary>
    /// Gets/sets a key/value mapping that contains the task's additional properties, if any
    /// </summary>
    [DataMember(Name = "metadata", Order = 2), JsonPropertyName("metadata"), JsonPropertyOrder(2), YamlMember(Alias = "metadata", Order = 2)]
    public virtual EquatableDictionary<string, object>? Metadata { get; set; }

}