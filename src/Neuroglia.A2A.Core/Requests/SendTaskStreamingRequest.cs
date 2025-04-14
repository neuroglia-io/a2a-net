namespace Neuroglia.A2A.Requests;

/// <summary>
/// Represents the request used to create a Task and to stream its updates
/// </summary>
[DataContract]
public record SendTaskStreamingRequest
    : RpcRequest<TaskSendParameters>
{

    /// <inheritdoc/>
    public override string Method { get; } = "tasks/sendSubscribe";

}
