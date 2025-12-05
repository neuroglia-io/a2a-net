namespace A2A.Server.Services;

/// <summary>
/// Defines the fundamentals of a service used to interact with an A2A agent.
/// </summary>
public interface IAgentRuntime
{

    /// <summary>
    /// Processes the specified message.
    /// </summary>
    /// <param name="message">The message to process.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/>.</param>
    /// <returns>A new <see cref="Models.Response"/> resulting from the processed message.</returns>
    Task<Models.Response> ProcessAsync(Models.Message message, CancellationToken cancellationToken = default);

    /// <summary>
    /// Executes the specified task.
    /// </summary>
    /// <param name="task">The task to execute.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/>.</param>
    /// <returns>A stream of <see cref="Models.TaskEvent"/>s resulting from the specified task execution.</returns>
    IAsyncEnumerable<Models.TaskEvent> ExecuteAsync(Models.Task task, CancellationToken cancellationToken = default);

}
