namespace A2A.Server.Services;

/// <summary>
/// Represents the Quartz implementation of the <see cref="ITaskQueue"/> interface.
/// </summary>
public sealed class QuartzTaskQueue(ISchedulerFactory schedulerFactory)
    : ITaskQueue
{

    /// <inheritdoc/>
    public async Task EnqueueAsync(Models.Task task, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(task);
        var job = JobBuilder.Create<QuartzTaskExecutionJob>().WithIdentity(task.Id).UsingJobData(QuartzTaskExecutionJob.TaskId, task.Id).Build();
        var trigger = TriggerBuilder.Create().StartNow().Build();
        var scheduler = await schedulerFactory.GetScheduler(cancellationToken).ConfigureAwait(false);
        await scheduler.ScheduleJob(job, trigger, cancellationToken).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public async Task CancelAsync(Models.Task task, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(task);
        var scheduler = await schedulerFactory.GetScheduler(cancellationToken).ConfigureAwait(false);
        await scheduler.DeleteJob(new JobKey(task.Id), cancellationToken).ConfigureAwait(false);
    }

}
