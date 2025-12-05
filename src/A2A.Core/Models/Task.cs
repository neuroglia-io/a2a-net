namespace A2A.Models;

/// <summary>
/// Represents the fundamental unit of work in A2A.
/// </summary>
[Description("Represents the fundamental unit of work in A2A.")]
[DataContract]
public sealed record Task
    : Response
{

    /// <summary>
    /// Gets the task's unique identifier.
    /// </summary>
    [Description("The task's unique identifier.")]
    [Required, MinLength(1)]
    [DataMember(Order = 1, Name = "id"), JsonPropertyOrder(1), JsonPropertyName("id")]
    public string Id { get; init; } = Guid.NewGuid().ToString("N");

    /// <summary>
    /// Gets the unique identifier of the context to which the task belongs.
    /// </summary>
    [Description("The unique identifier of the context to which the task belongs.")]
    [Required, MinLength(1)]
    [DataMember(Order = 2, Name = "contextId"), JsonPropertyOrder(2), JsonPropertyName("contextId")]
    public required string ContextId { get; init; }

    /// <summary>
    /// Gets the task's status.
    /// </summary>
    [Description("The task's status.")]
    [Required]
    [DataMember(Order = 3, Name = "status"), JsonPropertyOrder(3), JsonPropertyName("status")]
    public TaskStatus Status { get; set; } = new();

    /// <summary>
    /// Gets a collection containing the task's output artifacts, if any.
    /// </summary>
    [Description("A collection containing the task's output artifacts, if any.")]
    [DataMember(Order = 4, Name = "artifacts"), JsonPropertyOrder(4), JsonPropertyName("artifacts")]
    public ICollection<Artifact>? Artifacts { get; set; }

    /// <summary>
    /// Gets a collection containing the task's message history, if any.
    /// </summary>
    [Description("A collection containing the task's message history, if any.")]
    [DataMember(Order = 5, Name = "history"), JsonPropertyOrder(5), JsonPropertyName("history")]
    public ICollection<Message>? History { get; set; }

    /// <summary>
    /// Gets a key/value mapping, if any, containing metadata associated with the task.
    /// </summary>
    [Description("A key/value mapping, if any, containing metadata associated with the task.")]
    [DataMember(Order = 6, Name = "metadata"), JsonPropertyOrder(6), JsonPropertyName("metadata")]
    public IDictionary<string, JsonNode>? Metadata { get; init; }

}
