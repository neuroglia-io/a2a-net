namespace A2A.Models;

/// <summary>
/// Represents an object used to configure push notifications for task updates.
/// </summary>
[Description("Represents an object used to configure push notifications for task updates.")]
[DataContract]
public sealed record PushNotificationConfig
{

    /// <summary>
    /// Gets the configuration's unique identifier, if any.
    /// </summary>
    [Description("The configuration's unique identifier, if any.")]
    [DataMember(Order = 1, Name = "id"), JsonPropertyOrder(1), JsonPropertyName("id")]
    public string? Id { get; init; }

    /// <summary>
    /// Gets the URL to which to send push notifications.
    /// </summary>
    [Description("The URL to which to send push notifications.")]
    [DataMember(Order = 2, Name = "url"), JsonPropertyOrder(2), JsonPropertyName("url")]
    public required Uri Url { get; init; }

    /// <summary>
    /// Gets the token used to authenticate push notifications, if any.
    /// </summary>
    [Description("The token used to authenticate push notifications, if any.")]
    [DataMember(Order = 3, Name = "token"), JsonPropertyOrder(3), JsonPropertyName("token")]
    public string? Token { get; init; }

    /// <summary>
    /// Gets information about the authentication used for push notifications, if any.
    /// </summary>
    [Description("Information about the authentication used for push notifications, if any.")]
    [DataMember(Order = 4, Name = "authentication"), JsonPropertyOrder(4), JsonPropertyName("authentication")]
    public AuthenticationInfo? Authentication { get; init; }

}
