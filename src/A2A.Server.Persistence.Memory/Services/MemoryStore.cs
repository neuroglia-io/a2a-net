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
/// Represents the in-memory implementation of the <see cref="IA2AStore"/> interface.
/// </summary>
public sealed class MemoryStore(IOptions<MemoryStateStoreOptions> options)
    : IA2AStore
{

    readonly ConcurrentDictionary<string, Models.Task> tasks = new(StringComparer.Ordinal);
    readonly ConcurrentDictionary<string, ConcurrentDictionary<string, Models.PushNotificationConfig>> pushNotificationConfigurations = new(StringComparer.Ordinal);

    /// <inheritdoc/>
    public Task<Models.Task> AddTaskAsync(Models.Task task, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(task);
        ArgumentException.ThrowIfNullOrWhiteSpace(task.Id);
        cancellationToken.ThrowIfCancellationRequested();
        if (!tasks.TryAdd(task.Id, task)) throw new InvalidOperationException($"A task with id '{task.Id}' already exists.");
        return Task.FromResult(task);
    }

    /// <inheritdoc/>
    public Task<Models.Task?> GetTaskAsync(string id, CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(id);
        cancellationToken.ThrowIfCancellationRequested();
        tasks.TryGetValue(id, out var task);
        return Task.FromResult(task);
    }

    /// <inheritdoc/>
    public Task<Models.TaskQueryResult> ListTaskAsync(Models.TaskQueryOptions? queryOptions = null, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        queryOptions ??= new Models.TaskQueryOptions();
        var pageSize = (int)(queryOptions.PageSize is > 0 ? Math.Min(queryOptions.PageSize.Value, options.Value.MaxPageSize) : options.Value.DefaultPageSize);
        var offset = DecodeOffsetToken(queryOptions.PageToken);
        IEnumerable<Models.Task> query = tasks.Values;
        if (!string.IsNullOrWhiteSpace(queryOptions.ContextId)) query = query.Where(t => StringComparer.Ordinal.Equals(t.ContextId, queryOptions.ContextId));
        if (!string.IsNullOrWhiteSpace(queryOptions.Status)) query = query.Where(t => StringComparer.Ordinal.Equals(t.Status?.State.ToString(), queryOptions.Status));
        if (queryOptions.LastUpdateAfter is { } after) query = query.Where(t =>
        {
            var timestamp = t.Status?.Timestamp is DateTime dateTime ? new DateTimeOffset(DateTime.SpecifyKind(dateTime, dateTime.Kind == DateTimeKind.Unspecified ? DateTimeKind.Utc : dateTime.Kind)).ToUnixTimeSeconds() : 0;
            return timestamp > after;
        });
        query = query.OrderByDescending(GetSortTimeSeconds);
        var total = query.LongCount();
        var page = query.Skip(offset).Take(pageSize).Select(t => t.Project(queryOptions)).ToList();
        var nextOffset = offset + page.Count;
        var nextToken = nextOffset < total ? EncodeOffsetToken(nextOffset) : string.Empty;
        return Task.FromResult(new Models.TaskQueryResult
        {
            Tasks = page,
            NextPageToken = nextToken,
            PageSize = (uint)pageSize,
            TotalSize = (uint)Math.Min(uint.MaxValue, total)
        });
    }

    /// <inheritdoc/>
    public Task<Models.Task> UpdateTaskAsync(Models.Task task, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(task);
        ArgumentException.ThrowIfNullOrWhiteSpace(task.Id);
        cancellationToken.ThrowIfCancellationRequested();
        tasks.AddOrUpdate(task.Id,_ => throw new InvalidOperationException($"Failed to find a task with id '{task.Id}'."), (_, __) => task);
        return Task.FromResult(task);
    }

    /// <inheritdoc/>
    public Task<Models.PushNotificationConfig?> GetPushNotificationConfigAsync(string taskId, string configId, CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(taskId);
        ArgumentException.ThrowIfNullOrWhiteSpace(configId);
        cancellationToken.ThrowIfCancellationRequested();
        if (pushNotificationConfigurations.TryGetValue(taskId, out var configs) && configs.TryGetValue(configId, out var config)) return Task.FromResult<Models.PushNotificationConfig?>(config);
        return Task.FromResult<Models.PushNotificationConfig?>(null);
    }

    /// <inheritdoc/>
    public Task<Models.PushNotificationConfigQueryResult> ListPushNotificationConfigAsync(Models.PushNotificationConfigQueryOptions? queryOptions = null, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        queryOptions ??= new Models.PushNotificationConfigQueryOptions();
        var pageSize = (int)(queryOptions.PageSize is > 0 ? Math.Min(queryOptions.PageSize.Value, options.Value.MaxPageSize) : options.Value.DefaultPageSize);
        var offset = DecodeOffsetToken(queryOptions.PageToken);
        IEnumerable<Models.PushNotificationConfig> query;
        if (!string.IsNullOrWhiteSpace(queryOptions.TaskId))
        {
            var taskId = queryOptions.TaskId!;
            if (pushNotificationConfigurations.TryGetValue(taskId, out var dict)) query = dict.Values;
            else query = [];
        }
        else
        {
            query = pushNotificationConfigurations.Values.SelectMany(d => d.Values);
        }
        query = query.OrderBy(c => c.Id, StringComparer.Ordinal);
        var total = query.LongCount();
        var page = query.Skip(offset).Take(pageSize).ToList();
        var nextOffset = offset + page.Count;
        var nextToken = nextOffset < total ? EncodeOffsetToken(nextOffset) : null;
        return Task.FromResult(new Models.PushNotificationConfigQueryResult
        {
            Configs = page,
            NextPageToken = nextToken
        });
    }

    /// <inheritdoc/>
    public Task<Models.PushNotificationConfig> SetOrUpdatePushNotificationConfigAsync(string taskId, Models.PushNotificationConfig config, CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(taskId);
        ArgumentNullException.ThrowIfNull(config);
        ArgumentException.ThrowIfNullOrWhiteSpace(config.Id);
        cancellationToken.ThrowIfCancellationRequested();
        var configurationsPerTask = pushNotificationConfigurations.GetOrAdd(taskId, _ => new ConcurrentDictionary<string, Models.PushNotificationConfig>(StringComparer.Ordinal));
        configurationsPerTask[config.Id] = config;
        return Task.FromResult(config);
    }

    /// <inheritdoc/>
    public Task<bool> DeletePushNotificationConfigAsync(string taskId, string configId, CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(taskId);
        ArgumentException.ThrowIfNullOrWhiteSpace(configId);
        cancellationToken.ThrowIfCancellationRequested();
        if (!pushNotificationConfigurations.TryGetValue(taskId, out var configurationsPerTask)) return Task.FromResult(false);
        var removed = configurationsPerTask.TryRemove(configId, out _);
        if (configurationsPerTask.IsEmpty) pushNotificationConfigurations.TryRemove(taskId, out _);
        return Task.FromResult(removed);
    }

    static long GetSortTimeSeconds(Models.Task task)
    {
        if (task.Status?.Timestamp is not DateTime dateTime) return 0;
        if (dateTime.Kind == DateTimeKind.Unspecified) dateTime = DateTime.SpecifyKind(dateTime, DateTimeKind.Utc);
        return new DateTimeOffset(dateTime.ToUniversalTime()).ToUnixTimeSeconds();
    }

    static string EncodeOffsetToken(int offset) => Base64UrlEncode(Encoding.UTF8.GetBytes(offset.ToString()));

    static int DecodeOffsetToken(string? token)
    {
        if (string.IsNullOrWhiteSpace(token)) return 0;
        try
        {
            var bytes = Base64UrlDecode(token);
            var decoded = Encoding.UTF8.GetString(bytes);
            return int.TryParse(decoded, out var n) && n >= 0 ? n : 0;
        }
        catch { return 0; }
    }

    static string Base64UrlEncode(byte[] bytes)
    {
        var output = Convert.ToBase64String(bytes);
        return output.TrimEnd('=').Replace('+', '-').Replace('/', '_');
    }

    static byte[] Base64UrlDecode(string input)
    {
        input = input.Replace('-', '+').Replace('_', '/');
        switch (input.Length % 4)
        {
            case 2: input += "=="; break;
            case 3: input += "="; break;
        }
        return Convert.FromBase64String(input);
    }

}
