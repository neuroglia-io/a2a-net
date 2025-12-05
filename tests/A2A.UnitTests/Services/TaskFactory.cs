namespace A2A.UnitTests.Services;

internal static class TaskFactory
{

    internal static Models.Task Create() => new()
    {
        Id = Guid.NewGuid().ToString("N"),
        ContextId = Guid.NewGuid().ToString("N"),
        Status = TaskStatusFactory.Create(),
        Artifacts = 
        [
            ArtifactFactory.Create()
        ],
        History = 
        [
            MessageFactory.Create()
        ],
        Metadata = new Dictionary<string, JsonNode>
        {
            ["metaKey"] = "metaValue"
        }
    };

}
