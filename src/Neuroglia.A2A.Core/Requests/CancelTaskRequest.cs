namespace Neuroglia.A2A.Requests;

/// <summary>
/// Represents the request used to cancel a previously submitted task
/// </summary>
[DataContract]
public record CancelTaskRequest
    : RpcRequest<TaskIdParameters>
{

    /// <summary>
    /// Initializes a new <see cref="CancelTaskRequest"/>
    /// </summary>
    public CancelTaskRequest() { }

    /// <summary>
    /// Initializes a new <see cref="CancelTaskRequest"/>
    /// </summary>
    /// <param name="params">The request's parameters</param>
    public CancelTaskRequest(TaskIdParameters @params)
    {
        ArgumentNullException.ThrowIfNull(@params);
        Params = @params;
    }

    /// <inheritdoc/>
    public override string Method { get; } = "tasks/cancel";

}
