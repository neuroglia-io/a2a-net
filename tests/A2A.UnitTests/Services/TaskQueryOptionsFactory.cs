namespace A2A.UnitTests.Services;

internal static class TaskQueryOptionsFactory
{

    internal static TaskQueryOptions Create() => new()
    {
        ContextId = Guid.NewGuid().ToString("N"),
        Status = TaskState.Completed,
        PageSize = 50,
        HistoryLength = 10,
        IncludeArtifacts = true,
        LastUpdateAfter = 1625097600,
        PageToken = Guid.NewGuid().ToString("N"),
        Metadata = new Dictionary<string, JsonNode>
        {
            ["filterKey"] = "filterValue"
        }
    };

}
