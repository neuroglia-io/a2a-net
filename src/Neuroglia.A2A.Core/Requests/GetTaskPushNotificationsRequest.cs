namespace Neuroglia.A2A.Requests;

/// <summary>
/// Represents the request used to retrieve the currently configured push notification configuration for a Task
/// </summary>
[DataContract]
public record GetTaskPushNotificationsRequest
    : RpcRequest<TaskIdParameters>
{

    /// <inheritdoc/>
    public override string Method { get; } = "tasks/pushNotification/get";

}