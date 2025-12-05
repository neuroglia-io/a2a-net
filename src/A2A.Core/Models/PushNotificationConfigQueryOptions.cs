namespace A2A.Models;

/// <summary>
/// Represents the options used to configure a push notification configuration query.
/// </summary>
[Description("Represents the options used to configure a push notification configuration query.")]
[DataContract]
public sealed record PushNotificationConfigQueryOptions
{

    /// <summary>
    /// Gets the unique identifier, if any, of the task the push notifications to get belong to.
    /// </summary>
    [Description("The unique identifier, if any, of the task the push notifications to get belong to.")]
    [DataMember(Order = 1, Name = "taskId"), JsonPropertyOrder(1), JsonPropertyName("taskId")]
    public string? TaskId { get; init; }

    /// <summary>
    /// Gets the maximum number, if any, of push notification configurations to return.
    /// </summary>
    [Description("The maximum number, if any, of push notification configurations to return.")]
    [DataMember(Order = 2, Name = "pageSize"), JsonPropertyOrder(2), JsonPropertyName("pageSize")]
    public uint? PageSize { get; init; }

    /// <summary>
    /// Gets the token, if any, used to retrieve the next page of results.
    /// </summary>
    [Description("The token, if any, used to retrieve the next page of results.")]
    [DataMember(Order = 3, Name = "pageToken"), JsonPropertyOrder(3), JsonPropertyName("pageToken")]
    public string? PageToken { get; init; }

}
