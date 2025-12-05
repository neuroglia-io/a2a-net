namespace A2A.UnitTests.Services;

internal static class TaskQueryResultFactory
{

    internal static TaskQueryResult Create() => new()
    {
        Tasks = [ TaskFactory.Create() ],
        NextPageToken = Guid.NewGuid().ToString("N"),
        PageSize = 50,
        TotalSize = 100
    };

}