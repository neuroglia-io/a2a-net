namespace Neuroglia.A2A.Requests;

/// <summary>
/// Represents the request used to create, continue or restart a task
/// </summary>
[DataContract]
public record SendTaskRequest
    : RpcRequest<TaskSendParameters>
{

    /// <inheritdoc/>
    public override string Method { get; } = "tasks/send";

}
