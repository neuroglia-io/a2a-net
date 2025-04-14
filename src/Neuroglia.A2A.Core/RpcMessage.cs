namespace Neuroglia.A2A;

/// <summary>
/// Represents the base class for all A2A RPC messages
/// </summary>
public abstract record RpcMessage
{

    /// <summary>
    /// Gets/sets the JSON RPC version to use
    /// </summary>
    [Required, AllowedValues(JsonRpcVersion.V2), DefaultValue(JsonRpcVersion.V2)]
    [DataMember(Name = "jsonrpc", Order = 0), JsonPropertyName("jsonrpc"), JsonPropertyOrder(0), YamlMember(Alias = "jsonrpc", Order = 0)]
    public virtual string JsonRpc { get; set; } = JsonRpcVersion.V2;

    /// <summary>
    /// Gets/sets the message's unique identifier
    /// </summary>
    [Required, MinLength(1)]
    [DataMember(Name = "id", Order = 1), JsonPropertyName("id"), JsonPropertyOrder(1), YamlMember(Alias = "id", Order = 1)]
    public virtual string Id { get; set; } = null!;

}