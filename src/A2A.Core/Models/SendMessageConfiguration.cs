namespace A2A.Models;

/// <summary>
/// Represents an object used to configure a request to send a message.
/// </summary>
[Description("Represents an object used to configure a request to send a message.")]
[DataContract]
public sealed record SendMessageConfiguration
{

    /// <summary>
    /// Gets a collection, if any, containing the media types the client is prepared to accept for response parts.
    /// </summary>
    [Description("A collection, if any, containing the media types the client is prepared to accept for response parts.")]
    [DataMember(Order = 1, Name = "acceptedOutputModes"), JsonPropertyOrder(1), JsonPropertyName("acceptedOutputModes")]
    public IReadOnlyCollection<string>? AcceptedOutputModes { get; init; }

    /// <summary>
    /// Gets an object, if any, used to configure the agent's push notifications for task updates.
    /// </summary>
    [Description("An object, if any, used to configure the agent's push notifications for task updates.")]
    [DataMember(Order = 2, Name = "pushNotificationConfig"), JsonPropertyOrder(2), JsonPropertyName("pushNotificationConfig")]
    public PushNotificationConfig? PushNotificationConfig { get; init; }

    /// <summary>
    /// Gets the maximum number of state transitions, if any, to include in the agent's history, if state transition history is supported.
    /// </summary>
    [Description("The maximum number of state transitions, if any, to include in the agent's history, if state transition history is supported.")]
    [DataMember(Order = 3, Name = "historyLength"), JsonPropertyOrder(3), JsonPropertyName("historyLength")]
    public uint? HistoryLength  { get; init; }

    /// <summary>
    /// Gets a boolean indicating whether the agent should block processing until the message is complete.
    /// </summary>
    [Description("A boolean indicating whether the agent should block processing until the message is complete.")]
    [DataMember(Order = 4, Name = "blocking"), JsonPropertyOrder(4), JsonPropertyName("blocking")]
    public bool? Blocking { get; init; }

}
