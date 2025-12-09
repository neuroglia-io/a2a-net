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

using System.Collections.Concurrent;
using Microsoft.Extensions.DependencyInjection;

namespace A2A.IntegrationTests.Cases.TaskQueues;

public sealed class MemoryTaskQueueIntegrationTests
    : A2ATaskQueueIntegrationTests<MemoryTaskQueue>
{

    ServiceProvider serviceProvider = null!;
    FakeA2AServer server = null!;

    protected override Task<(MemoryTaskQueue Queue, ITaskQueueProbe Probe)> CreateAsync()
    {
        var services = new ServiceCollection();

        server = new FakeA2AServer();
        services.AddSingleton<IA2AServer>(server);

        serviceProvider = services.BuildServiceProvider();

        var queue = new MemoryTaskQueue(serviceProvider);
        var probe = new MemoryProbe(server);

        return Task.FromResult((queue, (ITaskQueueProbe)probe));
    }

    public override Task DisposeAsync()
    {
        if (serviceProvider is not null) serviceProvider.Dispose();
        return Task.CompletedTask;
    }

    sealed class MemoryProbe(FakeA2AServer server)
        : ITaskQueueProbe
    {

        public async Task AssertEnqueuedAsync(string taskId, string? tenant = null)
        {
            var key = GetKey(taskId, tenant);
            var tcs = server.GetOrCreateEnqueuedTcs(key);
            var completed = await Task.WhenAny(tcs.Task, Task.Delay(TimeSpan.FromSeconds(2)));
            if (!ReferenceEquals(completed, tcs.Task)) throw new TimeoutException($"Expected ExecuteTaskAsync to be called for '{key}'.");
        }

        public async Task AssertCancelledAsync(string taskId, string? tenant = null)
        {
            var key = GetKey(taskId, tenant);
            var tcs = server.GetOrCreateCancelledTcs(key);
            var completed = await Task.WhenAny(tcs.Task, Task.Delay(TimeSpan.FromSeconds(2)));
            if (!ReferenceEquals(completed, tcs.Task)) throw new TimeoutException($"Expected cancellation for '{key}'.");
        }

        static string GetKey(string taskId, string? tenant) => $"{(string.IsNullOrWhiteSpace(tenant) ? string.Empty : $"{tenant}:")}{taskId}";

    }

    sealed class FakeA2AServer
        : IA2AServer
    {

        readonly ConcurrentDictionary<string, TaskCompletionSource> enqueued = new(StringComparer.Ordinal);
        readonly ConcurrentDictionary<string, TaskCompletionSource> cancelled = new(StringComparer.Ordinal);

        public TaskCompletionSource GetOrCreateEnqueuedTcs(string key) => enqueued.GetOrAdd(key, _ => new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously));

        public TaskCompletionSource GetOrCreateCancelledTcs(string key) => cancelled.GetOrAdd(key, _ => new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously));

        public Task ExecuteTaskAsync(string taskId, string? tenant = null, CancellationToken cancellationToken = default)
        {
            var key = $"{(string.IsNullOrWhiteSpace(tenant) ? string.Empty : $"{tenant}:")}{taskId}";

            GetOrCreateEnqueuedTcs(key).TrySetResult();

            cancellationToken.Register(() => GetOrCreateCancelledTcs(key).TrySetResult());

            return Task.Delay(Timeout.InfiniteTimeSpan, cancellationToken);
        }

        public Task<Models.Response> SendMessageAsync(Models.SendMessageRequest request, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public IAsyncEnumerable<Models.StreamResponse> SendStreamingMessageAsync(Models.SendMessageRequest request, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<Models.Task> GetTaskAsync(string id, uint? historyLength = null, string? tenant = null, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<Models.TaskQueryResult> ListTasksAsync(Models.TaskQueryOptions? queryOptions = null, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<Models.Task> CancelTaskAsync(string id, string? tenant = null, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public IAsyncEnumerable<Models.StreamResponse> SubscribeToTaskAsync(string id, string? tenant = null, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<Models.TaskPushNotificationConfig> SetTaskPushNotificationConfigAsync(Models.SetTaskPushNotificationConfigRequest request, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<Models.TaskPushNotificationConfig> GetTaskPushNotificationConfigAsync(string taskId, string configId, string? tenant = null, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<Models.TaskPushNotificationConfigQueryResult> ListTaskPushNotificationConfigAsync(Models.TaskPushNotificationConfigQueryOptions queryOptions, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task DeletePushNotificationConfigAsync(string taskId, string configId, string? tenant = null, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

    }

}
