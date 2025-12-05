namespace A2A.Models;

/// <summary>
/// Represents a request to send a message.
/// </summary>
[Description("Represents a request to send a message.")]
[DataContract]
public sealed record SendMessageRequest
{

    /// <summary>
    /// Gets the message to send.
    /// </summary>
    [Description("The message to send.")]
    [Required]
    [DataMember(Order = 1, Name = "message"), JsonPropertyOrder(1), JsonPropertyName("message")]
    public required Message Message { get; init; }

    /// <summary>
    /// Gets the request's configuration, if any.
    /// </summary>
    [Description("The request's configuration, if any.")]
    [DataMember(Order = 2, Name = "configuration"), JsonPropertyOrder(2), JsonPropertyName("configuration")]
    public SendMessageConfiguration? Configuration { get; init; }

    /// <summary>
    /// Gets a key/value mapping, if any, containing additional context or parameters for the request.
    /// </summary>
    [Description("A key/value mapping, if any, containing additional context or parameters for the request.")]
    [DataMember(Order = 3, Name = "metadata"), JsonPropertyOrder(3), JsonPropertyName("metadata")]
    public IReadOnlyDictionary<string, JsonNode>? Metadata { get; init; }

}
