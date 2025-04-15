namespace Neuroglia.A2A.Server.Infrastructure.Services;

/// <summary>
/// Defines the fundamentals of a service used to interact with an AI agent to execute tasks
/// </summary>
public interface IAgentRuntime
{

    /// <summary>
    /// Executes the specified task and streams the content produced by the agent
    /// </summary>
    /// <param name="task">The task to execute</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/></param>
    /// <returns>A new <see cref="IAsyncEnumerable{T}"/> used to stream the content produced by the agent during the task's execution</returns>
    IAsyncEnumerable<AgentResponseContent> ExecuteAsync(Models.Task task, CancellationToken cancellationToken = default);

    /// <summary>
    /// Cancels the specified task's execution
    /// </summary>
    /// <param name="task">The task to cancel.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/></param>
    /// <returns>A new awaitable <see cref="Task"/></returns>
    System.Threading.Tasks.Task CancelAsync(Models.Task task, CancellationToken cancellationToken = default);

}