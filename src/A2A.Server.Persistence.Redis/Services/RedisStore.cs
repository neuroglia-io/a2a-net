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
/// Represents a Redis-based implementation of <see cref="IA2AStore"/>.
/// </summary>
/// <param name="options">The service used to access the options used to configure the <see cref="RedisStore"/>.</param>
/// <param name="connectionMultiplexer">The service used to interact with the Redis server to use.</param>
public sealed class RedisStore(IOptions<RedisStateStoreOptions> options, IConnectionMultiplexer connectionMultiplexer)
    : IA2AStore
{

    readonly IDatabase database = connectionMultiplexer.GetDatabase();

    /// <inheritdoc/>
    public async Task<Models.Task> AddTaskAsync(Models.Task task, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(task);
        var taskKey = GetTaskKey(task.Id);
        var metadataKey = GetTaskMetadataKey(task.Id);
        var timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        var state = task.Status?.State ?? TaskState.Unspecified;
        var contextId = task.ContextId ?? string.Empty;
        var json = JsonSerializer.Serialize(task, JsonSerializationContext.Default.Task);
        for (var attempt = 0; attempt < 10; attempt++)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var transaction = database.CreateTransaction();
            transaction.AddCondition(Condition.KeyNotExists(taskKey));
            var setTask = transaction.StringSetAsync(taskKey, json);
            var metaTask = transaction.HashSetAsync(metadataKey,
            [
                new HashEntry(TaskHashSetFields.State, state),
                new HashEntry(TaskHashSetFields.Created, timestamp),
                new HashEntry(TaskHashSetFields.Updated, timestamp),
                new HashEntry(TaskHashSetFields.Context, contextId)
            ]);
            var addToTaskCreatedIndexTask = transaction.SortedSetAddAsync(GetTasksCreatedIndexKey(), task.Id, timestamp);
            var addToTaskUpdateIndexTask = transaction.SortedSetAddAsync(GetTasksUpdatedIndexKey(), task.Id, timestamp);
            var addToTaskByStateIndexTask = transaction.SortedSetAddAsync(GetTasksByStateUpdatedIndexKey(state), task.Id, timestamp);
            Task? addToTaskByContextIndexTask = null;
            Task? addToTaskByContextAndStateIndexTask = null;
            if (!string.IsNullOrWhiteSpace(contextId))
            {
                addToTaskByContextIndexTask = transaction.SortedSetAddAsync(GetTasksByContextUpdatedIndexKey(contextId), task.Id, timestamp);
                addToTaskByContextAndStateIndexTask = transaction.SortedSetAddAsync(GetTasksByContextAndStateUpdatedIndexKey(contextId, state), task.Id, timestamp);
            }
            var ok = await transaction.ExecuteAsync().ConfigureAwait(false);
            if (ok)
            {
                await setTask.ConfigureAwait(false);
                await metaTask.ConfigureAwait(false);
                await addToTaskCreatedIndexTask.ConfigureAwait(false);
                await addToTaskUpdateIndexTask.ConfigureAwait(false);
                await addToTaskByStateIndexTask.ConfigureAwait(false);
                if (addToTaskByContextIndexTask is not null) await addToTaskByContextIndexTask.ConfigureAwait(false);
                if (addToTaskByContextAndStateIndexTask is not null) await addToTaskByContextAndStateIndexTask.ConfigureAwait(false);
                return task;
            }
        }
        throw new TimeoutException($"Failed to persist the task with id '{task.Id}' due to concurrency issues.");
    }

    /// <inheritdoc/>
    public async Task<Models.Task?> GetTaskAsync(string id, CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(id);
        cancellationToken.ThrowIfCancellationRequested();
        var key = GetTaskKey(id);
        var json = (await database.StringGetAsync(key).ConfigureAwait(false)).ToString();
        if (!string.IsNullOrWhiteSpace(json)) return JsonSerializer.Deserialize(json, JsonSerializationContext.Default.Task);
        await database.KeyDeleteAsync(GetTaskKey(id)).ConfigureAwait(false);
        return null;
    }

    /// <inheritdoc/>
    public async Task<Models.TaskPushNotificationConfig?> GetPushNotificationConfigAsync(string taskId, string configId, CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(taskId);
        ArgumentException.ThrowIfNullOrWhiteSpace(configId);
        cancellationToken.ThrowIfCancellationRequested();
        var key = GetPushNotificationConfigKey(taskId, configId);
        var json = (await database.StringGetAsync(key).ConfigureAwait(false)).ToString();
        if (!string.IsNullOrWhiteSpace(json)) return JsonSerializer.Deserialize(json, JsonSerializationContext.Default.TaskPushNotificationConfig);
        await database.KeyDeleteAsync(key).ConfigureAwait(false);
        return null;
    }

    /// <inheritdoc/>
    public async Task<Models.TaskQueryResult> ListTaskAsync(Models.TaskQueryOptions? queryOptions = null, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        queryOptions ??= new Models.TaskQueryOptions();
        var pageSize = ClampPageSize(queryOptions.PageSize);
        var cursor = TryDecodeCursor(queryOptions.PageToken);
        var contextId = queryOptions.ContextId;
        var status = queryOptions.Status ?? string.Empty;
        var useUpdated = queryOptions.LastUpdateAfter.HasValue || !string.IsNullOrWhiteSpace(status) || !string.IsNullOrWhiteSpace(contextId);
        var indexKey = useUpdated ? SelectUpdatedIndexKey(contextId, status) : GetTasksCreatedIndexKey();
        var minScore = queryOptions.LastUpdateAfter.HasValue ? queryOptions.LastUpdateAfter.Value : double.NegativeInfinity;
        var total = queryOptions.LastUpdateAfter.HasValue ? await database.SortedSetLengthAsync(indexKey, minScore, double.PositiveInfinity).ConfigureAwait(false) : await database.SortedSetLengthAsync(indexKey).ConfigureAwait(false);
        var fetchCount = (int)pageSize + 50;
        var entries = await database.SortedSetRangeByScoreWithScoresAsync(indexKey, minScore, double.PositiveInfinity, Exclude.None, Order.Descending, 0, fetchCount).ConfigureAwait(false);
        if (cursor is not null) entries = entries .SkipWhile(e => !(e.Score < cursor.Value.Score || (e.Score == cursor.Value.Score && StringComparer.Ordinal.Compare(e.Element.ToString(), cursor.Value.Member) < 0))) .ToArray();
        var pageEntries = entries.Take((int)pageSize).ToArray();
        var ids = pageEntries.Select(e => e.Element.ToString()).ToArray();
        var keys = ids.Select(id => GetTaskKey(id)).ToArray();
        var values = keys.Length == 0 ? Array.Empty<RedisValue>() : await database.StringGetAsync(keys).ConfigureAwait(false);
        var tasks = new List<Models.Task>(values.Length);
        for (var i = 0; i < values.Length; i++)
        {
            var json = values[i].ToString();
            if (string.IsNullOrWhiteSpace(json)) continue;
            var task = JsonSerializer.Deserialize(json, JsonSerializationContext.Default.Task);
            if (task is null) continue;
            task = task.Project(queryOptions);
            tasks.Add(task);
        }
        var nextPageToken = string.Empty;
        if (pageEntries.Length == pageSize)
        {
            var last = pageEntries[^1];
            nextPageToken = EncodeCursor(new Cursor(last.Score, last.Element.ToString()));
        }
        return new Models.TaskQueryResult
        {
            Tasks = tasks,
            NextPageToken = nextPageToken,
            PageSize = pageSize,
            TotalSize = (uint)Math.Min(uint.MaxValue, total)
        };
    }

    /// <inheritdoc/>
    public async Task<Models.PushNotificationConfigQueryResult> ListPushNotificationConfigAsync(Models.PushNotificationConfigQueryOptions? queryOptions = null, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        queryOptions ??= new Models.PushNotificationConfigQueryOptions();
        var pageSize = ClampPageSize(queryOptions.PageSize);
        var cursor = TryDecodeCursor(queryOptions.PageToken);
        var hasTaskFilter = !string.IsNullOrWhiteSpace(queryOptions.TaskId);
        RedisKey indexKey;
        Func<string, (string TaskId, string ConfigId)> parseGlobalKey;
        if (hasTaskFilter)
        {
            var taskId = queryOptions.TaskId!;
            indexKey = GetPushNotificationConfigByTaskIndexKey(taskId);
            parseGlobalKey = pushNotificationConfigId => (taskId, pushNotificationConfigId);
        }
        else
        {
            indexKey = GetPushNotificationConfigIndexKey();
            parseGlobalKey = ParsePushNotificationConfigGlobalKey;
        }
        var fetchCount = (int)pageSize + 50;
        var entries = await database.SortedSetRangeByRankWithScoresAsync(indexKey, 0, fetchCount - 1,order: Order.Descending).ConfigureAwait(false);
        if (cursor is not null) entries = [.. entries.SkipWhile(e => !(e.Score < cursor.Value.Score ||(e.Score == cursor.Value.Score && StringComparer.Ordinal.Compare(e.Element.ToString(), cursor.Value.Member) < 0)))];
        var pageEntries = entries.Take((int)pageSize).ToArray();
        var pairs = pageEntries.Select(e => parseGlobalKey(e.Element.ToString())).Where(p => !string.IsNullOrWhiteSpace(p.TaskId) && !string.IsNullOrWhiteSpace(p.ConfigId)).ToArray();
        var keys = pairs.Select(p => GetPushNotificationConfigKey(p.TaskId, p.ConfigId)).ToArray();
        var values = keys.Length == 0? [] : await database.StringGetAsync(keys).ConfigureAwait(false);
        var pushNotificationConfigs = new List<Models.TaskPushNotificationConfig>(values.Length);
        foreach (var value in values)
        {
            var json = value.ToString();
            if (string.IsNullOrWhiteSpace(json)) continue;
            var pushNotificationConfig = JsonSerializer.Deserialize(json, JsonSerializationContext.Default.TaskPushNotificationConfig);
            if (pushNotificationConfig is not null) pushNotificationConfigs.Add(pushNotificationConfig);
        }
        string? nextPageToken = null;
        if (pageEntries.Length == pageSize)
        {
            var last = pageEntries[^1];
            nextPageToken = EncodeCursor(new Cursor(last.Score, last.Element.ToString()));
        }
        return new Models.PushNotificationConfigQueryResult
        {
            Configs = pushNotificationConfigs,
            NextPageToken = nextPageToken
        };
    }

    /// <inheritdoc/>
    public async Task<Models.Task> UpdateTaskAsync(Models.Task task, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(task);
        ArgumentException.ThrowIfNullOrWhiteSpace(task.Id);
        var taskKey = GetTaskKey(task.Id);
        var metadataKey = GetTaskMetadataKey(task.Id);
        var timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        var newState = task.Status?.State ?? TaskState.Unspecified;
        var newContextId = task.ContextId ?? string.Empty;
        var json = JsonSerializer.Serialize(task, JsonSerializationContext.Default.Task);
        for (var attempt = 0; attempt < 10; attempt++)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var meta = await database.HashGetAllAsync(metadataKey).ConfigureAwait(false);
            if (meta.Length == 0) throw new InvalidOperationException($"Failed to find a task with the specified id '{task.Id}'.");
            var oldStateStr = meta.FirstOrDefault(h => h.Name == TaskHashSetFields.State).Value.ToString();
            if (string.IsNullOrWhiteSpace(oldStateStr)) oldStateStr = TaskState.Unspecified.ToString();
            var oldContextId = meta.FirstOrDefault(h => h.Name == TaskHashSetFields.Context).Value.ToString();
            var oldUpdatedVal = meta.FirstOrDefault(h => h.Name == TaskHashSetFields.Updated).Value;
            var transaction = database.CreateTransaction();
            transaction.AddCondition(Condition.KeyExists(taskKey));
            if (!oldUpdatedVal.IsNullOrEmpty) transaction.AddCondition(Condition.HashEqual(metadataKey, TaskHashSetFields.Updated, oldUpdatedVal));
            var setTask = transaction.StringSetAsync(taskKey, json);
            var setMetadataTask = transaction.HashSetAsync(metadataKey,
            [
                new HashEntry(TaskHashSetFields.State, newState),
                new HashEntry(TaskHashSetFields.Updated, timestamp),
                new HashEntry(TaskHashSetFields.Context, newContextId)
            ]);
            var updateIndexTask = transaction.SortedSetAddAsync(GetTasksUpdatedIndexKey(), task.Id, timestamp);
            _ = transaction.SortedSetRemoveAsync(GetTasksByStateUpdatedIndexKey(oldStateStr), task.Id);
            var updateByStateIndexTask = transaction.SortedSetAddAsync(GetTasksByStateUpdatedIndexKey(newState), task.Id, timestamp);
            Task? updateByContextIndexTask = null;
            Task? updateByContextAndStateIndexTask = null;
            if (!string.IsNullOrWhiteSpace(oldContextId))
            {
                _ = transaction.SortedSetRemoveAsync(GetTasksByContextUpdatedIndexKey(oldContextId), task.Id);
                _ = transaction.SortedSetRemoveAsync(GetTasksByContextAndStateUpdatedIndexKey(oldContextId, oldStateStr), task.Id);
            }
            if (!string.IsNullOrWhiteSpace(newContextId))
            {
                updateByContextIndexTask = transaction.SortedSetAddAsync(GetTasksByContextUpdatedIndexKey(newContextId), task.Id, timestamp);
                updateByContextAndStateIndexTask = transaction.SortedSetAddAsync(GetTasksByContextAndStateUpdatedIndexKey(newContextId, newState), task.Id, timestamp);
            }
            var ok = await transaction.ExecuteAsync().ConfigureAwait(false);
            if (ok)
            {
                await setTask.ConfigureAwait(false);
                await setMetadataTask.ConfigureAwait(false);
                await updateIndexTask.ConfigureAwait(false);
                await updateByStateIndexTask.ConfigureAwait(false);
                if (updateByContextIndexTask is not null) await updateByContextIndexTask.ConfigureAwait(false);
                if (updateByContextAndStateIndexTask is not null) await updateByContextAndStateIndexTask.ConfigureAwait(false);
                return task;
            }
        }
        throw new TimeoutException($"Failed to update the task with id '{task.Id}' due to concurrent writes.");
    }

    /// <inheritdoc/>
    public async Task<Models.TaskPushNotificationConfig> SetOrUpdatePushNotificationConfigAsync(Models.TaskPushNotificationConfig config, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(config);
        ArgumentException.ThrowIfNullOrWhiteSpace(config.PushNotificationConfig.Id);
        cancellationToken.ThrowIfCancellationRequested();
        var taskId = config.Name.Split('/')[1];
        var key = GetPushNotificationConfigKey(taskId, config.PushNotificationConfig.Id);
        var indexKey = GetPushNotificationConfigIndexKey();
        var byTaskIndexKey = GetPushNotificationConfigByTaskIndexKey(taskId);
        var json = JsonSerializer.Serialize(config, JsonSerializationContext.Default.PushNotificationConfig);
        var score = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        var globalMember = GetPushNotificationConfigGlobalKey(taskId, config.PushNotificationConfig.Id);
        for (var attempt = 0; attempt < 10; attempt++)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var transaction = database.CreateTransaction();
            var addTask = transaction.StringSetAsync(key, json);
            var addToIndexTask = transaction.SortedSetAddAsync(indexKey, globalMember, score);
            var addToByTaskIndexTask = transaction.SortedSetAddAsync(byTaskIndexKey, config.PushNotificationConfig.Id, score);
            if (await transaction.ExecuteAsync().ConfigureAwait(false))
            {
                await addTask.ConfigureAwait(false);
                await addToByTaskIndexTask.ConfigureAwait(false);
                await addToIndexTask.ConfigureAwait(false);
                return config;
            }
        }
        throw new TimeoutException($"Failed to upsert the push notification config with id '{config.PushNotificationConfig.Id}' for the task with id '{taskId}' due to concurrency issues.");
    }

    /// <inheritdoc/>
    public async Task<bool> DeletePushNotificationConfigAsync(string taskId, string configId, CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(taskId);
        ArgumentException.ThrowIfNullOrWhiteSpace(configId);
        cancellationToken.ThrowIfCancellationRequested();
        var globalMember = GetPushNotificationConfigGlobalKey(taskId, configId);
        var key = GetPushNotificationConfigKey(taskId, configId);
        var indexKey = GetPushNotificationConfigIndexKey();
        var byTaskIndexKey = GetPushNotificationConfigByTaskIndexKey(taskId);
        for (var attempt = 0; attempt < 10; attempt++)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var transaction = database.CreateTransaction();
            var deleteKeyTask = transaction.KeyDeleteAsync(key);
            _ = transaction.SortedSetRemoveAsync(indexKey, globalMember);
            _ = transaction.SortedSetRemoveAsync(byTaskIndexKey, configId);
            if (await transaction.ExecuteAsync().ConfigureAwait(false)) return await deleteKeyTask.ConfigureAwait(false);
        }
        throw new TimeoutException($"Failed to delete push notification config '{configId}' for task '{taskId}' due to concurrency issues.");
    }

    RedisKey GetTaskKey(string taskId) => $"{options.Value.KeyPrefix}task:{taskId}";

    RedisKey GetTaskMetadataKey(string taskId) => $"{options.Value.KeyPrefix}task-metadata:{taskId}";

    RedisKey GetTasksCreatedIndexKey() => $"{options.Value.KeyPrefix}task:index:created";

    RedisKey GetTasksUpdatedIndexKey() => $"{options.Value.KeyPrefix}task:index:updated";

    RedisKey GetTasksByStateUpdatedIndexKey(string state) => $"{options.Value.KeyPrefix}task:index:status:{state}:updated";

    RedisKey GetTasksByContextUpdatedIndexKey(string contextId) => $"{options.Value.KeyPrefix}task:index:context:{contextId}:updated";

    RedisKey GetTasksByContextAndStateUpdatedIndexKey(string contextId, string state) => $"{options.Value.KeyPrefix}task:index:context:{contextId}:status:{state}:updated";

    RedisKey GetPushNotificationConfigKey(string taskId, string pushNotificationConfigId) => $"{options.Value.KeyPrefix}push-notification-config:{taskId}:{pushNotificationConfigId}";

    RedisKey GetPushNotificationConfigIndexKey() => $"{options.Value.KeyPrefix}push-notification-config:index";

    RedisKey GetPushNotificationConfigByTaskIndexKey(string taskId) => $"{options.Value.KeyPrefix}push-notification-config:index:task:{taskId}";

    RedisKey SelectUpdatedIndexKey(string? contextId, string status)
    {
        if (!string.IsNullOrWhiteSpace(contextId) && !string.IsNullOrWhiteSpace(status)) return GetTasksByContextAndStateUpdatedIndexKey(contextId!, status);
        if (!string.IsNullOrWhiteSpace(contextId)) return GetTasksByContextUpdatedIndexKey(contextId!);
        if (!string.IsNullOrWhiteSpace(status)) return GetTasksByStateUpdatedIndexKey(status);
        return GetTasksUpdatedIndexKey();
    }

    static string GetPushNotificationConfigGlobalKey(string taskId, string configId) => $"{taskId}:{configId}";

    static (string TaskId, string ConfigId) ParsePushNotificationConfigGlobalKey(string member)
    {
        var i = member.IndexOf(':');
        if (i <= 0 || i == member.Length - 1) return (string.Empty, string.Empty);
        var t = member[..i];
        var c = member[(i + 1)..];
        return (t, c);
    }

    uint ClampPageSize(uint? requested)
    {
        var pageSize = requested ?? options.Value.DefaultPageSize;
        if (pageSize == 0) pageSize = options.Value.DefaultPageSize;
        if (pageSize > options.Value.MaxPageSize) pageSize = options.Value.MaxPageSize;
        return pageSize;
    }

    static string EncodeCursor(Cursor c) => Base64UrlEncode(Encoding.UTF8.GetBytes($"{c.Score:R}:{c.Member}"));

    static Cursor? TryDecodeCursor(string? token)
    {
        if (string.IsNullOrWhiteSpace(token)) return null;
        try
        {
            var raw = Encoding.UTF8.GetString(Base64UrlDecode(token));
            var i = raw.IndexOf(':');
            if (i <= 0) return null;
            var scoreStr = raw[..i];
            var member = raw[(i + 1)..];
            if (string.IsNullOrWhiteSpace(member)) return null;
            if (!double.TryParse(scoreStr, NumberStyles.Float, CultureInfo.InvariantCulture, out var score)) return null;
            return new Cursor(score, member);
        }
        catch { return null; }
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

    static class TaskHashSetFields
    {

        public const string State = "state";
        public const string Created = "created";
        public const string Updated = "updated";
        public const string Context = "context";

    }

    readonly record struct Cursor(double Score, string Member);

}
