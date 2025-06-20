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

using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace A2A.Server.Infrastructure.Services;

/// <summary>
/// Represents the distributed cache implementation of the <see cref="ITaskRepository"/> interface
/// </summary>
/// <param name="cache">The service used to cache data</param>
public class DistributedCacheTaskRepository(IDistributedCache cache)
    : ITaskRepository
{

    /// <summary>
    /// Gets the service used to cache data
    /// </summary>
    protected IDistributedCache Cache { get; } = cache;

    /// <inheritdoc/>
    public virtual async Task<TaskRecord> AddAsync(TaskRecord task, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(task);
        var key = BuildCacheKey(task.Id);
        var json = JsonSerializer.Serialize(task);
        await Cache.SetStringAsync(key, json, cancellationToken).ConfigureAwait(false);
        return task;
    }

    /// <inheritdoc/>
    public virtual async Task<bool> ContainsAsync(string id, CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(id);
        return !string.IsNullOrWhiteSpace(await Cache.GetStringAsync(BuildCacheKey(id), cancellationToken).ConfigureAwait(false));
    }

    /// <inheritdoc/>
    public virtual async Task<TaskRecord?> GetAsync(string id, CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(id);
        var json = await Cache.GetStringAsync(BuildCacheKey(id), cancellationToken).ConfigureAwait(false);
        if (string.IsNullOrWhiteSpace(json)) return null;
        return JsonSerializer.Deserialize<TaskRecord>(json);
    }

    /// <inheritdoc/>
    public virtual async Task<TaskRecord> UpdateAsync(TaskRecord task, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(task);
        var key = BuildCacheKey(task.Id);
        var json = JsonSerializer.Serialize(task);
        await Cache.SetStringAsync(key, json, cancellationToken).ConfigureAwait(false);
        return task;
    }

    /// <inheritdoc/>
    public virtual async System.Threading.Tasks.Task DeleteAsync(string id, CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(id);
        await Cache.RemoveAsync(BuildCacheKey(id), cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Builds a new cache key for the specified task
    /// </summary>
    /// <param name="taskId">The id of the task to build a new cache key for</param>
    /// <returns>A new cache key</returns>
    protected virtual string BuildCacheKey(string taskId) => $"a2a-task:{taskId}";

}
