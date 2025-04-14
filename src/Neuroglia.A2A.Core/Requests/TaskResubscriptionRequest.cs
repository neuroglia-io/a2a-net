namespace Neuroglia.A2A.Requests;

/// <summary>
/// Represents the request used to resubscribe to a remote agent
/// </summary>
[DataContract]
public record TaskResubscriptionRequest
    : RpcRequest<TaskQueryParameters>
{

    /// <inheritdoc/>
    public override string Method { get; } = "tasks/resubscribe";

}