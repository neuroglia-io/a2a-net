namespace Neuroglia.A2A.Server.Infrastructure.Services;

/// <summary>
/// Defines the fundamentals of a service used to handle tasks
/// </summary>
public interface ITaskHandler
{

    /// <summary>
    /// Submits the specified task
    /// </summary>
    /// <param name="task">The task to submit</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/></param>
    /// <returns>The submitted task</returns>
    Task<TaskRecord> SubmitAsync(TaskRecord task, CancellationToken cancellationToken = default);

    /// <summary>
    /// Cancels the specified task
    /// </summary>
    /// <param name="task">The task to cancel</param>
    /// <param name="message">A message, if any, associated with the task's cancellation</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/></param>
    /// <returns>The cancelled task</returns>
    Task<TaskRecord> CancelAsync(TaskRecord task, Message? message = null, CancellationToken cancellationToken = default);

}
