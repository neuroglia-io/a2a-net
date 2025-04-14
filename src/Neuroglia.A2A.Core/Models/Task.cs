namespace Neuroglia.A2A.Models;

/// <summary>
/// Represents a task
/// </summary>
[DataContract]
public record Task
{

    /// <summary>
    /// Gets/sets the task's unique identifier
    /// </summary>
    [Required, MinLength(1)]
    [DataMember(Name = "id", Order = 1), JsonPropertyName("id"), JsonPropertyOrder(1), YamlMember(Alias = "id", Order = 1)]
    public virtual string Id { get; set; } = null!;

    /// <summary>
    /// Gets/sets the unique identifier of the session holding the task
    /// </summary>
    [Required, MinLength(1)]
    [DataMember(Name = "sessionId", Order = 2), JsonPropertyName("sessionId"), JsonPropertyOrder(2), YamlMember(Alias = "sessionId", Order = 2)]
    public virtual string SessionId { get; set; } = null!;

    /// <summary>
    /// Gets/sets the task's status
    /// </summary>
    [Required]
    [DataMember(Name = "status", Order = 3), JsonPropertyName("status"), JsonPropertyOrder(3), YamlMember(Alias = "status", Order = 3)]
    public virtual TaskStatus Status { get; set; } = null!;

    /// <summary>
    /// Gets/sets the history of all the task's messages
    /// </summary>
    [DataMember(Name = "history", Order = 4), JsonPropertyName("history"), JsonPropertyOrder(4), YamlMember(Alias = "history", Order = 4)]
    public virtual EquatableList<Message>? History { get; set; }

    /// <summary>
    /// Gets/sets a key/value mapping that contains the task's additional properties, if any
    /// </summary>
    [DataMember(Name = "metadata", Order = 5), JsonPropertyName("metadata"), JsonPropertyOrder(5), YamlMember(Alias = "metadata", Order = 5)]
    public virtual EquatableDictionary<string, object>? Metadata { get; set; }

}
