namespace Neuroglia.A2A.Errors;

/// <summary>
/// Represents an error indicating that the specified task could not be found.
/// </summary>
[DataContract]
public record TaskNotFoundError()
    : RpcError(ErrorCode, "Task not found")
{

    /// <summary>
    /// Gets the error code associated with the <see cref="TaskNotFoundError"/>
    /// </summary>
    public const int ErrorCode = -32001;

}
