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

namespace A2A.Client.Transports;

internal static class A2AGrpcMapper
{

    public static string MapGrpcRole(A2a.V1.Role source)
    {
        return source switch
        {
            A2a.V1.Role.User => Role.User,
            A2a.V1.Role.Agent => Role.Agent,
            _ => Role.Unspecified
        };
    }

    public static A2a.V1.Role MapToGrpcRole(string source)
    {
        return source switch
        {
            Role.User => A2a.V1.Role.User,
            Role.Agent => A2a.V1.Role.Agent,
            _ => A2a.V1.Role.Unspecified
        };
    }

    public static Models.Response MapFromGrpc(SendMessageResponse source)
    {
        return source.PayloadCase switch 
        {
            SendMessageResponse.PayloadOneofCase.Task => MapFromGrpc(source.Task),
            SendMessageResponse.PayloadOneofCase.Msg => MapFromGrpc(source.Msg),
            _ => throw new NotSupportedException($"The specified response type '{source.PayloadCase}' is not supported.")
        };
    }

    public static Models.Message MapFromGrpc(Message source)
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

    public static Models.Task MapFromGrpc(A2a.V1.Task task)
    {
        return new()
        {
            ContextId = task.ContextId,
            Id = task.Id,
            History = task.History.Count == 0 ? null : [.. task.History.Select(MapFromGrpc)],
            Artifacts = task.Artifacts.Count == 0 ? null : [.. task.Artifacts.Select(MapFromGrpc)],
            Status = new Models.TaskStatus()
            {
                Message = task.Status.Message == null ? null : MapFromGrpc(task.Status.Message),
                State = MapFromGrpc(task.Status.State),
                Timestamp = task.Status.Timestamp.ToDateTime()
            },
            Metadata = task.Metadata == null ? null : JsonNode.Parse(task.Metadata.ToString())?.AsObject() ?? [],
        };
    }

    public static Models.Artifact MapFromGrpc(Artifact source)
    {
        return new()
        {
            ArtifactId = source.ArtifactId,
            Name = source.Name,
            Description = source.Description,
            Parts = [.. source.Parts.Select(MapFromGrpc)],
            Extensions = [.. source.Extensions.Select(e => new Uri(e))],
            Metadata = source.Metadata == null ? null : JsonNode.Parse(source.Metadata.ToString())?.AsObject() ?? [],
        };
    }

    public static Models.StreamResponse MapFromGrpc(StreamResponse source)
    {
        return new()
        {
            ArtifactUpdate = source.ArtifactUpdate == null ? null : MapFromGrpc(source.ArtifactUpdate),
            Message = source.Msg == null ? null : MapFromGrpc(source.Msg),
            StatusUpdate = source.StatusUpdate == null ? null : MapFromGrpc(source.StatusUpdate),
            Task = source.Task == null ? null : MapFromGrpc(source.Task),
        };
    }

    public static Models.TaskQueryResult MapFromGrpc(ListTasksResponse source)
    {
        return new()
        {
            NextPageToken = source.NextPageToken,
            PageSize = (uint)source.PageSize,
            TotalSize = (uint)source.TotalSize,
            Tasks = [.. source.Tasks.Select(MapFromGrpc)]
        };
    }

    public static Models.TaskPushNotificationConfigQueryResult MapFromGrpc(ListTaskPushNotificationConfigResponse source)
    {
        return new()
        {
            NextPageToken = source.NextPageToken,
            Configs = [.. source.Configs.Select(MapFromGrpc)]
        };
    }

    public static Models.Part MapFromGrpc(Part source)
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
                Uri = string.IsNullOrWhiteSpace(source.File.FileWithUri) ? null : new Uri(source.File.FileWithUri),
                Bytes = source.File.FileWithBytes.IsEmpty ? null : source.File.FileWithBytes.ToByteArray()
            },
            Part.PartOneofCase.Data => new Models.DataPart()
            {
                Data = JsonNode.Parse(source.Data.Data.ToString())?.AsObject() ?? []
            },
            _ => throw new NotSupportedException($"Part type '{source.PartCase}' is not supported.")
        };
    }

    public static string MapFromGrpc(A2a.V1.TaskState state)
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

    public static Models.PushNotificationConfig MapFromGrpc(PushNotificationConfig source)
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

    public static Models.TaskPushNotificationConfig MapFromGrpc(TaskPushNotificationConfig source)
    {
        return new()
        {
            Name = source.Name,
            PushNotificationConfig = MapFromGrpc(source.PushNotificationConfig)
        };
    }

    public static Models.TaskArtifactUpdateEvent MapFromGrpc(TaskArtifactUpdateEvent source)
    {
        return new()
        {
            Append = source.Append,
            Artifact = MapFromGrpc(source.Artifact),
            ContextId = source.ContextId,
            LastChunk = source.LastChunk,
            Metadata = source.Metadata == null ? null : JsonNode.Parse(source.Metadata.ToString())?.AsObject() ?? [],
            TaskId = source.TaskId
        };
    }

    public static Models.TaskStatusUpdateEvent MapFromGrpc(TaskStatusUpdateEvent source)
    {
        return new()
        {
            ContextId = source.ContextId,
            Final = source.Final,
            Metadata = source.Metadata == null ? null : JsonNode.Parse(source.Metadata.ToString())?.AsObject() ?? [],
            Status = new Models.TaskStatus()
            {
                Message = source.Status.Message == null ? null : MapFromGrpc(source.Status.Message),
                State = MapFromGrpc(source.Status.State),
                Timestamp = source.Status.Timestamp.ToDateTime()
            },
            TaskId = source.TaskId
        };
    }

    public static SendMessageRequest MapToGrpc(Models.SendMessageRequest source)
    {
        return new()
        {
            Request = MapToGrpc(source.Message),
            Tenant = source.Tenant,
            Configuration = source.Configuration == null ? null : MapToGrpc(source.Configuration),
            Metadata = source.Metadata == null ? null : Struct.Parser.ParseJson(source.Metadata.ToJsonString(JsonSerializerOptions.Web))
        };
    }

    public static SetTaskPushNotificationConfigRequest MapToGrpc(Models.SetTaskPushNotificationConfigRequest source)
    {
        return new()
        {
            Parent = source.Parent,
            ConfigId = source.ConfigId,
            Config = MapToGrpc(source.Config),
            Tenant = source.Tenant
        };
    }

    public static SendMessageConfiguration MapToGrpc(Models.SendMessageConfiguration source)
    {
        var target = new SendMessageConfiguration()
        {
            Blocking = source.Blocking ?? false,
            HistoryLength = (int?)source.HistoryLength ?? 0,
            PushNotificationConfig = source.PushNotificationConfig == null ? null : MapToGrpc(source.PushNotificationConfig)
        };
        if (source.AcceptedOutputModes is not null)  target.AcceptedOutputModes.AddRange(source.AcceptedOutputModes);
        return target;
    }

    public static Message MapToGrpc(Models.Message source)
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

    public static Part MapToGrpc(Models.Part source)
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

    public static A2a.V1.Task MapToGrpc(Models.Task source)
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

    public static A2a.V1.TaskState MapToGrpcTaskState(string source)
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

    public static Artifact MapToGrpc(Models.Artifact source)
    {
        return new()
        {
            ArtifactId = source.ArtifactId,
            Name = source.Name,
            Description = source.Description,
            Metadata = source.Metadata == null ? null : Struct.Parser.ParseJson(source.Metadata.ToJsonString(JsonSerializerOptions.Web))
        };
    }

    public static PushNotificationConfig MapToGrpc(Models.PushNotificationConfig source)
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

    public static TaskPushNotificationConfig MapToGrpc(Models.TaskPushNotificationConfig source)
    {
        return new()
        {
            Name = source.Name,
            PushNotificationConfig = MapToGrpc(source.PushNotificationConfig)
        };
    }

}
