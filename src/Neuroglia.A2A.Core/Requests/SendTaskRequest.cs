namespace Neuroglia.A2A.Requests;

/// <summary>
/// Represents the request used to create, continue or restart a task
/// </summary>
[DataContract]
public record SendTaskRequest
    : RpcRequest<SendTaskRequestParameters>
{

    /// <summary>
    /// Gets/sets the name of the A2A Protocol method to use
    /// </summary>
    public override string Method { get; } = "tasks/send";

}
