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
/// Represents an in-memory implementation of the <see cref="IA2ATaskQueue"/> interface.
/// </summary>
/// <param name="serviceProvider">The A2A server instance.</param>
public sealed class MemoryTaskQueue(IServiceProvider serviceProvider)
    : IA2ATaskQueue
{

    readonly ConcurrentDictionary<string, CancellationTokenSource> runningTasks = [];

    /// <inheritdoc/>
    public Task EnqueueAsync(Models.Task task, string? tenant = null, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(task);
        var cancellationTokenSource = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
        runningTasks.TryAdd(GetCacheKey(task.Id, tenant), cancellationTokenSource);
        _ = serviceProvider.GetRequiredService<IA2AServer>().ExecuteTaskAsync(task.Id, tenant, cancellationTokenSource.Token);
        return Task.CompletedTask;
    }

    /// <inheritdoc/>
    public async Task CancelAsync(Models.Task task, string? tenant = null, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(task);
        if (!runningTasks.TryRemove(GetCacheKey(task.Id, tenant), out var cancellationTokenSource)) return;
        await cancellationTokenSource.CancelAsync();
        cancellationTokenSource.Dispose();
    }

    static string GetCacheKey(string taskId, string? tenant) => $"{(string.IsNullOrWhiteSpace(tenant) ? string.Empty : $"{tenant}:")}{taskId}";

}
