namespace Neuroglia.A2A.Models;

/// <summary>
/// Represents an object used to configure task-related push notifications
/// </summary>
[DataContract]
public record TaskPushNotificationConfiguration
{

    /// <summary>
    /// Gets/sets the id of the task to push notifications about
    /// </summary>
    [Required, MinLength(1)]
    [DataMember(Name = "id", Order = 1), JsonPropertyName("id"), JsonPropertyOrder(1), YamlMember(Alias = "id", Order = 1)]
    public virtual string Id { get; set; } = null!;

    /// <summary>
    /// Gets/sets an object used to configure task-related push notifications
    /// </summary>
    [DataMember(Name = "pushNotificationConfig", Order = 2), JsonPropertyName("pushNotificationConfig"), JsonPropertyOrder(2), YamlMember(Alias = "pushNotificationConfig", Order = 2)]
    public virtual PushNotificationConfiguration? PushNotificationConfig { get; set; }

}