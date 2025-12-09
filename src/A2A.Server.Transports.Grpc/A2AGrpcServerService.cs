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

using A2a.V1;
using Task = System.Threading.Tasks.Task;

namespace A2A.Server.Transports;

/// <summary>
/// Represents the gRPC service that exposes the A2A server API.
/// </summary>
/// <param name="server">The A2A server.</param>
public class A2AGrpcServerService(IA2AServer server)
    : A2AService.A2AServiceBase
{

    /// <inheritdoc/>
    public override async Task<SendMessageResponse> SendMessage(SendMessageRequest rpcRequest, ServerCallContext context)
    {
        var request = new Models.SendMessageRequest()
        {
            Message = A2AGrpcMapper.MapFromGrpc(rpcRequest.Request),
            Configuration = rpcRequest.Configuration == null ? null : new Models.SendMessageConfiguration()
            {
                AcceptedOutputModes = rpcRequest.Configuration.AcceptedOutputModes == null ? null : [.. rpcRequest.Configuration.AcceptedOutputModes],
                HistoryLength = rpcRequest.Configuration.HistoryLength < 1 ? null : (uint?)rpcRequest.Configuration.HistoryLength,
                Blocking = rpcRequest.Configuration.Blocking,
                PushNotificationConfig = rpcRequest.Configuration.PushNotificationConfig == null ? null : A2AGrpcMapper.MapFromGrpc(rpcRequest.Configuration.PushNotificationConfig)
            }
        };
        var response = await server.SendMessageAsync(request, context.CancellationToken).ConfigureAwait(false);
        return new()
        {
            Task = response is Models.Task task ? A2AGrpcMapper.MapToGrpc(task) : null,
            Msg = response is Models.Message message ? A2AGrpcMapper.MapToGrpc(message) : null
        };
    }

    /// <inheritdoc/>
    public override async Task SendStreamingMessage(SendMessageRequest rpcRequest, IServerStreamWriter<StreamResponse> responseStream, ServerCallContext context)
    {
        var request = new Models.SendMessageRequest()
        {
            Message = A2AGrpcMapper.MapFromGrpc(rpcRequest.Request),
            Configuration = rpcRequest.Configuration == null ? null : new Models.SendMessageConfiguration()
            {
                AcceptedOutputModes = rpcRequest.Configuration.AcceptedOutputModes == null ? null : [.. rpcRequest.Configuration.AcceptedOutputModes],
                HistoryLength = rpcRequest.Configuration.HistoryLength < 1 ? null : (uint?)rpcRequest.Configuration.HistoryLength,
                Blocking = rpcRequest.Configuration.Blocking,
                PushNotificationConfig = rpcRequest.Configuration.PushNotificationConfig == null ? null : A2AGrpcMapper.MapFromGrpc(rpcRequest.Configuration.PushNotificationConfig)
            }
        };
        var stream = server.SendStreamingMessageAsync(request, context.CancellationToken);
        await foreach (var response in stream.WithCancellation(context.CancellationToken)) await responseStream.WriteAsync(A2AGrpcMapper.MapToGrpcResponse(response)).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public override async Task<A2a.V1.Task> GetTask(GetTaskRequest request, ServerCallContext context)
    {
        var taskId = request.Name.Split('/')[1];
        var task = await server.GetTaskAsync(taskId, (uint?)request.HistoryLength, request.Tenant, context.CancellationToken).ConfigureAwait(false);
        return A2AGrpcMapper.MapToGrpc(task);
    }

    /// <inheritdoc/>
    public override async Task<ListTasksResponse> ListTasks(ListTasksRequest request, ServerCallContext context)
    {
        var queryOptions = new Models.TaskQueryOptions()
        {
            ContextId = request.ContextId,
            HistoryLength = request.HasHistoryLength ? (uint?)request.HistoryLength : null,
            IncludeArtifacts = request.IncludeArtifacts,
            LastUpdateAfter = (uint?)request.LastUpdatedAfter,
            PageSize = request.HasPageSize ? (uint?)request.PageSize : null,
            PageToken = request.PageToken,
            Status = A2AGrpcMapper.MapFromGrpc(request.Status)
        };
        var taskList = await server.ListTasksAsync(queryOptions, context.CancellationToken).ConfigureAwait(false);
        var result = new ListTasksResponse()
        {
            NextPageToken = taskList.NextPageToken ?? string.Empty,
            PageSize = (int)taskList.PageSize,
            TotalSize = (int)taskList.TotalSize
        };
        result.Tasks.AddRange(taskList.Tasks.Select(A2AGrpcMapper.MapToGrpc));
        return result;
    }

    /// <inheritdoc/>
    public override async Task<A2a.V1.Task> CancelTask(CancelTaskRequest request, ServerCallContext context)
    {
        var taskId = request.Name.Split('/')[1];
        var task = await server.CancelTaskAsync(taskId, request.Tenant, context.CancellationToken).ConfigureAwait(false);
        return A2AGrpcMapper.MapToGrpc(task);
    }

    /// <inheritdoc/>
    public override async Task SubscribeToTask(SubscribeToTaskRequest request, IServerStreamWriter<StreamResponse> responseStream, ServerCallContext context)
    {
        var stream = server.SubscribeToTaskAsync(request.Name.Split('/')[1], request.Tenant, context.CancellationToken);
        await foreach (var response in stream.WithCancellation(context.CancellationToken)) await responseStream.WriteAsync(A2AGrpcMapper.MapToGrpcResponse(response)).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public override async Task<TaskPushNotificationConfig> SetTaskPushNotificationConfig(SetTaskPushNotificationConfigRequest request, ServerCallContext context)
    {
        var config = await server.SetTaskPushNotificationConfigAsync(A2AGrpcMapper.MapFromGrpc(request), context.CancellationToken).ConfigureAwait(false);
        return A2AGrpcMapper.MapToGrpc(config);
    }

    /// <inheritdoc/>
    public override async Task<TaskPushNotificationConfig> GetTaskPushNotificationConfig(GetTaskPushNotificationConfigRequest request, ServerCallContext context)
    {
        var segments = request.Name.Split('/');
        var taskId = segments[1];
        var configId = segments[3];
        var config = await server.GetTaskPushNotificationConfigAsync(taskId, configId, request.Tenant, context.CancellationToken).ConfigureAwait(false);
        return A2AGrpcMapper.MapToGrpc(config);
    }

    /// <inheritdoc/>
    public override async Task<ListTaskPushNotificationConfigResponse> ListTaskPushNotificationConfig(ListTaskPushNotificationConfigRequest request, ServerCallContext context)
    {
        var queryOptions = new Models.TaskPushNotificationConfigQueryOptions()
        {
            TaskId = request.Parent.Split('/')[1],
            PageSize = (uint?)request.PageSize,
            PageToken = request.PageToken
        };
        var configList = await server.ListTaskPushNotificationConfigAsync(queryOptions, context.CancellationToken).ConfigureAwait(false);
        var result = new ListTaskPushNotificationConfigResponse()
        {
            NextPageToken = configList.NextPageToken ?? string.Empty
        };
        result.Configs.AddRange(configList.Configs.Select(A2AGrpcMapper.MapToGrpc));
        return result;
    }

    /// <inheritdoc/>
    public override async Task<Empty> DeleteTaskPushNotificationConfig(DeleteTaskPushNotificationConfigRequest request, ServerCallContext context)
    {
        var segments = request.Name.Split('/');
        var taskId = segments[1];
        var configId = segments[3];
        await server.DeletePushNotificationConfigAsync(taskId, configId, request.Tenant, context.CancellationToken).ConfigureAwait(false);
        return new();
    }

}
