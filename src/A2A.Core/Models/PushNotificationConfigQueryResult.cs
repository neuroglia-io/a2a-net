namespace A2A.Models;

/// <summary>
/// Represents the result of a push notification configuration query.
/// </summary>
[Description("Represents the result of a push notification configuration query.")]
[DataContract]
public sealed record PushNotificationConfigQueryResult
{

    /// <summary>
    /// Gets a collection containing the push notification configurations matching the specified criteria.
    /// </summary>
    [Description("A collection containing the push notification configurations matching the specified criteria.")]
    [Required]
    [DataMember(Order = 1, Name = "configs"), JsonPropertyOrder(1), JsonPropertyName("configs")]
    public required IReadOnlyList<PushNotificationConfig> Configs { get; init; }

    /// <summary>
    /// Gets the token, if any, used to retrieve the next page of results.
    /// </summary>
    [Description("The token, if any, used to retrieve the next page of results.")]
    [DataMember(Order = 2, Name = "nextPageToken"), JsonPropertyOrder(2), JsonPropertyName("nextPageToken")]
    public string? NextPageToken { get; init; }

}