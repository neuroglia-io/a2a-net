namespace A2A.Server.Services;

/// <summary>
/// Defines the fundamentals of a service used to stream task events.
/// </summary>
public interface ITaskEventStream
    : ISubject<Models.TaskEvent>
{



}
