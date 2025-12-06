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

namespace A2A.Services;

/// <summary>
/// Defines the fundamentals of a service that handles the A2A protocol.
/// </summary>
public interface IA2AProtocolService
{

    /// <summary>
    /// Sends a message.
    /// </summary>
    /// <param name="request">The request to process.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/>.</param>
    /// <returns>A new <see cref="Response"/> representing the result of the operation.</returns>
    Task<Response> SendMessageAsync(SendMessageRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Sends a streaming message.
    /// </summary>
    /// <param name="request">The request to process.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/>.</param>
    /// <returns>A stream of <see cref="StreamResponse"/> used to monitor the progress of the operation.</returns>
    IAsyncEnumerable<StreamResponse> SendStreamingMessageAsync(SendMessageRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets the specified task.
    /// </summary>
    /// <param name="id">The unique identifier of the task to get.</param>
    /// <param name="historyLength">The maximum number of messages, if any, to include in the history.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/>.</param>
    /// <returns>The specified <see cref="Task"/>.</returns>
    Task<Models.Task> GetTaskAsync(string id, uint? historyLength, CancellationToken cancellationToken = default);

    /// <summary>
    /// Lists tasks.
    /// </summary>
    /// <param name="queryOptions">The query options, if any.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/>.</param>
    /// <returns>A new <see cref="TaskQueryResult"/> representing the result of the operation.</returns>
    Task<TaskQueryResult> ListTasksAsync(TaskQueryOptions? queryOptions = null, CancellationToken cancellationToken = default);

    /// <summary>
    /// Cancels the specified task.
    /// </summary>
    /// <param name="id">The unique identifier of the task to cancel.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/>.</param>
    /// <returns>The cancelled <see cref="Task"/>.</returns>
    Task<Models.Task> CancelTaskAsync(string id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Subscribes to the specified task.
    /// </summary>
    /// <param name="id">The unique identifier of the task to subscribe to.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/>.</param>
    /// <returns>A stream of <see cref="StreamResponse"/> used to monitor the progress of the operation.</returns>
    IAsyncEnumerable<StreamResponse> SubscribeToTaskAsync(string id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Sets or updates the push notification configuration for the specified task.
    /// </summary>
    /// <param name="taskId">The unique identifier of the task to set or update the push notification configuration for.</param>
    /// <param name="config">The push notification configuration to set or update.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/>.</param>
    /// <returns>The set or updated <see cref="PushNotificationConfig"/>.</returns>
    Task<PushNotificationConfig> SetOrUpdatePushNotificationConfigAsync(string taskId, PushNotificationConfig config, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets the push notification configuration for the specified task.
    /// </summary>
    /// <param name="taskId">The unique identifier of the task to get the push notification configuration for.</param>
    /// <param name="configId">The unique identifier of the push notification configuration to get.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/>.</param>
    /// <returns>The specified <see cref="PushNotificationConfig"/>.</returns>
    Task<PushNotificationConfig> GetPushNotificationConfigAsync(string taskId, string configId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Lists push notification configurations.
    /// </summary>
    /// <param name="queryOptions">The query options, if any.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/></param>
    /// <returns>A new <see cref="PushNotificationConfigQueryResult"/> representing the result of the operation.</returns>
    Task<PushNotificationConfigQueryResult> ListPushNotificationConfigAsync(PushNotificationConfigQueryOptions? queryOptions = null, CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes the specified push notification configuration.
    /// </summary>
    /// <param name="taskId">The unique identifier of the task to delete the push notification configuration from.</param>
    /// <param name="configId">The unique identifier of the push notification configuration to delete.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/>.</param>
    /// <returns>A boolean indicating whether the deletion was successful.</returns>
    Task<bool> DeletePushNotificationConfigAsync(string taskId, string configId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets a potentially more detailed version of the Agent Card after the client has authenticated.
    /// </summary>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/>.</param>
    /// <returns>A complete <see cref="AgentCard"/>, which may contain additional details or skills not present in the public card</returns>
    Task<AgentCard> GetExtendedAgentCardAsync(CancellationToken cancellationToken = default);

}