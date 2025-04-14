namespace Neuroglia.A2A.Models;

/// <summary>
/// Represents an object used to describe an agent's capabilities
/// </summary>
[DataContract]
public record AgentCapabilities
{

    /// <summary>
    /// Gets/sets a boolean indicating whether or not the agent supports streaming
    /// </summary>
    [DataMember(Name = "streaming", Order = 1), JsonPropertyName("streaming"), JsonPropertyOrder(1), YamlMember(Alias = "streaming", Order = 1)]
    public virtual bool Streaming { get; set; }

    /// <summary>
    /// Gets/sets a boolean indicating whether or not the agent can notify updates to client using push notifications
    /// </summary>
    [DataMember(Name = "pushNotifications", Order = 2), JsonPropertyName("pushNotifications"), JsonPropertyOrder(2), YamlMember(Alias = "pushNotifications", Order = 2)]
    public virtual bool PushNotifications { get; set; }

    /// <summary>
    /// Gets/sets a boolean indicating whether or not the agent exposes status change history for tasks
    /// </summary>
    [DataMember(Name = "stateTransitionHistory", Order = 3), JsonPropertyName("stateTransitionHistory"), JsonPropertyOrder(3), YamlMember(Alias = "stateTransitionHistory", Order = 3)]
    public virtual bool StateTransitionHistory { get; set; }

}