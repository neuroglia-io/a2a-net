namespace A2A.Server.Services;

/// <summary>
/// Defines the fundamentals of a service used to enqueue tasks for execution.
/// </summary>
public interface ITaskQueue
{

    /// <summary>
    /// Enqueues the specified task for execution.
    /// </summary>
    /// <param name="task">The task to enqueue.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/>.</param>
    /// <returns>A new awaitable <see cref="Task"/>.</returns>
    Task EnqueueAsync(Models.Task task, CancellationToken cancellationToken = default);

    /// <summary>
    /// Cancels the execution of the specified task.
    /// </summary>
    /// <param name="task">The task to cancel.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/>.</param>
    /// <returns>A new awaitable <see cref="Task"/>.</returns>
    Task CancelAsync(Models.Task task, CancellationToken cancellationToken = default);

}