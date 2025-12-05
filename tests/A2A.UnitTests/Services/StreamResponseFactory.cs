namespace A2A.UnitTests.Services;

internal static class StreamResponseFactory
{

    internal static StreamResponse CreateTaskResponse() => new()
    {
        Task = TaskFactory.Create()
    };

    internal static StreamResponse CreateMessageResponse() => new()
    {
        Message = MessageFactory.Create()
    };

    internal static StreamResponse CreateTaskStatusUpdateEventResponse() => new()
    {
        StatusUpdate = TaskStatusUpdateEventFactory.Create()
    };

    internal static StreamResponse CreateTaskArtifactUpdateEventResponse() => new()
    {
        ArtifactUpdate = TaskArtifactUpdateEventFactory.Create()
    };

}
