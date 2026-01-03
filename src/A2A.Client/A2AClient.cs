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

using Task = System.Threading.Tasks.Task;

namespace A2A.Client;

/// <summary>
/// Represents the default implementation of the <see cref="IA2AClient"/>.
/// </summary>
/// <param name="transport">The <see cref="IA2AClientTransport"/> to use.</param>
public sealed class A2AClient(IA2AClientTransport transport)
    : IA2AClient
{

    /// <inheritdoc/>
    public Task<Response> SendMessageAsync(SendMessageRequest request, CancellationToken cancellationToken = default) => transport.SendMessageAsync(request, cancellationToken);

    /// <inheritdoc/>
    public IAsyncEnumerable<StreamResponse> SendStreamingMessageAsync(SendMessageRequest request, CancellationToken cancellationToken = default) => transport.SendStreamingMessageAsync(request, cancellationToken);

    /// <inheritdoc/>
    public Task<Models.Task> GetTaskAsync(string id, uint? historyLength = null, string? tenant = null, CancellationToken cancellationToken = default) => transport.GetTaskAsync(id, historyLength, tenant, cancellationToken);

    /// <inheritdoc/>
    public Task<TaskQueryResult> ListTasksAsync(TaskQueryOptions? queryOptions = null, CancellationToken cancellationToken = default) => transport.ListTasksAsync(queryOptions, cancellationToken);

    /// <inheritdoc/>
    public Task<Models.Task> CancelTaskAsync(string id, string? tenant = null, CancellationToken cancellationToken = default) => transport.CancelTaskAsync(id, tenant, cancellationToken);

    /// <inheritdoc/>
    public IAsyncEnumerable<StreamResponse> SubscribeToTaskAsync(string id, string? tenant = null, CancellationToken cancellationToken = default) => transport.SubscribeToTaskAsync(id, tenant, cancellationToken);

    /// <inheritdoc/>
    public Task<TaskPushNotificationConfig> SetTaskPushNotificationConfigAsync(SetTaskPushNotificationConfigRequest request, CancellationToken cancellationToken = default) => transport.SetTaskPushNotificationConfigAsync(request, cancellationToken);

    /// <inheritdoc/>
    public Task<TaskPushNotificationConfig> GetTaskPushNotificationConfigAsync(string taskId, string configId, string? tenant = null, CancellationToken cancellationToken = default) => transport.GetTaskPushNotificationConfigAsync(taskId, configId, tenant, cancellationToken);

    /// <inheritdoc/>
    public Task<TaskPushNotificationConfigQueryResult> ListTaskPushNotificationConfigAsync(TaskPushNotificationConfigQueryOptions queryOptions, CancellationToken cancellationToken = default) => transport.ListTaskPushNotificationConfigAsync(queryOptions, cancellationToken);

    /// <inheritdoc/>
    public Task DeletePushNotificationConfigAsync(string taskId, string configId, string? tenant = null, CancellationToken cancellationToken = default) => transport.DeletePushNotificationConfigAsync(taskId, configId, tenant, cancellationToken);

    /// <inheritdoc/>
    public IA2AClient ActivateExtension(Uri uri)
    {
        ArgumentNullException.ThrowIfNull(uri);
        transport.ActivateExtension(uri);
        return this;
    }

    /// <inheritdoc/>
    public IA2AClient DeactivateExtension(Uri uri)
    {
        ArgumentNullException.ThrowIfNull(uri);
        transport.DeactivateExtension(uri);
        return this;
    }

}
