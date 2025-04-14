namespace Neuroglia.A2A;

/// <summary>
/// Represents the base class for all A2A RPC requests
/// </summary>
[DataContract]
public abstract record RpcRequest
    : RpcMessage
{

    /// <summary>
    /// Gets/sets the name of the A2A Protocol method to use
    /// </summary>
    [Required, MinLength(1)]
    [DataMember(Name = "method", Order = 2), JsonInclude, JsonPropertyName("method"), JsonPropertyOrder(2), YamlMember(Alias = "method", Order = 2)]
    public abstract string Method { get; }

}

/// <summary>
/// Represents the base class for all A2A RPC requests
/// </summary>
/// <typeparam name="TParams">The type of the request's parameters</typeparam>
[DataContract]
public abstract record RpcRequest<TParams>
    : RpcRequest
    where TParams : class
{

    /// <summary>
    /// Gets/sets the request's parameters
    /// </summary>
    [Required]
    [DataMember(Name = "params", Order = 3), JsonInclude, JsonPropertyName("params"), JsonPropertyOrder(3), YamlMember(Alias = "params", Order = 3)]
    public virtual TParams Params { get; set; } = null!;

}
