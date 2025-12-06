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

namespace A2A.Server.Services;

/// <summary>
/// Represents the Quartz implementation of the <see cref="IA2ATaskQueue"/> interface.
/// </summary>
public sealed class QuartzTaskQueue(ISchedulerFactory schedulerFactory)
    : IA2ATaskQueue
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
