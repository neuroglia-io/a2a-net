namespace A2A.UnitTests.Services;

internal static class TaskStatusFactory
{

    internal static Models.TaskStatus Create() => new()
    {
        State = TaskState.Completed,
        Timestamp = DateTime.UtcNow,
        Message = MessageFactory.Create()
    };

}