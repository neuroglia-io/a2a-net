namespace Neuroglia.A2A.Models;

/// <summary>
/// Represents an object used to configure push notifications
/// </summary>
[DataContract]
public record PushNotificationConfiguration
{

    /// <summary>
    /// Gets/sets the endpoint URL to which the push notification should be sent
    /// </summary>
    [Required]
    [DataMember(Name = "url", Order = 1), JsonPropertyName("url"), JsonPropertyOrder(1), YamlMember(Alias = "url", Order = 1)]
    public virtual Uri Url { get; set; } = null!;

    /// <summary>
    /// Gets/sets a token;, if any, that uniquely identifies the task or session associated with the push notification
    /// </summary>
    [DataMember(Name = "token", Order = 2), JsonPropertyName("token"), JsonPropertyOrder(2), YamlMember(Alias = "token", Order = 2)]
    public virtual string? Token { get; set; }

    /// <summary>
    /// Gets/sets information, if any, about the authentication used to push notification to the configured endpoint
    /// </summary>
    [DataMember(Name = "authentication", Order = 3), JsonPropertyName("authentication"), JsonPropertyOrder(3), YamlMember(Alias = "authentication", Order = 3)]
    public virtual AuthenticationInfo? Authentication { get; set; }

}
