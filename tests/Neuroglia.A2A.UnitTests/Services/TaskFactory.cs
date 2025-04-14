namespace Neuroglia.A2A.UnitTests.Services;

internal static class TaskFactory
{

    internal static Models.Task Create() => new()
    {
        Id = Guid.NewGuid().ToString("N"),
        Status = TaskStatusFactory.Create()
    };

}
