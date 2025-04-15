// Copyright � 2025-Present Neuroglia SRL
//
// Licensed under the Apache License, Version 2.0 (the "License"),
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

namespace Neuroglia.A2A.Server.Infrastructure.Services;

/// <summary>
/// Represents the default <see cref="IA2AProtocolServer"/> implementation
/// </summary>
/// <param name="logger">The service used to perform logging</param>
/// <param name="tasks">The service used to persist <see cref="Models.Task"/>s</param>
/// <param name="taskHandler">The service used to handle <see cref="Models.Task"/>s</param>
/// <param name="taskEventStream">The service used to stream task events</param>
public class A2AProtocolServer(ILogger<A2AProtocolServer> logger, ITaskRepository tasks, ITaskHandler taskHandler, ITaskEventStream taskEventStream)
    : IA2AProtocolServer
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
            Id = request.Id,
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
        }, cancellationToken);
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
