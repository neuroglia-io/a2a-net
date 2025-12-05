namespace A2A.Models;

/// <summary>
/// Represents an interface exposed by an agent.
/// </summary>
[Description("Represents an interface exposed by an agent.")]
[DataContract]
public sealed record AgentInterface
{

    /// <summary>
    /// Gets the protocol binding used by the interface.
    /// </summary>
    [Description("The protocol binding used by the interface.")]
    [Required, AllowedValues(A2A.ProtocolBinding.Grpc, A2A.ProtocolBinding.Http, A2A.ProtocolBinding.JsonRpc)]
    [DataMember(Order = 1, Name = "protocolBinding"), JsonPropertyOrder(1), JsonPropertyName("protocolBinding")]
    public required string ProtocolBinding { get; init; }

    /// <summary>
    /// Gets the url where the interface is available.
    /// </summary>
    [Description("The url where the interface is available.")]
    [Required]
    [DataMember(Order = 2, Name = "url"), JsonPropertyOrder(2), JsonPropertyName("url")]
    public required Uri Url { get; init; }

}
