using System.Reactive.Subjects;

namespace Neuroglia.A2A.Server.Infrastructure.Services;

/// <summary>
/// Defines the fundamentals of a service used to stream task events
/// </summary>
public interface ITaskEventStream
    : ISubject<TaskEvent>
{



}
