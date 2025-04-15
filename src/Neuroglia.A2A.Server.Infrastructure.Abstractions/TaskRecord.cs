namespace Neuroglia.A2A.Server.Infrastructure;

/// <summary>
/// Represents the internal record of a task, extending the protocol-level task definition with additional runtime information such as push notification configuration
/// </summary>
[DataContract]
public record TaskRecord
    : Models.Task
{

    /// <summary>
    /// Gets/sets the push notification configuration associated with the task, used to notify external systems about task updates
    /// </summary>
    [DataMember(Name = "notifications", Order = 5), JsonPropertyName("notifications"), JsonPropertyOrder(5), YamlMember(Alias = "notifications", Order = 5)]
    public virtual PushNotificationConfiguration? Notifications { get; set; }

}
