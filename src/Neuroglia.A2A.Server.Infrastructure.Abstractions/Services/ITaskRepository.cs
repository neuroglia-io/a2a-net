namespace Neuroglia.A2A.Server.Infrastructure.Services;

/// <summary>
/// Defines the fundamentals of a service used to persist A2A tasks
/// </summary>
public interface ITaskRepository
{

    /// <summary>
    /// Adds the specified <see cref="TaskRecord"/>
    /// </summary>
    /// <param name="task">The <see cref="TaskRecord"/> to add</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/></param>
    /// <returns>The newly added <see cref="TaskRecord"/></returns>
    Task<TaskRecord> AddAsync(TaskRecord task, CancellationToken cancellationToken = default);

    /// <summary>
    /// Determines whether or not the repository contains a <see cref="TaskRecord"/> with the specified id
    /// </summary>
    /// <param name="id">The id of the <see cref="TaskRecord"/> to get</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/></param>
    /// <returns>A boolean indicating whether or not the repository contains a <see cref="TaskRecord"/> with the specified id</returns>
    Task<bool> ContainsAsync(string id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets the <see cref="TaskRecord"/> with the specified id, if any
    /// </summary>
    /// <param name="id">The id of the <see cref="TaskRecord"/> to get</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/></param>
    /// <returns>The <see cref="TaskRecord"/> with the specified id, if any</returns>
    Task<TaskRecord?> GetAsync(string id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates the specified <see cref="TaskRecord"/>
    /// </summary>
    /// <param name="task">The <see cref="TaskRecord"/> to update</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/></param>
    /// <returns>The updated <see cref="TaskRecord"/></returns>
    Task<TaskRecord> UpdateAsync(TaskRecord task, CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes the <see cref="TaskRecord"/> with the specified id
    /// </summary>
    /// <param name="id">The id of the <see cref="TaskRecord"/> to delete</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/></param>
    /// <returns>A new awaitable <see cref="Task"/></returns>
    System.Threading.Tasks.Task DeleteAsync(string id, CancellationToken cancellationToken = default);

}
