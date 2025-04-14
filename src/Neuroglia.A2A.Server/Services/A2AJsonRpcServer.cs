using Neuroglia.A2A.Requests;

namespace Neuroglia.A2A.Server.Services;

/// <summary>
/// Represents the JSON RPC server for the A2A protocol
/// </summary>
public class A2AJsonRpcServer
{

    /// <summary>
    /// Sends content to a remote agent to start a new Task, resumes an interrupted Task or reopens a completed Task<para></para>
    /// A Task interrupt may be caused due to an agent requiring additional user input or a runtime error
    /// </summary>
    /// <param name="request">The request to perform</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/></param>
    /// <returns>A new <see cref="RpcResponse"/> that describes the result of the operation</returns>
    [JsonRpcMethod("tasks/send")]
    public virtual async Task<RpcResponse<Models.Task>> SendTaskAsync(SendTaskRequest request, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);

        return new();
    }

    /// <summary>
    /// Retrieves the currently configured push notification configuration for a Task
    /// </summary>
    /// <param name="request">The request to perform</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/></param>
    /// <returns>A new <see cref="RpcResponse"/> that describes the result of the operation</returns>
    [JsonRpcMethod("tasks/sendSubscribe")]
    public virtual async IAsyncEnumerable<RpcEvent> SendTaskStreamingAsync(SendTaskStreamingRequest request, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);

    }

    /// <summary>
    /// Resubscribes to a remote agent
    /// </summary>
    /// <param name="request">The request to perform</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/></param>
    /// <returns>A new <see cref="RpcResponse"/> that describes the result of the operation</returns>
    [JsonRpcMethod("tasks/sendSubscribe")]
    public virtual async IAsyncEnumerable<RpcEvent> ResubscribeToTaskAsync(TaskResubscriptionRequest request, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);

    }

    /// <summary>
    /// Retrieves the generated Artifacts for a Task
    /// </summary>
    /// <param name="request">The request to perform</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/></param>
    /// <returns>A new <see cref="RpcResponse"/> that describes the result of the operation</returns>
    [JsonRpcMethod("tasks/get")]
    public virtual async Task<RpcResponse<Models.Task>> GetTaskAsync(GetTaskRequest request, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);

        return new();
    }

    /// <summary>
    /// Cancels a previously submitted task
    /// </summary>
    /// <param name="request">The request to perform</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/></param>
    /// <returns>A new <see cref="RpcResponse"/> that describes the result of the operation</returns>
    [JsonRpcMethod("tasks/cancel")]
    public virtual async Task<RpcResponse<Models.Task>> CancelTaskAsync(CancelTaskRequest request, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);

        return new();
    }

    /// <summary>
    /// Configures a push notification URL for receiving an update on Task status change
    /// </summary>
    /// <param name="request">The request to perform</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/></param>
    /// <returns>A new <see cref="RpcResponse"/> that describes the result of the operation</returns>
    [JsonRpcMethod("tasks/pushNotification/set")]
    public virtual async Task<RpcResponse<Models.Task>> SetTaskPushNotificationsAsync(SetTaskPushNotificationsRequest request, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);

        return new();
    }

    /// <summary>
    /// Retrieves the currently configured push notification configuration for a Task
    /// </summary>
    /// <param name="request">The request to perform</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/></param>
    /// <returns>A new <see cref="RpcResponse"/> that describes the result of the operation</returns>
    [JsonRpcMethod("tasks/pushNotification/get")]
    public virtual async Task<RpcResponse<Models.Task>> GetTaskPushNotificationsAsync(GetTaskPushNotificationsRequest request, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);

        return new();
    }

}
