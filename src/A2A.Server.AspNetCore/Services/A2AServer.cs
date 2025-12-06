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
            var response = await agent.ProcessAsync(request.Message, cancellationToken).ConfigureAwait(false);
            if (response is Models.Task task)
            {
                task = await store.AddTaskAsync(task, cancellationToken).ConfigureAwait(false);
                await taskQueue.EnqueueAsync(task, cancellationToken).ConfigureAwait(false);
            }
            return response;
        }
        else
        {
            var task = await store.GetTaskAsync(request.Message.TaskId, cancellationToken).ConfigureAwait(false) ?? throw new A2AException(ErrorCode.TaskNotFound, $"Failed to find a task with the specified id '{request.Message.TaskId}'");
            if (task.Status.State is not TaskState.Unspecified and not TaskState.Submitted and not TaskState.AuthRequired and not TaskState.InputRequired)
            {
                logger.LogError("Failed to process the message for the task with id '{id}' because the task is in an unexpected state '{state}'", task.Id, task.Status.State);
                throw new A2AException(ErrorCode.UnsupportedOperation, $"Failed to process the message for the task with id '{task.Id}' because the task is in an unexpected state '{task.Status.State}'");
            }
            task = await store.UpdateTaskAsync(task with
            {
                History = task.History is null ? [request.Message] : task.History.Append(request.Message).ToList()
            }, cancellationToken).ConfigureAwait(false);
            await taskQueue.EnqueueAsync(task, cancellationToken).ConfigureAwait(false);
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
            if (task.Status.State is not TaskState.Unspecified and not TaskState.Submitted and not TaskState.AuthRequired and not TaskState.InputRequired)
            {
                logger.LogError("Failed to process the message for the task with id '{id}' because the task is in an unexpected state '{state}'", task.Id, task.Status.State);
                throw new A2AException(ErrorCode.UnsupportedOperation, $"Failed to process the message for the task with id '{task.Id}' because the task is in an unexpected state '{task.Status.State}'");
            }
            task = await store.UpdateTaskAsync(task with
            {
                History = task.History is null ? [request.Message] : task.History.Append(request.Message).ToList()
            }, cancellationToken).ConfigureAwait(false);
        }
        await taskQueue.EnqueueAsync(task, cancellationToken).ConfigureAwait(false);
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
        await taskQueue.CancelAsync(task, cancellationToken).ConfigureAwait(false);
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
        if (agentCard.Capabilities?.PushNotifications != true)
        {
            logger.LogError("Push notifications are not supported by the agent.");
            throw new A2AException(ErrorCode.PushNotificationNotSupported);
        }
        return await store.SetOrUpdatePushNotificationConfigAsync(taskId, config, cancellationToken).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public async Task<PushNotificationConfig> GetPushNotificationConfigAsync(string taskId, string configId, CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(taskId);
        ArgumentException.ThrowIfNullOrWhiteSpace(configId);
        if (agentCard.Capabilities?.PushNotifications != true)
        {
            logger.LogError("Push notifications are not supported by the agent.");
            throw new A2AException(ErrorCode.PushNotificationNotSupported);
        }
        return await store.GetPushNotificationConfigAsync(taskId, configId, cancellationToken).ConfigureAwait(false) ?? throw new A2AException(ErrorCode.TaskNotFound, $"Failed to find a push notification configuration with the specified id '{configId}' for the task with id '{taskId}'");
    }

    /// <inheritdoc/>
    public Task<PushNotificationConfigQueryResult> ListPushNotificationConfigAsync(PushNotificationConfigQueryOptions? queryOptions = null, CancellationToken cancellationToken = default)
    {
        if (agentCard.Capabilities?.PushNotifications != true)
        {
            logger.LogError("Push notifications are not supported by the agent.");
            throw new A2AException(ErrorCode.PushNotificationNotSupported);
        }
        return store.ListPushNotificationConfigAsync(queryOptions, cancellationToken);
    }

    /// <inheritdoc/>
    public Task<bool> DeletePushNotificationConfigAsync(string taskId, string configId, CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(taskId);
        ArgumentException.ThrowIfNullOrWhiteSpace(configId);
        if (agentCard.Capabilities?.PushNotifications != true)
        {
            logger.LogError("Push notifications are not supported by the agent.");
            throw new A2AException(ErrorCode.PushNotificationNotSupported);
        }
        return store.DeletePushNotificationConfigAsync(taskId, configId, cancellationToken);
    }

    /// <inheritdoc/>
    public Task<AgentCard> GetExtendedAgentCardAsync(CancellationToken cancellationToken = default) => Task.FromResult(serviceProvider.GetKeyedService<AgentCard>(A2AServerDefaults.ExtendedAgentCardServiceKey) ?? serviceProvider.GetRequiredKeyedService<AgentCard>(null));

    /// <inheritdoc/>
    public async Task ExecuteTaskAsync(string taskId, CancellationToken cancellationToken = default)
    {
        var task = await store.GetTaskAsync(taskId, cancellationToken).ConfigureAwait(false) ?? throw new NullReferenceException($"Failed to find a task with the specified id '{taskId}'.");
        try
        {
            if (task.Status.State != TaskState.Submitted)
            {
                task.History ??= [];
                if (task.Status.Message != null) task.History.Add(task.Status.Message);
                task.Status = new()
                {
                    Timestamp = DateTime.Now,
                    State = TaskState.Submitted
                };
                task = await store.UpdateTaskAsync(task, cancellationToken).ConfigureAwait(false);
                await StreamTaskEventAsync(new TaskStatusUpdateEvent()
                {
                    TaskId = task.Id,
                    ContextId = task.Id,
                    Status = task.Status
                }, cancellationToken).ConfigureAwait(false);
            }
            var working = false;
            await foreach (var e in agent.ExecuteAsync(task, cancellationToken).ConfigureAwait(false))
            {
                if (!working)
                {
                    task.History ??= [];
                    if (task.Status.Message != null) task.History.Add(task.Status.Message);
                    task.Status = new()
                    {
                        Timestamp = DateTime.Now,
                        State = TaskState.Working
                    };
                    task = await store.UpdateTaskAsync(task, cancellationToken).ConfigureAwait(false);
                    await StreamTaskEventAsync(new TaskStatusUpdateEvent()
                    {
                        TaskId = task.Id,
                        ContextId = task.Id,
                        Status = task.Status
                    }, cancellationToken).ConfigureAwait(false);
                    working = true;
                }
                switch (e)
                {
                    case TaskArtifactUpdateEvent artifactUpdateEvent:
                        if (artifactUpdateEvent.Append == true)
                        {
                            var artifact = task.Artifacts?.FirstOrDefault(a => a.ArtifactId == artifactUpdateEvent.Artifact.ArtifactId) ?? throw new NullReferenceException($"Failed to find an artifact with id '{artifactUpdateEvent.Artifact.ArtifactId}' in the task with id '{task.Id}'.");
                            artifact = artifact with
                            {
                                Parts = [.. artifact.Parts, .. artifactUpdateEvent.Artifact.Parts],
                                Metadata = (artifact.Metadata?.ToList() ?? [])
                                    .Concat(artifactUpdateEvent.Artifact.Metadata?.ToList() ?? [])
                                    .GroupBy(kv => kv.Key, StringComparer.Ordinal)
                                    .ToDictionary(g => g.Key, g => g.Last().Value, StringComparer.Ordinal)
                            };
                        }
                        else
                        {
                            task.Artifacts ??= [];
                            task.Artifacts.Add(artifactUpdateEvent.Artifact);
                        }
                        task = await store.UpdateTaskAsync(task, cancellationToken).ConfigureAwait(false);
                        await StreamTaskEventAsync(artifactUpdateEvent, cancellationToken).ConfigureAwait(false);
                        break;
                    case TaskStatusUpdateEvent statusUpdateEvent:
                        task = await store.UpdateTaskAsync(task with
                        {
                            Status = statusUpdateEvent.Status

                        }, cancellationToken).ConfigureAwait(false);
                        await StreamTaskEventAsync(statusUpdateEvent, cancellationToken).ConfigureAwait(false);
                        break;
                }
            }
            task = await CompleteAsync(task, cancellationToken).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            task = await FailAsync(task, new()
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

    async Task<Models.Task> CompleteAsync(Models.Task task, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(task);
        task.History ??= [];
        if (task.Status.Message != null) task.History.Add(task.Status.Message);
        task.Status = new()
        {
            Timestamp = DateTime.Now,
            State = TaskState.Completed
        };
        task = await store.UpdateTaskAsync(task, cancellationToken).ConfigureAwait(false);
        await StreamTaskEventAsync(new TaskStatusUpdateEvent()
        {
            TaskId = task.Id,
            ContextId = task.ContextId,
            Status = task.Status,
            Final = true
        }, cancellationToken).ConfigureAwait(false);
        return task;
    }

    async Task<Models.Task> FailAsync(Models.Task task, Message? message = null, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(task);
        task.History ??= [];
        if (task.Status.Message != null) task.History.Add(task.Status.Message);
        task.Status = new()
        {
            Timestamp = DateTime.Now,
            State = TaskState.Failed,
            Message = message
        };
        task = await store.UpdateTaskAsync(task, cancellationToken).ConfigureAwait(false);
        await StreamTaskEventAsync(new TaskStatusUpdateEvent()
        {
            TaskId = task.Id,
            ContextId = task.ContextId,
            Status = task.Status,
            Final = true
        }, cancellationToken).ConfigureAwait(false);
        return task;
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
        var pushNotificationConfigs = await store.ListPushNotificationConfigAsync(new()
        {
            TaskId = e.TaskId
        }, cancellationToken).ConfigureAwait(false);
        var pushNotificationTasks = pushNotificationConfigs.Configs.Select(c => pushNotificationSender.SendPushNotificationAsync(c.Url, response, cancellationToken));
        await Task.WhenAll(pushNotificationTasks).ConfigureAwait(false);
    }

}
