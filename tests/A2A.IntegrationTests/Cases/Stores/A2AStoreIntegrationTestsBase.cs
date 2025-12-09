namespace A2A.IntegrationTests.Cases.Stores;

using A2A.Server.Services;
using System.Threading.Tasks;
using Xunit;

public abstract class A2AStoreIntegrationTestsBase<TStore>
    : IAsyncLifetime
    where TStore : class, IA2AStore
{

    protected TStore Store { get; private set; } = default!;

    protected abstract Task<TStore> CreateStoreAsync();

    public virtual async Task InitializeAsync()
    {
        Store = await CreateStoreAsync();
    }

    public virtual Task DisposeAsync() => Task.CompletedTask;

    [Fact]
    public async Task AddTaskAsync_ShouldStoreAndGetTaskPerTenant()
    {
        var taskA = new Models.Task { Id = "task-1", ContextId = "ctx", Status = new Models.TaskStatus { State = TaskState.Submitted, Timestamp = DateTime.UtcNow } };
        var taskB = new Models.Task { Id = "task-1", ContextId = "ctx", Status = new Models.TaskStatus { State = TaskState.Submitted, Timestamp = DateTime.UtcNow } };

        _ = await Store.AddTaskAsync(taskA, tenant: "tenant-a");
        _ = await Store.AddTaskAsync(taskB, tenant: "tenant-b");

        var a = await Store.GetTaskAsync("task-1", tenant: "tenant-a");
        var b = await Store.GetTaskAsync("task-1", tenant: "tenant-b");
        var missing = await Store.GetTaskAsync("task-1", tenant: "tenant-c");

        Assert.NotNull(a);
        Assert.NotNull(b);
        Assert.Null(missing);
        Assert.Equal("task-1", a!.Id);
        Assert.Equal("task-1", b!.Id);
    }

    [Fact]
    public async Task AddTaskAsync_SameIdSameTenant_ShouldThrow()
    {
        var taskA1 = new Models.Task { Id = "dup-1", ContextId = "ctx", Status = new Models.TaskStatus { State = TaskState.Submitted, Timestamp = DateTime.UtcNow } };
        var taskA2 = new Models.Task { Id = "dup-1", ContextId = "ctx", Status = new Models.TaskStatus { State = TaskState.Submitted, Timestamp = DateTime.UtcNow } };

        _ = await Store.AddTaskAsync(taskA1, tenant: "tenant-a");

        await Assert.ThrowsAnyAsync<Exception>(async () =>
        {
            _ = await Store.AddTaskAsync(taskA2, tenant: "tenant-a");
        });
    }

    [Fact]
    public async Task ListTaskAsync_ShouldListOnlyTasksForTenant()
    {
        for (var i = 0; i < 5; i++)
        {
            _ = await Store.AddTaskAsync(new Models.Task
            {
                Id = $"tA-{i}",
                ContextId = "ctx",
                Status = new Models.TaskStatus { State = TaskState.Submitted, Timestamp = DateTime.UtcNow.AddSeconds(i) }
            }, tenant: "tenant-a");
        }

        for (var i = 0; i < 3; i++)
        {
            _ = await Store.AddTaskAsync(new Models.Task
            {
                Id = $"tB-{i}",
                ContextId = "ctx",
                Status = new Models.TaskStatus { State = TaskState.Submitted, Timestamp = DateTime.UtcNow.AddSeconds(i) }
            }, tenant: "tenant-b");
        }

        var resultA = await Store.ListTaskAsync(new Models.TaskQueryOptions { Tenant = "tenant-a", PageSize = 50 });
        var resultB = await Store.ListTaskAsync(new Models.TaskQueryOptions { Tenant = "tenant-b", PageSize = 50 });

        Assert.Equal(5, resultA.Tasks.Count);
        Assert.Equal(3, resultB.Tasks.Count);
        Assert.All(resultA.Tasks, t => Assert.StartsWith("tA-", t.Id, StringComparison.Ordinal));
        Assert.All(resultB.Tasks, t => Assert.StartsWith("tB-", t.Id, StringComparison.Ordinal));
    }

    [Fact]
    public async Task ListTaskAsync_ShouldPage()
    {
        for (var i = 0; i < 25; i++)
        {
            _ = await Store.AddTaskAsync(new Models.Task
            {
                Id = $"p-{i:D2}",
                ContextId = "ctx",
                Status = new Models.TaskStatus { State = TaskState.Submitted, Timestamp = DateTime.UtcNow.AddSeconds(i) }
            }, tenant: "tenant-a");
        }

        var first = await Store.ListTaskAsync(new Models.TaskQueryOptions { Tenant = "tenant-a", PageSize = 10 });
        Assert.Equal(10, first.Tasks.Count);
        Assert.False(string.IsNullOrWhiteSpace(first.NextPageToken));

        var second = await Store.ListTaskAsync(new Models.TaskQueryOptions { Tenant = "tenant-a", PageSize = 10, PageToken = first.NextPageToken });
        Assert.Equal(10, second.Tasks.Count);
        Assert.False(string.IsNullOrWhiteSpace(second.NextPageToken));

        var third = await Store.ListTaskAsync(new Models.TaskQueryOptions { Tenant = "tenant-a", PageSize = 10, PageToken = second.NextPageToken });
        Assert.Equal(5, third.Tasks.Count);
        Assert.True(string.IsNullOrWhiteSpace(third.NextPageToken) || third.NextPageToken == string.Empty);
    }

    [Fact]
    public async Task UpdateTaskAsync_ShouldUpdateIndexesAndListByStatus()
    {
        var id = "u-1";
        _ = await Store.AddTaskAsync(new Models.Task
        {
            Id = id,
            ContextId = "ctx",
            Status = new Models.TaskStatus { State = TaskState.Submitted, Timestamp = DateTime.UtcNow }
        }, tenant: "tenant-a");

        var updated = new Models.Task
        {
            Id = id,
            ContextId = "ctx",
            Status = new Models.TaskStatus { State = TaskState.Completed, Timestamp = DateTime.UtcNow.AddMinutes(1) }
        };

        _ = await Store.UpdateTaskAsync(updated, tenant: "tenant-a");

        var got = await Store.GetTaskAsync(id, tenant: "tenant-a");
        Assert.NotNull(got);
        Assert.Equal(TaskState.Completed, got!.Status!.State);

        var submitted = await Store.ListTaskAsync(new Models.TaskQueryOptions { Tenant = "tenant-a", Status = TaskState.Submitted, PageSize = 50 });
        var completed = await Store.ListTaskAsync(new Models.TaskQueryOptions { Tenant = "tenant-a", Status = TaskState.Completed, PageSize = 50 });

        Assert.DoesNotContain(submitted.Tasks, t => StringComparer.Ordinal.Equals(t.Id, id));
        Assert.Contains(completed.Tasks, t => StringComparer.Ordinal.Equals(t.Id, id));
    }

    [Fact]
    public async Task TaskPushNotificationConfig_ShouldRoundTrip_PerTenant()
    {
        var taskId = "task-pn-1";

        _ = await Store.AddTaskAsync(new Models.Task
        {
            Id = taskId,
            ContextId = "ctx",
            Status = new Models.TaskStatus { State = TaskState.Submitted, Timestamp = DateTime.UtcNow }
        }, tenant: "tenant-a");

        var requestA = new Models.SetTaskPushNotificationConfigRequest
        {
            Parent = $"tasks/{taskId}",
            Tenant = "tenant-a",
            ConfigId = "cfg-1",
            Config = new Models.TaskPushNotificationConfig
            {
                Name = $"tasks/{taskId}/cfg-1",
                PushNotificationConfig = new()
                {
                    Id = "cfg-1",
                    Url = new("https://example.com/notify")
                }
            }
        };

        var requestB = new Models.SetTaskPushNotificationConfigRequest
        {
            Parent = $"tasks/{taskId}",
            Tenant = "tenant-b",
            ConfigId = "cfg-1",
            Config = new Models.TaskPushNotificationConfig
            {
                Name = $"tasks/{taskId}/cfg-1",
                PushNotificationConfig = new()
                {
                    Id = "cfg-1",
                    Url = new("https://example.com/notify")
                }
            }
        };

        _ = await Store.SetTaskPushNotificationConfigAsync(requestA);
        _ = await Store.SetTaskPushNotificationConfigAsync(requestB);

        var gotA = await Store.GetTaskPushNotificationConfigAsync(taskId, "cfg-1", tenant: "tenant-a");
        var gotB = await Store.GetTaskPushNotificationConfigAsync(taskId, "cfg-1", tenant: "tenant-b");

        Assert.NotNull(gotA);
        Assert.NotNull(gotB);

        var listA = await Store.ListTaskPushNotificationConfigAsync(new Models.TaskPushNotificationConfigQueryOptions { Tenant = "tenant-a", TaskId = taskId, PageSize = 50 });
        var listB = await Store.ListTaskPushNotificationConfigAsync(new Models.TaskPushNotificationConfigQueryOptions { Tenant = "tenant-b", TaskId = taskId, PageSize = 50 });

        Assert.Single(listA.Configs);
        Assert.Single(listB.Configs);

        var deletedA = await Store.DeleteTaskPushNotificationConfigAsync(taskId, "cfg-1", tenant: "tenant-a");
        Assert.True(deletedA);

        var stillB = await Store.GetTaskPushNotificationConfigAsync(taskId, "cfg-1", tenant: "tenant-b");
        Assert.NotNull(stillB);
    }

}