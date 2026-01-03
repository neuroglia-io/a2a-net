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

using A2A.Models;
using Task = System.Threading.Tasks.Task;

namespace A2A.Server.Services;

/// <summary>
/// Represents the default implementation of the <see cref="IA2AServer"/> interface.
/// </summary>
/// <param name="logger">The service used to perform logging.</param>
/// <param name="serviceProvider">The current <see cref="IServiceProvider"/>.</param>
/// <param name="pushNotificationSender">The service used to send push notifications.</param>
/// <param name="store">The service used to store and retrieve A2A-specific resources.</param>
/// <param name="taskEvents">The service used to stream task-related events.</param>
/// <param name="taskQueue">The service used to enqueue tasks for execution.</param>
/// <param name="agent">The service used to interact with the agent runtime.</param>
/// <param name="agentCard">The agent card representing the agent's capabilities and configurations.</param>
public sealed class A2AServer(ILogger<A2AServer> logger, IServiceProvider serviceProvider, IA2APushNotificationSender pushNotificationSender, IA2AStore store, IA2ATaskEventStream taskEvents, IA2ATaskQueue taskQueue, IA2AAgentRuntime agent, AgentCard agentCard)
    : IA2AServer
{

    /// <inheritdoc/>
    public async Task<Response> SendMessageAsync(SendMessageRequest request, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);
        if (request.Configuration?.PushNotificationConfig is not null)
        {
            if (agentCard.Capabilities?.PushNotifications != true)
            {
                logger.LogError("Push notifications are not supported by the agent.");
                throw new A2AException(ErrorCode.PushNotificationNotSupported);
            }
            if (!await pushNotificationSender.VerifyPushNotificationUrlAsync(request.Configuration.PushNotificationConfig.Url, cancellationToken).ConfigureAwait(false))
            {
                logger.LogError("Failed to validate the specified push notification url '{url}'", request.Configuration.PushNotificationConfig.Url);
                throw new A2AException(ErrorCode.UnsupportedOperation, $"Failed to validate the specified push notification url '{request.Configuration.PushNotificationConfig.Url}'");
            }
        }
        if (string.IsNullOrWhiteSpace(request.Message.TaskId))
        {
            var response = await agent.ProcessAsync(request.Message, new A2AAgentInvocationContext(request.Tenant, request.Metadata), cancellationToken).ConfigureAwait(false);
            if (response is Models.Task task)
            {
                task = await store.AddTaskAsync(task, request.Tenant, cancellationToken).ConfigureAwait(false);
                await taskQueue.EnqueueAsync(task, request.Tenant, cancellationToken).ConfigureAwait(false);
            }
            return response;
        }
        else
        {
            var task = await store.GetTaskAsync(request.Message.TaskId, request.Tenant, cancellationToken).ConfigureAwait(false) ?? throw new A2AException(ErrorCode.TaskNotFound, $"Failed to find a task with the specified id '{request.Message.TaskId}'");
            if (task.Status.State is not TaskState.Unspecified and not TaskState.Submitted and not TaskState.AuthRequired and not TaskState.InputRequired)
            {
                logger.LogError("Failed to process the message for the task with id '{id}' because the task is in an unexpected state '{state}'", task.Id, task.Status.State);
                throw new A2AException(ErrorCode.UnsupportedOperation, $"Failed to process the message for the task with id '{task.Id}' because the task is in an unexpected state '{task.Status.State}'");
            }
            task = await store.UpdateTaskAsync(task with
            {
                History = task.History is null ? [request.Message] : task.History.Append(request.Message).ToList()
            }, request.Tenant, cancellationToken).ConfigureAwait(false);
            await taskQueue.EnqueueAsync(task, request.Tenant, cancellationToken).ConfigureAwait(false);
            return task;
        }
    }

    /// <inheritdoc/>
    public async IAsyncEnumerable<StreamResponse> SendStreamingMessageAsync(SendMessageRequest request, [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);
        if (agentCard.Capabilities?.Streaming != true) 
        {
            logger.LogError("Streaming is not supported by the agent.");
            throw new A2AException(ErrorCode.UnsupportedOperation, "Streaming is not supported by the agent.");
        }
        if (request.Configuration?.PushNotificationConfig is not null)
        {
            if (agentCard.Capabilities?.PushNotifications != true)
            {
                logger.LogError("Push notifications are not supported by the agent.");
                throw new A2AException(ErrorCode.PushNotificationNotSupported);
            }
            if (!await pushNotificationSender.VerifyPushNotificationUrlAsync(request.Configuration.PushNotificationConfig.Url, cancellationToken).ConfigureAwait(false))
            {
                logger.LogError("Failed to validate the specified push notification url '{url}'", request.Configuration.PushNotificationConfig.Url);
                throw new A2AException(ErrorCode.UnsupportedOperation, $"Failed to validate the specified push notification url '{request.Configuration.PushNotificationConfig.Url}'");
            }
        }
        Models.Task task;
        if (string.IsNullOrWhiteSpace(request.Message.TaskId))
        {
            var response = await agent.ProcessAsync(request.Message, new A2AAgentInvocationContext(request.Tenant, request.Metadata), cancellationToken).ConfigureAwait(false);
            switch (response)
            {
                case Message message:
                    yield return new StreamResponse
                    {
                        Message = message
                    };
                    yield break;
                case Models.Task:
                    task = await store.AddTaskAsync((Models.Task)response, request.Tenant, cancellationToken).ConfigureAwait(false);
                    break;
                default:
                    throw new NotSupportedException($"The specified response type '{response?.GetType().FullName}' is not supported in this context.");
            }
        }
        else
        {
            task = await store.GetTaskAsync(request.Message.TaskId, request.Tenant, cancellationToken).ConfigureAwait(false) ?? throw new A2AException(ErrorCode.TaskNotFound, $"Failed to find a task with the specified id '{request.Message.TaskId}'");
            if (task.Status.State is not TaskState.Unspecified and not TaskState.Submitted and not TaskState.AuthRequired and not TaskState.InputRequired)
            {
                logger.LogError("Failed to process the message for the task with id '{id}' because the task is in an unexpected state '{state}'", task.Id, task.Status.State);
                throw new A2AException(ErrorCode.UnsupportedOperation, $"Failed to process the message for the task with id '{task.Id}' because the task is in an unexpected state '{task.Status.State}'");
            }
            task = await store.UpdateTaskAsync(task with
            {
                History = task.History is null ? [request.Message] : task.History.Append(request.Message).ToList()
            }, request.Tenant, cancellationToken).ConfigureAwait(false);
        }
        await taskQueue.EnqueueAsync(task, request.Tenant, cancellationToken).ConfigureAwait(false);
        if (string.IsNullOrWhiteSpace(request.Message.TaskId)) yield return new StreamResponse
        {
            Task = task
        };
        await foreach (var e in taskEvents.Where(e => e.TaskId == task.Id).TakeUntil(e => e is TaskStatusUpdateEvent statusUpdate && statusUpdate.Final).ToAsyncEnumerable().WithCancellation(cancellationToken)) yield return e switch
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
    public async Task<Models.Task> GetTaskAsync(string id, uint? historyLength = null, string? tenant = null, CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(id);
        var task = await store.GetTaskAsync(id, tenant, cancellationToken).ConfigureAwait(false) ?? throw new A2AException(ErrorCode.TaskNotFound, $"Failed to find a task with the specified id '{id}'");
        if (historyLength.HasValue && task.History is not null) task = task with 
        { 
            History = [.. task.History.TakeLast((int)historyLength.Value)]
        };
        return task;
    }

    /// <inheritdoc/>
    public Task<TaskQueryResult> ListTasksAsync(TaskQueryOptions? queryOptions = null, CancellationToken cancellationToken = default) => store.ListTaskAsync(queryOptions, cancellationToken);

    /// <inheritdoc/>
    public async Task<Models.Task> CancelTaskAsync(string id, string? tenant = null, CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(id);
        var task = await store.GetTaskAsync(id, tenant, cancellationToken).ConfigureAwait(false) ?? throw new A2AException(ErrorCode.TaskNotFound, $"Failed to find a task with the specified id '{id}'");
        if (task.Status.State is not not TaskState.AuthRequired and not TaskState.InputRequired and not TaskState.Submitted and not TaskState.Unspecified and not TaskState.Working)
        {
            logger.LogError($"Failed to cancel the task with id '{id}' because it is in an unexpected state '{task.Status}'", id, task.Status);
            throw new A2AException(ErrorCode.TaskNotCancelable, $"Failed to cancel the task with id '{id}' because it is in an unexpected state '{task.Status}'");
        }
        task.Status = task.Status with
        {
            State = TaskState.Cancelled,
            Timestamp = DateTime.UtcNow
        };
        task = await store.UpdateTaskAsync(task, tenant, cancellationToken).ConfigureAwait(false);
        await taskQueue.CancelAsync(task, tenant, cancellationToken).ConfigureAwait(false);
        await StreamTaskEventAsync(new TaskStatusUpdateEvent()
        {
            TaskId = task.Id,
            ContextId = task.ContextId,
            Status = task.Status,
            Final = true
        }, cancellationToken).ConfigureAwait(false);
        return task;
    }

    /// <inheritdoc/>
    public async IAsyncEnumerable<StreamResponse> SubscribeToTaskAsync(string id, string? tenant = null, [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(id);
        var task = await store.GetTaskAsync(id, tenant, cancellationToken).ConfigureAwait(false) ?? throw new A2AException(ErrorCode.TaskNotFound, $"Failed to find a task with the specified id '{id}'");
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
    public async Task<TaskPushNotificationConfig> SetTaskPushNotificationConfigAsync(SetTaskPushNotificationConfigRequest request, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);
        if (agentCard.Capabilities?.PushNotifications != true)
        {
            logger.LogError("Push notifications are not supported by the agent.");
            throw new A2AException(ErrorCode.PushNotificationNotSupported);
        }
        return await store.SetTaskPushNotificationConfigAsync(request, cancellationToken).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public async Task<TaskPushNotificationConfig> GetTaskPushNotificationConfigAsync(string taskId, string configId, string? tenant = null, CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(taskId);
        ArgumentException.ThrowIfNullOrWhiteSpace(configId);
        if (agentCard.Capabilities?.PushNotifications != true)
        {
            logger.LogError("Push notifications are not supported by the agent.");
            throw new A2AException(ErrorCode.PushNotificationNotSupported);
        }
        return await store.GetTaskPushNotificationConfigAsync(taskId, configId, tenant, cancellationToken).ConfigureAwait(false) ?? throw new A2AException(ErrorCode.TaskNotFound, $"Failed to find a push notification configuration with the specified id '{configId}' for the task with id '{taskId}'");
    }

    /// <inheritdoc/>
    public Task<TaskPushNotificationConfigQueryResult> ListTaskPushNotificationConfigAsync(TaskPushNotificationConfigQueryOptions queryOptions, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(queryOptions);
        if (agentCard.Capabilities?.PushNotifications != true)
        {
            logger.LogError("Push notifications are not supported by the agent.");
            throw new A2AException(ErrorCode.PushNotificationNotSupported);
        }
        return store.ListTaskPushNotificationConfigAsync(queryOptions, cancellationToken);
    }

    /// <inheritdoc/>
    public Task DeletePushNotificationConfigAsync(string taskId, string configId, string? tenant = null, CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(taskId);
        ArgumentException.ThrowIfNullOrWhiteSpace(configId);
        if (agentCard.Capabilities?.PushNotifications != true)
        {
            logger.LogError("Push notifications are not supported by the agent.");
            throw new A2AException(ErrorCode.PushNotificationNotSupported);
        }
        return store.DeleteTaskPushNotificationConfigAsync(taskId, configId, tenant, cancellationToken);
    }

    /// <inheritdoc/>
    public Task<AgentCard> GetExtendedAgentCardAsync(CancellationToken cancellationToken = default) => Task.FromResult(serviceProvider.GetKeyedService<AgentCard>(A2AServerDefaults.ExtendedAgentCardServiceKey) ?? serviceProvider.GetRequiredKeyedService<AgentCard>(null));

    /// <inheritdoc/>
    public async Task ExecuteTaskAsync(string taskId, string? tenant = null, CancellationToken cancellationToken = default)
    {
        var task = await store.GetTaskAsync(taskId, tenant, cancellationToken).ConfigureAwait(false) ?? throw new NullReferenceException($"Failed to find a task with the specified id '{taskId}'.");
        try
        {
            if (task.Status.State != TaskState.Submitted) task = await UpdateTaskStatusAsync(task, tenant, new()
                {
                    Timestamp = DateTime.Now,
                    State = TaskState.Submitted
                }, false, cancellationToken).ConfigureAwait(false);
            var working = false;
            await foreach (var e in agent.ExecuteAsync(task, cancellationToken).ConfigureAwait(false))
            {
                if (!working)
                {
                    if (e is not TaskStatusUpdateEvent statusUpdate || statusUpdate.Status.State != TaskState.Working) task = await UpdateTaskStatusAsync(task, tenant, new()
                    {
                        Timestamp = DateTime.Now,
                        State = TaskState.Working
                    }, false, cancellationToken).ConfigureAwait(false);
                    working = true;
                }
                switch (e)
                {
                    case TaskArtifactUpdateEvent artifactUpdateEvent:
                        if (artifactUpdateEvent.Append == true)
                        {
                            var artifact = task.Artifacts?.FirstOrDefault(a => a.ArtifactId == artifactUpdateEvent.Artifact.ArtifactId) ?? throw new NullReferenceException($"Failed to find an artifact with id '{artifactUpdateEvent.Artifact.ArtifactId}' in the task with id '{task.Id}'.");
                            artifact.Parts = [.. artifact.Parts, .. artifactUpdateEvent.Artifact.Parts];
                            artifact.Metadata = artifactUpdateEvent.Metadata ?? artifact.Metadata;
                        }
                        else
                        {
                            task.Artifacts ??= [];
                            task.Artifacts.Add(artifactUpdateEvent.Artifact);
                        }
                        task = await store.UpdateTaskAsync(task, tenant, cancellationToken).ConfigureAwait(false);
                        await StreamTaskEventAsync(artifactUpdateEvent, cancellationToken).ConfigureAwait(false);
                        break;
                    case TaskStatusUpdateEvent statusUpdateEvent:
                        await UpdateTaskStatusAsync(task, tenant, statusUpdateEvent.Status, statusUpdateEvent.Final, cancellationToken).ConfigureAwait(false);
                        break;
                }
            }
            if (task.Status.State == TaskState.Working) task = await CompleteAsync(task, tenant, cancellationToken).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            if (ex is OperationCanceledException)
            {
                task = await store.GetTaskAsync(taskId, tenant, cancellationToken).ConfigureAwait(false) ?? throw new NullReferenceException($"Failed to find a task with the specified id '{taskId}' during error handling.");
                if (task.Status.State == TaskState.Cancelled) return;
            }
            task = await FailAsync(task, tenant, new()
            {
                Role = Role.Agent,
                Parts =
                [
                    new TextPart()
                    {
                        Text = $"An error occurred while executing the task with id '{task.Id}': {ex}"
                    }
                ]
            }, cancellationToken).ConfigureAwait(false);
        }
    }

    async Task<Models.Task> UpdateTaskStatusAsync(Models.Task task, string? tenant, Models.TaskStatus status, bool final, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(task);
        task.History ??= [];
        if (status.Message != null) task.History.Add(status.Message);
        task.Status = status;
        task = await store.UpdateTaskAsync(task, tenant, cancellationToken).ConfigureAwait(false);
        await StreamTaskEventAsync(new TaskStatusUpdateEvent()
        {
            TaskId = task.Id,
            ContextId = task.ContextId,
            Status = task.Status,
            Final = final
        }, cancellationToken).ConfigureAwait(false);
        return task;
    }

    Task<Models.Task> CompleteAsync(Models.Task task, string? tenant, CancellationToken cancellationToken)
    {
       return UpdateTaskStatusAsync(task, tenant, new()
        {
            Timestamp = DateTime.UtcNow,
            State = TaskState.Completed
        }, true, cancellationToken);
    }

    Task<Models.Task> FailAsync(Models.Task task, string? tenant, Message? message, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(task);
        return UpdateTaskStatusAsync(task, tenant, new()
        {
            Timestamp = DateTime.UtcNow,
            State = TaskState.Failed,
            Message = message
        }, true, cancellationToken);
    }

    async Task StreamTaskEventAsync(TaskEvent e, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(e);
        var response = e switch
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
        taskEvents.OnNext(e);
        var pushNotificationConfigs = await store.ListTaskPushNotificationConfigAsync(new()
        {
            TaskId = e.TaskId
        }, cancellationToken).ConfigureAwait(false);
        var pushNotificationTasks = pushNotificationConfigs.Configs.Select(c => pushNotificationSender.SendPushNotificationAsync(c.PushNotificationConfig.Url, response, cancellationToken));
        await Task.WhenAll(pushNotificationTasks).ConfigureAwait(false);
    }

}
