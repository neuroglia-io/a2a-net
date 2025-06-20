// Copyright © 2025-Present the a2a-net Authors
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

namespace A2A.Server.Infrastructure.Services;

/// <summary>
/// Represents the default implementation of the <see cref="ITaskHandler"/> interface
/// </summary>
/// <param name="tasks">The service used to persist <see cref="Models.Task"/>s</param>
/// <param name="taskEventStream">The service used to stream task events</param>
/// <param name="agent">The service used to interact with an AI agent to execute tasks</param>
/// <param name="pushNotificationSender">The service used to send push notifications</param>
/// <param name="httpClient">The service used to perform HTTP requests</param>
public class TaskHandler(ITaskRepository tasks, ITaskEventStream taskEventStream, IAgentRuntime agent, IPushNotificationSender pushNotificationSender, HttpClient httpClient)
    : ITaskHandler
{

    /// <summary>
    /// Gets the service used to persist <see cref="Models.Task"/>s
    /// </summary>
    protected ITaskRepository Tasks { get; } = tasks;

    /// <summary>
    /// Gets the service used to stream task events
    /// </summary>
    protected ITaskEventStream TaskEventStream { get; } = taskEventStream;

    /// <summary>
    /// Gets the service used to interact with an AI agent to execute tasks
    /// </summary>
    protected IAgentRuntime AgentRuntime { get; } = agent;

    /// <summary>
    /// Gets the service used to send push notifications
    /// </summary>
    protected IPushNotificationSender PushNotificationSender { get; } = pushNotificationSender;

    /// <summary>
    /// Gets the service used to perform HTTP requests
    /// </summary>
    protected HttpClient HttpClient { get; } = httpClient;

    /// <inheritdoc/>
    public virtual async Task<TaskRecord> SubmitAsync(TaskRecord task, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(task);
        try
        {
            if (task.Status.State != TaskState.Submitted)
            {
                if (task.Status.State == TaskState.Working) task = await CancelAsync(task, new Message
                {
                    Role = MessageRole.Agent,
                    Parts =
                       [
                           new TextPart("Task automatically cancelled before resubmission")
                       ]
                }, cancellationToken).ConfigureAwait(false);
                if (task.Status.State != TaskState.InputRequired) task.Artifacts = null;
                task.History ??= [];
                if (task.Status.Message != null) task.History.Add(task.Status.Message);
                task.Status = new()
                {
                    Timestamp = DateTimeOffset.Now,
                    State = TaskState.Submitted
                };
                task = await Tasks.UpdateAsync(task, cancellationToken).ConfigureAwait(false);
                await NotifyTaskStatusUpdateAsync(task, cancellationToken).ConfigureAwait(false);
            }
            var working = false;
            await foreach (var content in AgentRuntime.ExecuteAsync(task, cancellationToken).ConfigureAwait(false))
            {
                if (!working)
                {
                    task.History ??= [];
                    if (task.Status.Message != null) task.History.Add(task.Status.Message);
                    task.Status = new()
                    {
                        Timestamp = DateTimeOffset.Now,
                        State = TaskState.Working
                    };
                    task = await Tasks.UpdateAsync(task, cancellationToken).ConfigureAwait(false);
                    await NotifyTaskStatusUpdateAsync(task, cancellationToken).ConfigureAwait(false);
                    working = true;
                }
                switch (content.Type)
                {
                    case AgentResponseContentType.Artifact:
                        task = await StreamArtifactAsync(task, content.Artifact!, cancellationToken).ConfigureAwait(false);
                        break;
                    case AgentResponseContentType.Message:
                        task = await WaitForInputAsync(task, content.Message!, cancellationToken).ConfigureAwait(false);
                        return task;
                    default:
                        throw new NotSupportedException($"The specified agent response content type '{content.Type}' is not supported");
                }
            }
            task = await CompleteAsync(task, cancellationToken).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            task = await FailAsync(task, new()
            {
                Role = MessageRole.Agent,
                Parts =
                [
                    new TextPart($"An error occurred while executing the task with id '{task.Id}': {ex}")
                ]
            }, cancellationToken).ConfigureAwait(false);
        }
        return task;
    }

    /// <inheritdoc/>
    public virtual async Task<TaskRecord> CancelAsync(TaskRecord task, Message? message = null, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(task);
        task.History ??= [];
        if (task.Status.Message != null) task.History.Add(task.Status.Message);
        task.Status = new()
        {
            Timestamp = DateTimeOffset.Now,
            State = TaskState.Cancelled,
            Message = message
        };
        task = await Tasks.UpdateAsync(task, cancellationToken).ConfigureAwait(false);
        await NotifyTaskStatusUpdateAsync(task, cancellationToken).ConfigureAwait(false);
        //todo: cancel the actual process
        return task;
    }

    /// <summary>
    /// Marks the specified task as awaiting external input
    /// </summary>
    /// <param name="task">The task to mark as awaiting external input</param>
    /// <param name="message">The message that triggered the input requirement</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/></param>
    /// <returns>The task as awaiting external input</returns>
    protected virtual async Task<TaskRecord> WaitForInputAsync(TaskRecord task, Message message, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(task);
        ArgumentNullException.ThrowIfNull(message);
        task.History ??= [];
        if (task.Status.Message != null) task.History.Add(task.Status.Message);
        task.Status = new()
        {
            Timestamp = DateTimeOffset.Now,
            State = TaskState.InputRequired,
            Message = message
        };
        task = await Tasks.UpdateAsync(task, cancellationToken).ConfigureAwait(false);
        await NotifyTaskStatusUpdateAsync(task, cancellationToken).ConfigureAwait(false);
        return task;
    }

    /// <summary>
    /// Streams an artifact generated by the agent as part of the task's output, typically used to push incremental or partial results
    /// </summary>
    /// <param name="task">The task being processed</param>
    /// <param name="artifact">The artifact to stream</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/></param>
    /// <returns>The updated task</returns>
    protected virtual async Task<TaskRecord> StreamArtifactAsync(TaskRecord task, Artifact artifact, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(task);
        ArgumentNullException.ThrowIfNull(artifact);
        task.Artifacts ??= [];
        task.Artifacts.Add(artifact);
        task = await Tasks.UpdateAsync(task, cancellationToken).ConfigureAwait(false);
        await NotifyTaskArtifactUpdateAsync(task, artifact, cancellationToken).ConfigureAwait(false);
        return task;
    }

    /// <summary>
    /// Completes the specified task
    /// </summary>
    /// <param name="task">The task to complete</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/></param>
    /// <returns>The completed task</returns>
    protected virtual async Task<TaskRecord> CompleteAsync(TaskRecord task, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(task);
        task.History ??= [];
        if (task.Status.Message != null) task.History.Add(task.Status.Message);
        task.Status = new()
        {
            Timestamp = DateTimeOffset.Now,
            State = TaskState.Completed
        };
        task = await Tasks.UpdateAsync(task, cancellationToken).ConfigureAwait(false);
        await NotifyTaskStatusUpdateAsync(task, cancellationToken).ConfigureAwait(false);
        return task;
    }

    /// <summary>
    /// Fails the specified task
    /// </summary>
    /// <param name="task">The task to fail</param>
    /// <param name="message">A message, if any, associated with the task's failure</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/></param>
    /// <returns>The failed task</returns>
    protected virtual async Task<TaskRecord> FailAsync(TaskRecord task, Message? message = null, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(task);
        task.History ??= [];
        if (task.Status.Message != null) task.History.Add(task.Status.Message);
        task.Status = new()
        {
            Timestamp = DateTimeOffset.Now,
            State = TaskState.Failed,
            Message = message
        };
        task = await Tasks.UpdateAsync(task, cancellationToken).ConfigureAwait(false);
        await NotifyTaskStatusUpdateAsync(task, cancellationToken).ConfigureAwait(false);
        return task;
    }

    /// <summary>
    /// Notifies subscribers of a change in the specified task's status
    /// </summary>
    /// <param name="task">The task whose status has been updated</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/></param>
    /// <returns>A new awaitable <see cref="System.Threading.Tasks.Task"/></returns>
    protected virtual async System.Threading.Tasks.Task NotifyTaskStatusUpdateAsync(TaskRecord task, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(task);
        TaskEventStream.OnNext(new TaskStatusUpdateEvent()
        {
            Id = task.Id,
            Status = task.Status,
            Final = task.Status.State == TaskState.Completed || task.Status.State == TaskState.Cancelled || task.Status.State == TaskState.Failed
        });
        if (task.Notifications != null) await PushNotificationSender.SendPushNotificationAsync(task.Notifications.Url, task, cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Notifies subscribers that a new artifact has been produced by the specified task
    /// </summary>
    /// <param name="task">The task the artifact has been produced for</param>
    /// <param name="artifact">The newly produced artifact</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/></param>
    /// <returns>A new awaitable <see cref="System.Threading.Tasks.Task"/></returns>
    protected virtual async System.Threading.Tasks.Task NotifyTaskArtifactUpdateAsync(TaskRecord task, Artifact artifact, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(task);
        ArgumentNullException.ThrowIfNull(artifact);
        TaskEventStream.OnNext(new TaskArtifactUpdateEvent
        {
            Id = task.Id,
            Artifact = artifact
        });
        if (task.Notifications != null) await PushNotificationSender.SendPushNotificationAsync(task.Notifications.Url, task, cancellationToken).ConfigureAwait(false);
    }

}