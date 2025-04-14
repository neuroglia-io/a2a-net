namespace Neuroglia.A2A.UnitTests.Services;

internal static class TaskStatusFactory
{

    internal static Models.TaskStatus Create() => new()
    {
        State = TaskState.Submitted,
        Message = MessageFactory.Create(),
        Timestamp = DateTimeOffset.Now
    };

}