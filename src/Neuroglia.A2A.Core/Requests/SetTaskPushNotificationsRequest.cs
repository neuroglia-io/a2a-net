namespace Neuroglia.A2A.Requests;

/// <summary>
/// Represents the request used to configure a push notification URL for receiving an update on Task status change
/// </summary>
[DataContract]
public record SetTaskPushNotificationsRequest
    : RpcRequest<TaskPushNotificationConfiguration>
{

    /// <inheritdoc/>
    public override string Method { get; } = "tasks/pushNotification/set";

}
