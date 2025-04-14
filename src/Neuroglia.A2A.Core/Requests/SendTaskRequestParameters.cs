﻿namespace Neuroglia.A2A.Requests;

/// <summary>
/// Represents the parameters of a request used to create, continue or restart a task
/// </summary>
[DataContract]
public record SendTaskRequestParameters
{

    /// <summary>
    /// Gets/sets the task's unique identifier
    /// </summary>
    [Required, MinLength(1)]
    [DataMember(Name = "id", Order = 1), JsonPropertyName("id"), JsonPropertyOrder(1), YamlMember(Alias = "id", Order = 1)]
    public virtual string Id { get; set; } = null!;

    /// <summary>
    /// Gets/sets the unique identifier of the session the task belongs to<para></para>
    /// The server creates a new session id for new tasks if not set
    /// </summary>
    [DataMember(Name = "sessionId", Order = 2), JsonPropertyName("sessionId"), JsonPropertyOrder(2), YamlMember(Alias = "sessionId", Order = 2)]
    public virtual string? SessionId { get; set; }

    /// <summary>
    /// Gets/sets the task's message
    /// </summary>
    [Required]
    [DataMember(Name = "message", Order = 3), JsonPropertyName("message"), JsonPropertyOrder(3), YamlMember(Alias = "message", Order = 3)]
    public virtual Message Message { get; set; } = null!;

    /// <summary>
    /// Gets/sets the number of recent messages, if any, to be retrieved
    /// </summary>
    [Required]
    [DataMember(Name = "historyLength", Order = 4), JsonPropertyName("historyLength"), JsonPropertyOrder(4), YamlMember(Alias = "historyLength", Order = 4)]
    public virtual uint? HistoryLength { get; set; } = null!;
    
    /// <summary>
    /// Gets/sets the configuration, if any, for the task's push notifications
    /// </summary>
    [DataMember(Name = "pushNotification", Order = 5), JsonPropertyName("pushNotification"), JsonPropertyOrder(5), YamlMember(Alias = "pushNotification", Order = 5)]
    public virtual PushNotificationConfiguration? PushNotification { get; set; }

    /// <summary>
    /// Gets/sets a key/value mapping that contains the request's additional properties, if any
    /// </summary>
    [DataMember(Name = "metadata", Order = 99), JsonPropertyName("metadata"), JsonPropertyOrder(99), YamlMember(Alias = "metadata", Order = 99)]
    public virtual EquatableDictionary<string, object>? Metadata { get; set; }

}
