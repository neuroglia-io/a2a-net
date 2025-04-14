namespace Neuroglia.A2A.Requests;

/// <summary>
/// Represents the request used to cancel a previously submitted task
/// </summary>
[DataContract]
public record CancelTaskRequest
    : RpcRequest<TaskIdParameters>
{

    /// <inheritdoc/>
    public override string Method { get; } = "tasks/cancel";

}
