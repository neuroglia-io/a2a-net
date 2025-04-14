namespace Neuroglia.A2A.Requests;

/// <summary>
/// Represents the request used to retrieve the generated Artifacts for a Task 
/// </summary>
[DataContract]
public record GetTaskRequest
    : RpcRequest<TaskQueryParameters>
{

    /// <inheritdoc/>
    public override string Method { get; } = "tasks/get";

}
