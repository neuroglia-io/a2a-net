// Copyright © 2025-Present the a2a-net Authors
//
// Licensed under the Apache License, Version 2.0 (the "License"),
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

namespace A2A.Server.Services;

/// <summary>
/// Defines the fundamentals of a service used to store and retrieve A2A-specific resources.
/// </summary>
public interface IStateStore
{

    /// <summary>
    /// Adds the specified task.
    /// </summary>
    /// <param name="task">The task to add.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/>.</param>
    /// <returns>The newly added task.</returns>
    Task<Models.Task> AddTaskAsync(Models.Task task, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets the task with the specified identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the task to get.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/>.</param>
    /// <returns>The task with the specified identifier, or <see langword="null"/> if not found.</returns>
    Task<Models.Task?> GetTaskAsync(string id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets the push notification configuration with the specified identifier for the specified task.
    /// </summary>
    /// <param name="taskId">The unique identifier of the task the push notification configuration belongs to.</param>
    /// <param name="configId">The unique identifier of the push notification configuration to get.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/>.</param>
    /// <returns>The push notification configuration with the specified identifier, or <see langword="null"/> if not found.</returns>
    Task<Models.PushNotificationConfig?> GetPushNotificationConfigAsync(string taskId, string configId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Lists all tasks.
    /// </summary>
    /// <param name="queryOptions">The query options, if any.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/>.</param>
    /// <returns>A new <see cref="Models.TaskQueryResult"/> containing the tasks matching the specified criteria.</returns>
    Task<Models.TaskQueryResult> ListTaskAsync(Models.TaskQueryOptions? queryOptions = null, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates the specified task.
    /// </summary>
    /// <param name="task">The task to update.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/>.</param>
    /// <returns>The updated task.</returns>
    Task<Models.Task> UpdateTaskAsync(Models.Task task, CancellationToken cancellationToken = default);

    /// <summary>
    /// Sets or updates the push notification configuration for the specified task.
    /// </summary>
    /// <param name="taskId">The unique identifier of the task to set or update the push notification configuration for.</param>
    /// <param name="config">The push notification configuration to set or update.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/>.</param>
    /// <returns>The set or updated <see cref="Models.PushNotificationConfig"/>.</returns>
    Task<Models.PushNotificationConfig> SetOrUpdatePushNotificationConfigAsync(string taskId, Models.PushNotificationConfig config, CancellationToken cancellationToken = default);

    /// <summary>
    /// Lists push notification configurations.
    /// </summary>
    /// <param name="queryOptions">The query options, if any.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/></param>
    /// <returns>A new <see cref="Models.PushNotificationConfigQueryResult"/> representing the result of the operation.</returns>
    Task<Models.PushNotificationConfigQueryResult> ListPushNotificationConfigAsync(Models.PushNotificationConfigQueryOptions? queryOptions = null, CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes the specified push notification configuration.
    /// </summary>
    /// <param name="taskId">The unique identifier of the task to delete the push notification configuration from.</param>
    /// <param name="configId">The unique identifier of the push notification configuration to delete.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/>.</param>
    /// <returns>A boolean indicating whether the deletion was successful.</returns>
    Task<bool> DeletePushNotificationConfigAsync(string taskId, string configId, CancellationToken cancellationToken = default);

}
