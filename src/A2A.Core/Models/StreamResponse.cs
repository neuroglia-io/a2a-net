namespace A2A.Models;

/// <summary>
/// Represents a streaming response from an A2A service.
/// </summary>
[Description("Represents a streaming response from an A2A service.")]
[DataContract]
public sealed record StreamResponse
{

    /// <summary>
    /// Gets the task, if any, returned as part of the streaming response.
    /// </summary>
    [Description("The task, if any, returned as part of the streaming response.")]
    [DataMember(Order = 1, Name = "task"), JsonPropertyOrder(1), JsonPropertyName("task")]
    public Task? Task { get; init; }

    /// <summary>
    /// Gets the message, if any, returned as part of the streaming response.
    /// </summary>
    [Description("The message, if any, returned as part of the streaming response.")]
    [DataMember(Order = 2, Name = "message"), JsonPropertyOrder(2), JsonPropertyName("message")]
    public Message? Message { get; init; }

    /// <summary>
    /// Gets the task status update event, if any, returned as part of the streaming response.
    /// </summary>
    [Description("The task status update event, if any, returned as part of the streaming response.")]
    [DataMember(Order = 3, Name = "statusUpdate"), JsonPropertyOrder(3), JsonPropertyName("statusUpdate")]
    public TaskStatusUpdateEvent? StatusUpdate { get; init; }

    /// <summary>
    /// Gets the task artifact update event, if any, returned as part of the streaming response.
    /// </summary>
    [Description("The task artifact update event, if any, returned as part of the streaming response.")]
    [DataMember(Order = 4, Name = "artifactUpdate"), JsonPropertyOrder(4), JsonPropertyName("artifactUpdate")]
    public TaskArtifactUpdateEvent? ArtifactUpdate { get; init; }

    /// <summary>
    /// Gets the streaming response type.
    /// </summary>
    [IgnoreDataMember, JsonIgnore]
    public string Type => Task is not null ? StreamResponseType.Task : Message is not null ? StreamResponseType.Message : StatusUpdate is not null ? StreamResponseType.StatusUpdate : ArtifactUpdate is not null ? StreamResponseType.ArtifactUpdate : StreamResponseType.Unknown;

}