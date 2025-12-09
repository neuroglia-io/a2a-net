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

namespace A2A.IntegrationTests.Cases.TaskQueues;

public abstract class A2ATaskQueueIntegrationTests<TQueue>
    : IAsyncLifetime
    where TQueue : IA2ATaskQueue
{

    protected TQueue Queue { get; private set; } = default!;

    protected ITaskQueueProbe Probe { get; private set; } = default!;

    protected abstract Task<(TQueue Queue, ITaskQueueProbe Probe)> CreateAsync();

    public virtual async Task InitializeAsync()
    {
        (Queue, Probe) = await CreateAsync();
    }

    public virtual Task DisposeAsync() => Task.CompletedTask;

    [Fact]
    public async Task EnqueueAsync_Should_Enqueue()
    {
        var task = new Models.Task { Id = "task-1", ContextId = "context-1" };

        await Queue.EnqueueAsync(task, tenant: "tenant-a");

        await Probe.AssertEnqueuedAsync(task.Id, tenant: "tenant-a");
    }

    [Fact]
    public async Task CancelAsync_Should_Cancel()
    {
        var task = new Models.Task { Id = "task-2", ContextId = "context-2" };

        await Queue.EnqueueAsync(task, tenant: "tenant-a");
        await Probe.AssertEnqueuedAsync(task.Id, tenant: "tenant-a");

        await Queue.CancelAsync(task, tenant: "tenant-a");

        await Probe.AssertCancelledAsync(task.Id, tenant: "tenant-a");
    }

    [Fact]
    public async Task CancelAsync_Should_Be_Tenant_Scoped()
    {
        var task = new Models.Task { Id = "task-3", ContextId = "context-3" };

        await Queue.EnqueueAsync(task, tenant: "tenant-a");
        await Queue.EnqueueAsync(task, tenant: "tenant-b");

        await Probe.AssertEnqueuedAsync(task.Id, tenant: "tenant-a");
        await Probe.AssertEnqueuedAsync(task.Id, tenant: "tenant-b");

        await Queue.CancelAsync(task, tenant: "tenant-a");

        await Probe.AssertCancelledAsync(task.Id, tenant: "tenant-a");
        await Probe.AssertEnqueuedAsync(task.Id, tenant: "tenant-b");
    }

    protected interface ITaskQueueProbe
    {

        Task AssertEnqueuedAsync(string taskId, string? tenant = null);

        Task AssertCancelledAsync(string taskId, string? tenant = null);

    }

}
