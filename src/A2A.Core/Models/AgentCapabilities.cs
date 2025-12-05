namespace A2A.Models;

/// <summary>
/// Represents an object used to describe an agent's capabilities.
/// </summary>
[Description("Represents an object used to describe an agent's capabilities.")]
[DataContract]
public sealed record AgentCapabilities
{

    /// <summary>
    /// Gets a boolean indicating whether the agent supports streaming responses.
    /// </summary>
    [Description("A boolean indicating whether the agent supports streaming responses.")]
    [DataMember(Order = 1, Name = "streaming"), JsonPropertyOrder(1), JsonPropertyName("streaming")]
    public bool? Streaming { get; init; }

    /// <summary>
    /// Gets a boolean indicating whether the agent supports push notifications.
    /// </summary>
    [Description("A boolean indicating whether the agent supports push notifications.")]
    [DataMember(Order = 2, Name = "pushNotifications"), JsonPropertyOrder(2), JsonPropertyName("pushNotifications")]
    public bool? PushNotifications { get; init; }

    /// <summary>
    /// Gets a boolean indicating whether the agent maintains a state transition history.
    /// </summary>
    [Description("A boolean indicating whether the agent maintains a state transition history.")]
    [DataMember(Order = 3, Name = "stateTransitionHistory"), JsonPropertyOrder(3), JsonPropertyName("stateTransitionHistory")]
    public bool? StateTransitionHistory { get; init; }

    /// <summary>
    /// Gets a collection containing the agent's extensions, if any.
    /// </summary>
    [Description("A collection containing the agent's extensions, if any.")]
    [DataMember(Order = 4, Name = "extensions"), JsonPropertyOrder(4), JsonPropertyName("extensions")]
    public IReadOnlyCollection<AgentExtension>? Extensions { get; init; }

}
