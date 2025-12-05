using A2A.Models;

namespace A2A.Server.Services;

/// <summary>
/// Represents the default implementation of the <see cref="IA2AProtocolServer"/> interface.
/// </summary>
/// <param name="logger">The service used to perform logging.</param>
/// <param name="pushNotificationSender">The service used to send push notifications.</param>
/// <param name="store">The service used to store and retrieve A2A-specific resources.</param>
/// <param name="taskEvents">The service used to stream task-related events.</param>
/// <param name="taskQueue">The service used to enqueue tasks for execution.</param>
/// <param name="agent">The service used to interact with the agent runtime.</param>
public sealed class A2AProtocolServer(ILogger<A2AProtocolServer> logger, IPushNotificationSender pushNotificationSender, IStateStore store, ITaskEventStream taskEvents, ITaskQueue taskQueue, IAgentRuntime agent)
    : IA2AProtocolServer
{

    /// <inheritdoc/>
    public async Task<Response> SendMessageAsync(SendMessageRequest request, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);
        if (request.Configuration?.PushNotificationConfig is not null && !await pushNotificationSender.VerifyPushNotificationUrlAsync(request.Configuration.PushNotificationConfig.Url, cancellationToken).ConfigureAwait(false))
        {
            logger.LogError("Failed to validate the specified push notification url '{url}'", request.Configuration.PushNotificationConfig.Url);
            throw new A2AException(ErrorCode.UnsupportedOperation, $"Failed to validate the specified push notification url '{request.Configuration.PushNotificationConfig.Url}'");
        }
        if (string.IsNullOrWhiteSpace(request.Message.TaskId))
        {
            var response = await agent.ProcessAsync(request.Message, cancellationToken).ConfigureAwait(false);
            if (response is Models.Task task)
            {
                task = await store.AddTaskAsync(task, cancellationToken).ConfigureAwait(false);
                taskQueue.EnqueueAsync(task, cancellationToken).ConfigureAwait(false);
            }
            return response;
        }
        else
        {
            var task = await store.GetTaskAsync(request.Message.TaskId, cancellationToken).ConfigureAwait(false) ?? throw new A2AException(ErrorCode.TaskNotFound, $"Failed to find a task with the specified id '{request.Message.TaskId}'");
            if (task.Status.State is TaskState.Cancelled or TaskState.Completed or TaskState.Failed)
            {
                logger.LogError("Failed to process the message for the task with id '{id}' because the task is in an unexpected state '{state}'", task.Id, task.Status.State);
                throw new A2AException(ErrorCode.UnsupportedOperation, $"Failed to process the message for the task with id '{task.Id}' because the task is in an unexpected state '{task.Status.State}'");
            }
            task = await store.UpdateTaskAsync(task with
            {
                History = task.History is null ? [request.Message] : task.History.Append(request.Message).ToList()
            }, cancellationToken).ConfigureAwait(false);
            taskQueue.EnqueueAsync(task, cancellationToken).ConfigureAwait(false);
            return task;
        }
    }

    /// <inheritdoc/>
    public async IAsyncEnumerable<StreamResponse> SendStreamingMessageAsync(SendMessageRequest request, [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);
        if (request.Configuration?.PushNotificationConfig is not null && !await pushNotificationSender.VerifyPushNotificationUrlAsync(request.Configuration.PushNotificationConfig.Url, cancellationToken).ConfigureAwait(false))
        {
            logger.LogError("Failed to validate the specified push notification url '{url}'", request.Configuration.PushNotificationConfig.Url);
            throw new A2AException(ErrorCode.UnsupportedOperation, $"Failed to validate the specified push notification url '{request.Configuration.PushNotificationConfig.Url}'");
        }
        Models.Task task;
        if (string.IsNullOrWhiteSpace(request.Message.TaskId))
        {
            var response = await agent.ProcessAsync(request.Message, cancellationToken).ConfigureAwait(false);
            switch (response)
            {
                case Message message:
                    yield return new StreamResponse
                    {
                        Message = message
                    };
                    yield break;
                case Models.Task:
                    task = await store.AddTaskAsync((Models.Task)response, cancellationToken).ConfigureAwait(false);
                    break;
                default:
                    throw new NotSupportedException($"The specified response type '{response?.GetType().FullName}' is not supported in this context.");
            }
        }
        else
        {
            task = await store.GetTaskAsync(request.Message.TaskId, cancellationToken).ConfigureAwait(false) ?? throw new A2AException(ErrorCode.TaskNotFound, $"Failed to find a task with the specified id '{request.Message.TaskId}'");
            if (task.Status.State is TaskState.Cancelled or TaskState.Completed or TaskState.Failed)
            {
                logger.LogError("Failed to process the message for the task with id '{id}' because the task is in an unexpected state '{state}'", task.Id, task.Status.State);
                throw new A2AException(ErrorCode.UnsupportedOperation, $"Failed to process the message for the task with id '{task.Id}' because the task is in an unexpected state '{task.Status.State}'");
            }
            task = await store.UpdateTaskAsync(task with
            {
                History = task.History is null ? [request.Message] : task.History.Append(request.Message).ToList()
            }, cancellationToken).ConfigureAwait(false);
        }
        taskQueue.EnqueueAsync(task, cancellationToken).ConfigureAwait(false);
        if (string.IsNullOrWhiteSpace(request.Message.TaskId)) yield return new StreamResponse
        {
            Task = task
        };
        await foreach (var e in taskEvents.Where(e => e.TaskId == task.Id).ToAsyncEnumerable().WithCancellation(cancellationToken)) yield return e switch
        {
            TaskArtifactUpdateEvent artifactUpdate => new StreamResponse
            {
                ArtifactUpdate = artifactUpdate
            },
            TaskStatusUpdateEvent statusUpdate => new StreamResponse
            {
                StatusUpdate = statusUpdate
            },
            _ => throw new NotSupportedException($"The specified task event type '{e?.GetType().FullName}' is not supported in this context.")
        };
    }

    /// <inheritdoc/>
    public async Task<Models.Task> GetTaskAsync(string id, uint? historyLength, CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(id);
        var task = await store.GetTaskAsync(id, cancellationToken).ConfigureAwait(false) ?? throw new A2AException(ErrorCode.TaskNotFound, $"Failed to find a task with the specified id '{id}'");
        if (historyLength.HasValue && task.History is not null) task = task with 
        { 
            History = [.. task.History.TakeLast((int)historyLength.Value)]
        };
        return task;
    }

    /// <inheritdoc/>
    public Task<TaskQueryResult> ListTasksAsync(TaskQueryOptions? queryOptions = null, CancellationToken cancellationToken = default) => store.ListTaskAsync(queryOptions, cancellationToken);

    /// <inheritdoc/>
    public async Task<Models.Task> CancelTaskAsync(string id, CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(id);
        var task = await store.GetTaskAsync(id, cancellationToken).ConfigureAwait(false) ?? throw new A2AException(ErrorCode.TaskNotFound, $"Failed to find a task with the specified id '{id}'");
        if (task.Status.State is not not TaskState.AuthRequired and not TaskState.InputRequired and not TaskState.Submitted and not TaskState.Unspecified and not TaskState.Working)
        {
            logger.LogError($"Failed to cancel the task with id '{id}' because it is in an unexpected state '{task.Status}'", id, task.Status);
            throw new A2AException(ErrorCode.TaskNotCancelable, $"Failed to cancel the task with id '{id}' because it is in an unexpected state '{task.Status}'");
        }
        task = await store.UpdateTaskAsync(task with
        { 
            Status = task.Status with
            {
                State = TaskState.Cancelled, 
                Timestamp = DateTime.UtcNow
            }
        }, cancellationToken).ConfigureAwait(false);
        taskQueue.CancelAsync(task, cancellationToken).ConfigureAwait(false);
        return task;
    }

    /// <inheritdoc/>
    public async IAsyncEnumerable<StreamResponse> SubscribeToTaskAsync(string id, [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(id);
        var task = await store.GetTaskAsync(id, cancellationToken).ConfigureAwait(false) ?? throw new A2AException(ErrorCode.TaskNotFound, $"Failed to find a task with the specified id '{id}'");
        if (task.Status.State != TaskState.Working) yield break;
        await foreach (var e in taskEvents.Where(e => e.TaskId == task.Id).ToAsyncEnumerable().WithCancellation(cancellationToken)) yield return e switch
        {
            TaskArtifactUpdateEvent artifactUpdate => new StreamResponse
            {
                ArtifactUpdate = artifactUpdate
            },
            TaskStatusUpdateEvent statusUpdate => new StreamResponse 
            { 
                StatusUpdate = statusUpdate
            },
            _ => throw new NotSupportedException($"The specified task event type '{e?.GetType().FullName}' is not supported in this context.")
        };
    }

    /// <inheritdoc/>
    public async Task<PushNotificationConfig> SetOrUpdatePushNotificationConfigAsync(string taskId, PushNotificationConfig config, CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(taskId);
        ArgumentNullException.ThrowIfNull(config);
        return await store.SetOrUpdatePushNotificationConfigAsync(taskId, config, cancellationToken).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public async Task<PushNotificationConfig> GetPushNotificationConfigAsync(string taskId, string configId, CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(taskId);
        ArgumentException.ThrowIfNullOrWhiteSpace(configId);
        return await store.GetPushNotificationConfigAsync(taskId, configId, cancellationToken).ConfigureAwait(false) ?? throw new A2AException(ErrorCode.TaskNotFound, $"Failed to find a push notification configuration with the specified id '{configId}' for the task with id '{taskId}'");
    }

    /// <inheritdoc/>
    public Task<PushNotificationConfigQueryResult> ListPushNotificationConfigAsync(PushNotificationConfigQueryOptions? queryOptions = null, CancellationToken cancellationToken = default) => store.ListPushNotificationConfigAsync(queryOptions, cancellationToken);

    /// <inheritdoc/>
    public Task<bool> DeletePushNotificationConfigAsync(string taskId, string configId, CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(taskId);
        ArgumentException.ThrowIfNullOrWhiteSpace(configId);
        return store.DeletePushNotificationConfigAsync(taskId, configId, cancellationToken);
    }

    /// <inheritdoc/>
    public async Task<AgentCard> GetExtendedAgentCardAsync(CancellationToken cancellationToken = default)
    {
        
    }

}
