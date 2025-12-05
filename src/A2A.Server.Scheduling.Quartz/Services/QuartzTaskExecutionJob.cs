namespace A2A.Server.Services;

/// <summary>
/// Represents a Quartz job used to execute an A2A task.
/// </summary>
/// <param name="store">The service used to store and retrieve A2A-specific resources.</param>
/// <param name="agent">The service used to interact with an A2A agent.</param>
public sealed class QuartzTaskExecutionJob(IStateStore store, IAgentRuntime agent)
    : IJob
{

    /// <summary>
    /// Gets the key of the job data used to store the task identifier.
    /// </summary>
    public const string TaskId = "taskId";

    /// <inheritdoc />
    public async Task Execute(IJobExecutionContext context)
    {
        var taskId = context.MergedJobDataMap.GetString(TaskId) ?? throw new NullReferenceException($"The required '{TaskId}' job data is missing.");
        var task = await store.GetTaskAsync(taskId, context.CancellationToken).ConfigureAwait(false) ?? throw new NullReferenceException($"Failed to find a task with the specified id '{taskId}'.");
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
                task = await store.UpdateTaskAsync(task, context.CancellationToken).ConfigureAwait(false);
                await NotifyTaskStatusUpdateAsync(task, context.CancellationToken).ConfigureAwait(false);
            }
            var working = false;
            await foreach (var e in agent.ExecuteAsync(task, context.CancellationToken).ConfigureAwait(false))
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
                    task = await store.UpdateTaskAsync(task, context.CancellationToken).ConfigureAwait(false);
                    await NotifyTaskStatusUpdateAsync(task, context.CancellationToken).ConfigureAwait(false);
                    working = true;
                }
                switch (e)
                {
                    case Models.TaskArtifactUpdateEvent artifactUpdateEvent:
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
                        task = await store.UpdateTaskAsync(task, context.CancellationToken).ConfigureAwait(false);
                        await NotifyTaskArtifactUpdateAsync(task, context.CancellationToken).ConfigureAwait(false);
                        break;
                    case Models.TaskStatusUpdateEvent statusUpdateEvent:
                        task = await store.UpdateTaskAsync(task with
                        {
                            Status = statusUpdateEvent.Status

                        }, context.CancellationToken).ConfigureAwait(false);
                        await NotifyTaskStatusUpdateAsync(task, context.CancellationToken).ConfigureAwait(false);
                        break;
                }
            }
            task = await CompleteAsync(task, context.CancellationToken).ConfigureAwait(false);
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
            }, context.CancellationToken).ConfigureAwait(false);
        }
    }

}