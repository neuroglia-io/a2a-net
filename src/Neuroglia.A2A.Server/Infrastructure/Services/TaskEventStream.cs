using System.Reactive.Subjects;

namespace Neuroglia.A2A.Server.Infrastructure.Services;

/// <summary>
/// Represents the default implementation of the <see cref="ITaskEventStream"/> interface
/// </summary>
public class TaskEventStream
    : ITaskEventStream
{

    /// <summary>
    /// Gets the <see cref="ISubject{RpcEvent}"/> used to stream <see cref="TaskEvent"/>s
    /// </summary>
    protected Subject<TaskEvent> Subject { get; } = new();

    /// <inheritdoc/>
    public virtual IDisposable Subscribe(IObserver<TaskEvent> observer) => Subject.Subscribe(observer);

    /// <inheritdoc/>
    public virtual void OnNext(TaskEvent value) => Subject.OnNext(value);

    /// <inheritdoc/>
    public virtual void OnCompleted() => Subject.OnCompleted();

    /// <inheritdoc/>
    public virtual void OnError(Exception error) => Subject.OnError(error);

}
