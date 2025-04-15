namespace Neuroglia.A2A.Server.Infrastructure.Services;

/// <summary>
/// Represents the default implementation of the <see cref="IA2AProtocolHandler"/> interface
/// </summary>
/// <param name="logger">The service used to perform logging</param>
/// <param name="tasks">The service used to persist <see cref="Models.Task"/>s</param>
/// <param name="taskHandler">The service used to handle <see cref="Models.Task"/>s</param>
/// <param name="taskEventStream">The service used to stream task events</param>
public class A2AProtocolHandler(ILogger<A2AProtocolHandler> logger, ITaskRepository tasks, ITaskHandler taskHandler, ITaskEventStream taskEventStream)
    : IA2AProtocolHandler
{

    /// <summary>
    /// Gets the service used to perform logging
    /// </summary>
    protected ILogger Logger { get; } = logger;

    /// <summary>
    /// Gets the service used to persist <see cref="Models.Task"/>s
    /// </summary>
    protected ITaskRepository Tasks { get; } = tasks;

    /// <summary>
    /// Gets the service used to handle <see cref="Models.Task"/>s
    /// </summary>
    protected ITaskHandler TaskHandler { get; } = taskHandler;

    /// <summary>
    /// Gets the service used to stream task events
    /// </summary>
    protected ITaskEventStream TaskEventStream { get; } = taskEventStream;

    /// <inheritdoc/>
    public virtual async Task<RpcResponse<Models.Task>> SendTaskAsync(SendTaskRequest request, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);
        var task = await Tasks.GetAsync(request.Params.Id, cancellationToken).ConfigureAwait(false) ?? await Tasks.AddAsync(new()
        {
            Id = request.Params.Id,
            SessionId = request.Params.SessionId ?? Guid.NewGuid().ToString("N"),
            Status = new()
            {
                Timestamp = DateTimeOffset.Now,
                State = TaskState.Submitted
            },
            Notifications = request.Params.PushNotification
        },
        cancellationToken).ConfigureAwait(false);
        task = await TaskHandler.SubmitAsync(task, cancellationToken).ConfigureAwait(false);
        return new()
        {
            Id = task.Id,
            Result = task.AsTask()
        };
    }

    /// <inheritdoc/>
    public virtual async IAsyncEnumerable<RpcResponse<TaskEvent>> SendTaskStreamingAsync(SendTaskStreamingRequest request, [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);
        var task = await Tasks.GetAsync(request.Params.Id, cancellationToken).ConfigureAwait(false) ?? await Tasks.AddAsync(new()
        {
            Id = request.Params.Id,
            SessionId = request.Params.SessionId ?? Guid.NewGuid().ToString("N"),
            Status = new()
            {
                Timestamp = DateTimeOffset.Now,
                State = TaskState.Submitted
            },
            Notifications = request.Params.PushNotification
        },
        cancellationToken).ConfigureAwait(false);
        _ = System.Threading.Tasks.Task.Run(async () =>
        {
            try
            {
                await TaskHandler.SubmitAsync(task, cancellationToken);
            }
            catch (Exception ex)
            {
                Logger.LogError("An error occurred while (re)submitting the task with id '{taskId}': {ex}", task.Id, ex);
            }
        });
        await foreach (var e in TaskEventStream.Where(e => e.Id == request.Params.Id).ToAsyncEnumerable().WithCancellation(cancellationToken)) yield return new()
        {
            Id = request.Id,
            Result = e
        };
    }

    /// <inheritdoc/>
    public virtual async IAsyncEnumerable<RpcResponse<TaskEvent>> ResubscribeToTaskAsync(TaskResubscriptionRequest request, [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);
        var task = await Tasks.GetAsync(request.Params.Id, cancellationToken).ConfigureAwait(false) ?? throw new LocalRpcException($"Failed to find a task with the specified id '{request.Params.Id}'");
        if (task.Status.State == TaskState.Working) yield break; 
        await foreach (var e in TaskEventStream.Where(e => e.Id == request.Params.Id).ToAsyncEnumerable().WithCancellation(cancellationToken)) yield return new()
        {
            Id = request.Id,
            Result = e
        };
    }

    /// <inheritdoc/>
    public virtual async Task<RpcResponse<Models.Task>> GetTaskAsync(GetTaskRequest request, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);
        var task = (await Tasks.GetAsync(request.Params.Id, cancellationToken).ConfigureAwait(false))?.AsTask();
        if (task == null) return new()
        {
            Id = request.Id,
            Error = new TaskNotFoundError()
        };
        var history = task.History;
        if (request.Params.HistoryLength.HasValue && history != null) history = [..history.TakeLast((int)request.Params.HistoryLength.Value)];
        return new()
        {
            Id = request.Id,
            Result = task with
            {
                History = history
            }
        };
    }

    /// <inheritdoc/>
    public virtual async Task<RpcResponse<Models.Task>> CancelTaskAsync(CancelTaskRequest request, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);
        var task = await Tasks.GetAsync(request.Params.Id, cancellationToken).ConfigureAwait(false);
        if (task == null) return new()
        {
            Id = request.Id,
            Error = new TaskNotFoundError()
        };
        task = await TaskHandler.CancelAsync(task, new()
        {
            Role = MessageRole.Agent,
            Parts =
            [
                new TextPart("Task cancelled by user")
            ]
        }, cancellationToken).ConfigureAwait(false);
        return new()
        {
            Id = request.Id,
            Result = task.AsTask()
        };
    }

    /// <inheritdoc/>
    public virtual async Task<RpcResponse<TaskPushNotificationConfiguration>> SetTaskPushNotificationsAsync(SetTaskPushNotificationsRequest request, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);
        var task = await Tasks.GetAsync(request.Params.Id, cancellationToken).ConfigureAwait(false);
        if (task == null) return new()
        {
            Id = request.Id,
            Error = new TaskNotFoundError()
        };
        task.Notifications = request.Params.PushNotificationConfig;
        task = await Tasks.UpdateAsync(task, cancellationToken).ConfigureAwait(false);
        return new()
        {
            Id = request.Id,
            Result = new()
            {
                Id = task.Id,
                PushNotificationConfig = task.Notifications
            }
        };
    }

    /// <inheritdoc/>
    public virtual async Task<RpcResponse<TaskPushNotificationConfiguration>> GetTaskPushNotificationsAsync(GetTaskPushNotificationsRequest request, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);
        var task = await Tasks.GetAsync(request.Params.Id, cancellationToken).ConfigureAwait(false);
        if (task == null) return new()
        {
            Id = request.Id,
            Error = new TaskNotFoundError()
        };
        return new()
        {
            Id = request.Id,
            Result = new()
            {
                Id = task.Id,
                PushNotificationConfig = task.Notifications
            }
        };
    }

}
