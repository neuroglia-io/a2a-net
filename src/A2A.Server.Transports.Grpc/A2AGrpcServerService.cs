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
            Message = MapFromGrpc(rpcRequest.Request),
            Configuration = rpcRequest.Configuration == null ? null : new Models.SendMessageConfiguration()
            {
                AcceptedOutputModes = rpcRequest.Configuration.AcceptedOutputModes == null ? null : [.. rpcRequest.Configuration.AcceptedOutputModes],
                HistoryLength = rpcRequest.Configuration.HistoryLength < 1 ? null : (uint?)rpcRequest.Configuration.HistoryLength,
                Blocking = rpcRequest.Configuration.Blocking,
                PushNotificationConfig = rpcRequest.Configuration.PushNotificationConfig == null ? null : MapFromGrpc(rpcRequest.Configuration.PushNotificationConfig)
            }
        };
        var response = await server.SendMessageAsync(request, context.CancellationToken).ConfigureAwait(false);
        return new()
        {
            Task = response is Models.Task task ? MapToGrpc(task) : null,
            Msg = response is Models.Message message ? MapToGrpc(message) : null
        };
    }

    /// <inheritdoc/>
    public override async Task SendStreamingMessage(SendMessageRequest rpcRequest, IServerStreamWriter<StreamResponse> responseStream, ServerCallContext context)
    {
        var request = new Models.SendMessageRequest()
        {
            Message = MapFromGrpc(rpcRequest.Request),
            Configuration = rpcRequest.Configuration == null ? null : new Models.SendMessageConfiguration()
            {
                AcceptedOutputModes = rpcRequest.Configuration.AcceptedOutputModes == null ? null : [.. rpcRequest.Configuration.AcceptedOutputModes],
                HistoryLength = rpcRequest.Configuration.HistoryLength < 1 ? null : (uint?)rpcRequest.Configuration.HistoryLength,
                Blocking = rpcRequest.Configuration.Blocking,
                PushNotificationConfig = rpcRequest.Configuration.PushNotificationConfig == null ? null : MapFromGrpc(rpcRequest.Configuration.PushNotificationConfig)
            }
        };
        var stream = server.SendStreamingMessageAsync(request, context.CancellationToken);
        await foreach (var response in stream.WithCancellation(context.CancellationToken)) await responseStream.WriteAsync(MapToGrpcResponse(response)).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public override async Task<A2a.V1.Task> GetTask(GetTaskRequest request, ServerCallContext context)
    {
        var taskId = request.Name.Split('/')[1];
        var task = await server.GetTaskAsync(taskId, (uint?)request.HistoryLength, context.CancellationToken).ConfigureAwait(false);
        return MapToGrpc(task);
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
            Status = MapFromGrp(request.Status)
        };
        var taskList = await server.ListTasksAsync(queryOptions, context.CancellationToken).ConfigureAwait(false);
        var result = new ListTasksResponse()
        {
            NextPageToken = taskList.NextPageToken ?? string.Empty,
            PageSize = (int)taskList.PageSize,
            TotalSize = (int)taskList.TotalSize
        };
        result.Tasks.AddRange(taskList.Tasks.Select(MapToGrpc));
        return result;
    }

    /// <inheritdoc/>
    public override async Task<A2a.V1.Task> CancelTask(CancelTaskRequest request, ServerCallContext context)
    {
        var taskId = request.Name.Split('/')[1];
        var task = await server.CancelTaskAsync(taskId, context.CancellationToken).ConfigureAwait(false);
        return MapToGrpc(task);
    }

    /// <inheritdoc/>
    public override async Task SubscribeToTask(SubscribeToTaskRequest request, IServerStreamWriter<StreamResponse> responseStream, ServerCallContext context)
    {
        var stream = server.SubscribeToTaskAsync(request.Name.Split('/')[1], context.CancellationToken);
        await foreach (var response in stream.WithCancellation(context.CancellationToken)) await responseStream.WriteAsync(MapToGrpcResponse(response)).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public override async Task<TaskPushNotificationConfig> SetTaskPushNotificationConfig(SetTaskPushNotificationConfigRequest request, ServerCallContext context)
    {
        var taskId = request.Parent.Split('/')[1];
        var config = await server.SetOrUpdatePushNotificationConfigAsync(taskId, MapFromGrpc(request.Config), context.CancellationToken).ConfigureAwait(false);
        return MapToGrpc(config);
    }

    /// <inheritdoc/>
    public override async Task<TaskPushNotificationConfig> GetTaskPushNotificationConfig(GetTaskPushNotificationConfigRequest request, ServerCallContext context)
    {
        var segments = request.Name.Split('/');
        var taskId = segments[1];
        var configId = segments[3];
        var config = await server.GetPushNotificationConfigAsync(taskId, configId, context.CancellationToken).ConfigureAwait(false);
        return MapToGrpc(config);
    }

    /// <inheritdoc/>
    public override async Task<ListTaskPushNotificationConfigResponse> ListTaskPushNotificationConfig(ListTaskPushNotificationConfigRequest request, ServerCallContext context)
    {
        var queryOptions = new Models.PushNotificationConfigQueryOptions()
        {
            TaskId = request.Parent.Split('/')[1],
            PageSize = (uint?)request.PageSize,
            PageToken = request.PageToken
        };
        var configList = await server.ListPushNotificationConfigAsync(queryOptions, context.CancellationToken).ConfigureAwait(false);
        var result = new ListTaskPushNotificationConfigResponse()
        {
            NextPageToken = configList.NextPageToken ?? string.Empty
        };
        result.Configs.AddRange(configList.Configs.Select(MapToGrpc));
        return result;
    }

    /// <inheritdoc/>
    public override async Task<Empty> DeleteTaskPushNotificationConfig(DeleteTaskPushNotificationConfigRequest request, ServerCallContext context)
    {
        var segments = request.Name.Split('/');
        var taskId = segments[1];
        var configId = segments[3];
        await server.DeletePushNotificationConfigAsync(taskId, configId, context.CancellationToken).ConfigureAwait(false);
        return new();
    }

    string MapGrpcRole(A2a.V1.Role source)
    {
        return source switch
        {
            A2a.V1.Role.User => Role.User,
            A2a.V1.Role.Agent => Role.Agent,
            _ => Role.Unspecified
        };
    }

    A2a.V1.Role MapToGrpcRole(string source)
    {
        return source switch
        {
            Role.User => A2a.V1.Role.User,
            Role.Agent => A2a.V1.Role.Agent,
            _ => A2a.V1.Role.Unspecified
        };
    }

    Models.Message MapFromGrpc(Message source)
    {
        return new()
        {
            ContextId = source.ContextId,
            TaskId = source.TaskId,
            MessageId = source.MessageId,
            Role = MapGrpcRole(source.Role),
            Parts = [.. source.Parts.Select(MapFromGrpc)],
            Metadata = source.Metadata == null ? null : JsonNode.Parse(source.Metadata.ToString())?.AsObject() ?? [],
            Extensions = [.. source.Extensions.Select(e => new Uri(e))],
            ReferencedTaskIds = [.. source.ReferenceTaskIds]
        };
    }

    Models.Part MapFromGrpc(Part source)
    {
        return source.PartCase switch
        {
            Part.PartOneofCase.Text => new Models.TextPart()
            {
                Text = source.Text
            },
            Part.PartOneofCase.File => new Models.FilePart()
            {
                Name = source.File.Name,
                MediaType = source.File.MediaType,
                Uri = string.IsNullOrEmpty(source.File.FileWithUri) ? null : new Uri(source.File.FileWithUri),
                Bytes = source.File.FileWithBytes.IsEmpty ? null : source.File.FileWithBytes.ToByteArray()
            },
            Part.PartOneofCase.Data => new Models.DataPart()
            {
                Data = JsonNode.Parse(source.Data.Data.ToString())?.AsObject() ?? []
            },
            _ => throw new NotSupportedException($"Part type '{source.PartCase}' is not supported.")
        };
    }

    string MapFromGrp(A2a.V1.TaskState state)
    {
        return state switch
        {
            A2a.V1.TaskState.AuthRequired => TaskState.AuthRequired,
            A2a.V1.TaskState.Cancelled => TaskState.Cancelled,
            A2a.V1.TaskState.Completed => TaskState.Completed,
            A2a.V1.TaskState.Failed => TaskState.Failed,
            A2a.V1.TaskState.InputRequired => TaskState.InputRequired,
            A2a.V1.TaskState.Rejected => TaskState.Rejected,
            A2a.V1.TaskState.Submitted => TaskState.Submitted,
            A2a.V1.TaskState.Unspecified => TaskState.Unspecified,
            A2a.V1.TaskState.Working => TaskState.Working,
            _ => TaskState.Unspecified
        };
    }

    Models.PushNotificationConfig MapFromGrpc(PushNotificationConfig source)
    {
        return new()
        {
            Id = source.Id,
            Url = new(source.Url),
            Authentication = source.Authentication == null ? null : new Models.AuthenticationInfo()
            {
                Schemes = [.. source.Authentication.Schemes],
                Credentials = source.Authentication.Credentials
            },
            Token = source.Token
        };
    }

    Models.TaskPushNotificationConfig MapFromGrpc(TaskPushNotificationConfig source)
    {
        return new()
        {
            Name = source.Name,
            PushNotificationConfig = MapFromGrpc(source.PushNotificationConfig)
        };
    }

    StreamResponse MapToGrpcResponse(Models.StreamResponse source)
    {
        return new()
        {
            ArtifactUpdate = source.ArtifactUpdate == null ? null : MapToGrpc(source.ArtifactUpdate),
            Msg = source.Message == null ? null : MapToGrpc(source.Message),
            StatusUpdate = source.StatusUpdate == null ? null : MapToGrpc(source.StatusUpdate),
            Task = source.Task == null ? null : MapToGrpc(source.Task),
        };
    }

    Message MapToGrpc(Models.Message source)
    {
        var target = new Message()
        {
            ContextId = source.ContextId ?? Guid.NewGuid().ToString("N"),
            TaskId = source.TaskId ?? Guid.NewGuid().ToString("N"),
            MessageId = source.MessageId,
            Role = MapToGrpcRole(source.Role),
            Metadata = source.Metadata == null ? null : Struct.Parser.ParseJson(source.Metadata.ToJsonString(JsonSerializerOptions.Web))
        };
        target.Parts.AddRange(source.Parts.Select(MapToGrpc));
        if (source.Extensions != null) target.Extensions.AddRange(source.Extensions.Select(e => e.OriginalString));
        if (source.ReferencedTaskIds != null) target.ReferenceTaskIds.AddRange(source.ReferencedTaskIds);
        return target;
    }

    Part MapToGrpc(Models.Part source)
    {
        return source switch
        {
            Models.TextPart textPart => new Part()
            {
                Text = textPart.Text
            },
            Models.FilePart filePart => new Part()
            {
                File = new FilePart()
                {
                    Name = filePart.Name,
                    MediaType = filePart.MediaType,
                    FileWithUri = filePart.Uri?.OriginalString ?? string.Empty,
                    FileWithBytes = filePart.Bytes == null ? ByteString.Empty : ByteString.CopyFrom(filePart.Bytes.Value.ToArray())
                }
            },
            Models.DataPart dataPart => new Part()
            {
                Data = new() 
                { 
                    Data = Struct.Parser.ParseJson(dataPart.Data.ToJsonString()) 
                }
            },
            _ => throw new NotSupportedException($"Part type '{source.GetType().Name}' is not supported.")
        };
    }

    A2a.V1.Task MapToGrpc(Models.Task source)
    {
        var target = new A2a.V1.Task()
        {
            ContextId = source.ContextId,
            Id = source.Id,
            Status = new()
            {
                Message = source.Status.Message == null ? null : MapToGrpc(source.Status.Message),
                State = MapToGrpcTaskState(source.Status.State),
                Timestamp = source.Status.Timestamp?.ToTimestamp()
            }
        };
        if (source.Artifacts != null) target.Artifacts.AddRange(source.Artifacts.Select(MapToGrpc));
        return target;
    }

    A2a.V1.TaskState MapToGrpcTaskState(string source)
    {
        return source switch
        {
            TaskState.AuthRequired => A2a.V1.TaskState.AuthRequired,
            TaskState.Cancelled => A2a.V1.TaskState.Cancelled,
            TaskState.Completed => A2a.V1.TaskState.Completed,
            TaskState.Failed => A2a.V1.TaskState.Failed,
            TaskState.InputRequired => A2a.V1.TaskState.InputRequired,
            TaskState.Rejected => A2a.V1.TaskState.Rejected,
            TaskState.Submitted => A2a.V1.TaskState.Submitted,
            TaskState.Unspecified => A2a.V1.TaskState.Unspecified,
            TaskState.Working => A2a.V1.TaskState.Working,
            _ => A2a.V1.TaskState.Unspecified
        };
    }

    Artifact MapToGrpc(Models.Artifact source)
    {
        return new()
        {
            ArtifactId = source.ArtifactId,
            Name = source.Name,
            Description = source.Description,
            Metadata = source.Metadata == null ? null : Struct.Parser.ParseJson(source.Metadata.ToJsonString(JsonSerializerOptions.Web))
        };
    }

    TaskArtifactUpdateEvent MapToGrpc(Models.TaskArtifactUpdateEvent source)
    {
        return new()
        {
            ContextId = source.ContextId,
            TaskId = source.TaskId,
            Artifact = source.Artifact == null ? null : MapToGrpc(source.Artifact),
            Append = source.Append ?? false,
            LastChunk = source.LastChunk ?? false,
            Metadata = source.Metadata == null ? null : Struct.Parser.ParseJson(source.Metadata.ToJsonString(JsonSerializerOptions.Web))
        };
    }

    TaskStatusUpdateEvent MapToGrpc(Models.TaskStatusUpdateEvent source)
    {
        return new()
        {
            ContextId = source.ContextId,
            TaskId = source.TaskId,
            Status = new()
            {
                Message = source.Status.Message == null ? null : MapToGrpc(source.Status.Message),
                State = MapToGrpcTaskState(source.Status.State),
                Timestamp = source.Status.Timestamp?.ToTimestamp()
            },
            Final = source.Final,
            Metadata = source.Metadata == null ? null : Struct.Parser.ParseJson(source.Metadata.ToJsonString(JsonSerializerOptions.Web))
        };
    }

    PushNotificationConfig MapToGrpc(Models.PushNotificationConfig source)
    {
        var target = new PushNotificationConfig()
        {
            Id = source.Id,
            Url = source.Url.OriginalString,
            Authentication = source.Authentication == null ? null : new()
            {
                Credentials = source.Authentication.Credentials
            },
            Token = source.Token
        };
        if (source.Authentication?.Schemes != null) target.Authentication?.Schemes.AddRange(source.Authentication.Schemes);
        return target;
    }

    TaskPushNotificationConfig MapToGrpc(Models.TaskPushNotificationConfig source)
    {
        return new()
        {
            Name = source.Name,
            PushNotificationConfig = MapToGrpc(source.PushNotificationConfig)
        };
    }

}

