namespace A2A.UnitTests.Services;

internal static class TaskArtifactUpdateEventFactory
{

    internal static TaskArtifactUpdateEvent Create() => new()
    {
        TaskId = Guid.NewGuid().ToString("N"),
        ContextId = Guid.NewGuid().ToString("N"),
        Artifact = ArtifactFactory.Create(),
        Metadata = new Dictionary<string, JsonNode>
        {
            { "exampleKey", JsonValue.Create("exampleValue") }
        },
        Append = true,
        LastChunk = false
    };

}