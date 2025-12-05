namespace A2A.UnitTests.Services;

internal static class TaskStatusUpdateEventFactory
{

    internal static TaskStatusUpdateEvent Create() => new()
    {
        TaskId = Guid.NewGuid().ToString("N"),
        ContextId = Guid.NewGuid().ToString("N"),
        Status = TaskStatusFactory.Create(),
        Final = true,
        Metadata = new Dictionary<string, JsonNode>
        {
            { "exampleKey", JsonValue.Create("exampleValue") }
        }
    };

}
