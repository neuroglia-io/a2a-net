namespace Neuroglia.A2A;

/// <summary>
/// Represents the base class for all A2A responses
/// </summary>
[DataContract]
public abstract record RpcResponse
    : RpcMessage
{

    /// <summary>
    /// Gets/sets the error, if any, that has occurred during the request's execution
    /// </summary>
    [DataMember(Name = "error", Order = 2), JsonPropertyName("error"), JsonPropertyOrder(2), YamlMember(Alias = "error", Order = 2)]
    public virtual RpcError? Error { get; set; } = null!;

}

/// <summary>
/// Represents the base class for all A2A responses
/// </summary>
/// <typeparam name="TResult">The type of the response's content</typeparam>
[DataContract]
public record RpcResponse<TResult>
    : RpcResponse
    where TResult : class
{

    /// <summary>
    /// Gets/sets the response's content
    /// </summary>
    [Required]
    [DataMember(Name = "result", Order = 2), JsonInclude, JsonPropertyName("result"), JsonPropertyOrder(2), YamlMember(Alias = "result", Order = 2)]
    public virtual TResult? Result { get; set; } = null!;

}