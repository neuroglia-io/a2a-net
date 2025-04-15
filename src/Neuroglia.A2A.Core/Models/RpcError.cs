namespace Neuroglia.A2A.Models;

/// <summary>
/// Represents an object used to describe an error that has occurred during an RPC call
/// </summary>
[DataContract]
public record RpcError
{

    /// <summary>
    /// Initializes a new <see cref="RpcError"/>
    /// </summary>
    public RpcError() { }

    /// <summary>
    /// Initializes a new <see cref="RpcError"/>
    /// </summary>
    /// <param name="code">The error code</param>
    /// <param name="message">The error message</param>
    /// <param name="data">Data, if any, associated to the error</param>
    public RpcError(int code, string message, object? data = null)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(message);
        Code = code;
        Message = message;
        Data = data;
    }

    /// <summary>
    /// Gets/sets the error code
    /// </summary>
    [DataMember(Name = "code", Order = 1), JsonPropertyName("code"), JsonPropertyOrder(1), YamlMember(Alias = "code", Order = 1)]
    public virtual int Code { get; set; }

    /// <summary>
    /// Gets/sets the error message
    /// </summary>
    [Required, MinLength(1)]
    [DataMember(Name = "message", Order = 2), JsonPropertyName("message"), JsonPropertyOrder(2), YamlMember(Alias = "message", Order = 2)]
    public virtual string Message { get; set; } = null!;

    /// <summary>
    /// Gets/sets data, if any, associated to the error
    /// </summary>
    [DataMember(Name = "data", Order = 3), JsonPropertyName("data"), JsonPropertyOrder(3), YamlMember(Alias = "data", Order = 3)]
    public virtual object? Data { get; set; }

}