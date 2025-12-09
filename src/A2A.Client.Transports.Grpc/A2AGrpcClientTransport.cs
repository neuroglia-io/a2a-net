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

using Grpc.Core;
using Task = System.Threading.Tasks.Task;

namespace A2A.Client.Transports;

/// <summary>
/// Represents the gRPC implementation of the <see cref="IA2AClientTransport"/> interface.
/// </summary>
/// <param name="grpcClient">The gRPC client to use.</param>
public sealed class A2AGrpcClientTransport(A2a.V1.A2AService.A2AServiceClient grpcClient)
    : IA2AClientTransport
{

    /// <inheritdoc/>
    public async Task<Models.Response> SendMessageAsync(Models.SendMessageRequest request, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);
        return A2AGrpcMapper.MapFromGrpc(await grpcClient.SendMessageAsync(A2AGrpcMapper.MapToGrpc(request), cancellationToken: cancellationToken).ConfigureAwait(false));
    }

    /// <inheritdoc/>
    public async IAsyncEnumerable<Models.StreamResponse> SendStreamingMessageAsync(Models.SendMessageRequest request, [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);
        var result = grpcClient.SendStreamingMessage(A2AGrpcMapper.MapToGrpc(request), cancellationToken: cancellationToken);
        await foreach (var streamResponse in result.ResponseStream.ReadAllAsync(cancellationToken).ConfigureAwait(false))
        {
            if (streamResponse is null) continue;
            yield return A2AGrpcMapper.MapFromGrpc(streamResponse);
        }
    }

    /// <inheritdoc/>
    public async Task<Models.Task> GetTaskAsync(string id, uint? historyLength = null, string? tenant = null, CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(id);
        return A2AGrpcMapper.MapFromGrpc(await grpcClient.GetTaskAsync(new()
        {
            Name = $"tasks/{id}",
            HistoryLength = (int?)historyLength ?? 0,
            Tenant = tenant
        }, cancellationToken: cancellationToken).ConfigureAwait(false));
    }

    /// <inheritdoc/>
    public async Task<Models.TaskQueryResult> ListTasksAsync(Models.TaskQueryOptions? queryOptions = null, CancellationToken cancellationToken = default)
    {
        return A2AGrpcMapper.MapFromGrpc(await grpcClient.ListTasksAsync(new()
        {
            ContextId = queryOptions?.ContextId,
            HistoryLength = (int?)queryOptions?.HistoryLength ?? 0,
            IncludeArtifacts = queryOptions?.IncludeArtifacts ?? false,
            LastUpdatedAfter = queryOptions?.LastUpdateAfter ?? 0,
            PageSize = (int?)queryOptions?.PageSize ?? 0,
            PageToken = queryOptions?.PageToken,
            Status = queryOptions?.Status is null ? A2a.V1.TaskState.Unspecified : A2AGrpcMapper.MapToGrpcTaskState(queryOptions.Status),
            Tenant = queryOptions?.Tenant
        }, cancellationToken: cancellationToken).ConfigureAwait(false));
    }

    /// <inheritdoc/>
    public async Task<Models.Task> CancelTaskAsync(string id, string? tenant = null, CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(id);
        return A2AGrpcMapper.MapFromGrpc(await grpcClient.CancelTaskAsync(new()
        {
            Name = $"tasks/{id}",
            Tenant = tenant
        }, cancellationToken: cancellationToken).ConfigureAwait(false));
    }

    /// <inheritdoc/>
    public async IAsyncEnumerable<Models.StreamResponse> SubscribeToTaskAsync(string id, string? tenant = null, [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(id);
        var result = grpcClient.SubscribeToTask(new()
        {
            Name = $"tasks/{id}",
            Tenant = tenant
        }, cancellationToken: cancellationToken);
        await foreach (var streamResponse in result.ResponseStream.ReadAllAsync(cancellationToken).ConfigureAwait(false))
        {
            if (streamResponse is null) continue;
            yield return A2AGrpcMapper.MapFromGrpc(streamResponse);
        }
    }

    /// <inheritdoc/>
    public async Task<Models.TaskPushNotificationConfig> SetTaskPushNotificationConfigAsync(Models.SetTaskPushNotificationConfigRequest request, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);
        return A2AGrpcMapper.MapFromGrpc(await grpcClient.SetTaskPushNotificationConfigAsync(A2AGrpcMapper.MapToGrpc(request), cancellationToken: cancellationToken).ConfigureAwait(false));
    }

    /// <inheritdoc/>
    public async Task<Models.TaskPushNotificationConfig> GetTaskPushNotificationConfigAsync(string taskId, string configId, string? tenant = null, CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(taskId);
        ArgumentException.ThrowIfNullOrWhiteSpace(configId);
        return A2AGrpcMapper.MapFromGrpc(await grpcClient.GetTaskPushNotificationConfigAsync(new()
        {
            Name = $"tasks/{taskId}/pushNotificationConfigs/{configId}",
            Tenant = tenant
        }, cancellationToken: cancellationToken).ConfigureAwait(false));
    }

    /// <inheritdoc/>
    public async Task<Models.TaskPushNotificationConfigQueryResult> ListTaskPushNotificationConfigAsync(Models.TaskPushNotificationConfigQueryOptions queryOptions, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(queryOptions);
        return A2AGrpcMapper.MapFromGrpc(await grpcClient.ListTaskPushNotificationConfigAsync(new()
        {
            PageSize = (int?)queryOptions.PageSize ?? 0,
            PageToken = queryOptions.PageToken,
            Parent = $"tasks/{queryOptions.TaskId}",
            Tenant = queryOptions.Tenant
        }, cancellationToken: cancellationToken).ConfigureAwait(false));
    }

    /// <inheritdoc/>
    public async Task DeletePushNotificationConfigAsync(string taskId, string configId, string? tenant = null, CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(taskId);
        ArgumentException.ThrowIfNullOrWhiteSpace(configId);
        await grpcClient.DeleteTaskPushNotificationConfigAsync(new()
        {
            Name = $"tasks/{taskId}/pushNotificationConfigs/{configId}",
            Tenant = tenant
        }, cancellationToken: cancellationToken).ConfigureAwait(false);
    }

}
