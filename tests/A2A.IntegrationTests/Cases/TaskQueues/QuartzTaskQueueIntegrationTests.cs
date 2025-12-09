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

using Quartz;
using Quartz.Impl;

namespace A2A.IntegrationTests.Cases.TaskQueues;

public sealed class QuartzTaskQueueIntegrationTests
    : A2ATaskQueueIntegrationTests<QuartzTaskQueue>
{

    IScheduler scheduler = null!;
    ISchedulerFactory schedulerFactory = null!;

    protected override async Task<(QuartzTaskQueue Queue, ITaskQueueProbe Probe)> CreateAsync()
    {
        schedulerFactory = new StdSchedulerFactory(new System.Collections.Specialized.NameValueCollection
        {
            ["quartz.scheduler.instanceName"] = $"A2A-Tests-{Guid.NewGuid():N}",
            ["quartz.threadPool.type"] = "Quartz.Simpl.SimpleThreadPool, Quartz",
            ["quartz.threadPool.threadCount"] = "2",
            ["quartz.threadPool.threadPriority"] = "Normal",
            ["quartz.jobStore.type"] = "Quartz.Simpl.RAMJobStore, Quartz"
        });

        scheduler = await schedulerFactory.GetScheduler();
        await scheduler.Start();

        var queue = new QuartzTaskQueue(new SingleSchedulerFactory(scheduler));
        var probe = new QuartzProbe(scheduler);

        return (queue, probe);
    }

    public override async Task DisposeAsync()
    {
        if (scheduler is not null) await scheduler.Shutdown(waitForJobsToComplete: false);
    }

    sealed class QuartzProbe(IScheduler scheduler)
        : ITaskQueueProbe
    {

        public async Task AssertEnqueuedAsync(string taskId, string? tenant = null)
        {
            var jobKey = new JobKey(GetJobKey(taskId, tenant));
            var ok = await scheduler.CheckExists(jobKey);
            if (!ok) throw new InvalidOperationException($"Expected Quartz job '{jobKey}' to exist.");
        }

        public async Task AssertCancelledAsync(string taskId, string? tenant = null)
        {
            var jobKey = new JobKey(GetJobKey(taskId, tenant));
            var ok = await scheduler.CheckExists(jobKey);
            if (ok) throw new InvalidOperationException($"Expected Quartz job '{jobKey}' to be deleted.");
        }

        static string GetJobKey(string taskId, string? tenant) => $"{(string.IsNullOrWhiteSpace(tenant) ? string.Empty : $"{tenant}:")}{taskId}";

    }

    sealed class SingleSchedulerFactory(IScheduler scheduler)
        : ISchedulerFactory
    {

        public Task<IScheduler> GetScheduler(CancellationToken cancellationToken = default) => Task.FromResult(scheduler);

        public Task<IReadOnlyCollection<IScheduler>> GetAllSchedulers(CancellationToken cancellationToken = default) => Task.FromResult<IReadOnlyCollection<IScheduler>>([scheduler]);

        Task<IReadOnlyList<IScheduler>> ISchedulerFactory.GetAllSchedulers(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<IScheduler?> GetScheduler(string schedName, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }

}
