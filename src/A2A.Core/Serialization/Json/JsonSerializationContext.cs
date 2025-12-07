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

namespace A2A.Serialization.Json;

/// <summary>
/// Represents the JSON serialization context for A2A core types
/// </summary>
[JsonSourceGenerationOptions(GenerationMode = JsonSourceGenerationMode.Metadata, PropertyNamingPolicy = JsonKnownNamingPolicy.CamelCase, WriteIndented = false, DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull, AllowOutOfOrderMetadataProperties = true)]
[JsonSerializable(typeof(AgentCapabilities))]
[JsonSerializable(typeof(AgentCard))]
[JsonSerializable(typeof(AgentCardSignature))]
[JsonSerializable(typeof(AgentExtension))]
[JsonSerializable(typeof(AgentInterface))]
[JsonSerializable(typeof(AgentProvider))]
[JsonSerializable(typeof(AgentSkill))]
[JsonSerializable(typeof(ApiKeySecurityScheme))]
[JsonSerializable(typeof(Artifact))]
[JsonSerializable(typeof(AuthenticationInfo))]
[JsonSerializable(typeof(DataPart))]
[JsonSerializable(typeof(FilePart))]
[JsonSerializable(typeof(HttpSecurityScheme))]
[JsonSerializable(typeof(Message))]
[JsonSerializable(typeof(MutualTlsSecurityScheme))]
[JsonSerializable(typeof(OAuth2SecurityScheme))]
[JsonSerializable(typeof(OAuthFlow))]
[JsonSerializable(typeof(OAuthFlows))]
[JsonSerializable(typeof(OpenIdConnectSecurityScheme))]
[JsonSerializable(typeof(Part))]
[JsonSerializable(typeof(PushNotificationConfig))]
[JsonSerializable(typeof(PushNotificationConfigQueryOptions))]
[JsonSerializable(typeof(PushNotificationConfigQueryResult))]
[JsonSerializable(typeof(Response))]
[JsonSerializable(typeof(SecurityScheme))]
[JsonSerializable(typeof(SendMessageRequest))]
[JsonSerializable(typeof(SendMessageConfiguration))]
[JsonSerializable(typeof(SetOrUpdatePushNotificationConfigRequest))]
[JsonSerializable(typeof(StreamResponse))]
[JsonSerializable(typeof(Models.Task))]
[JsonSerializable(typeof(TaskEvent))]
[JsonSerializable(typeof(TaskArtifactUpdateEvent))]
[JsonSerializable(typeof(TaskPushNotificationConfig))]
[JsonSerializable(typeof(TaskQueryOptions))]
[JsonSerializable(typeof(TaskQueryResult))]
[JsonSerializable(typeof(Models.TaskStatus))]
[JsonSerializable(typeof(TaskStatusUpdateEvent))]
[JsonSerializable(typeof(TextPart))]
public partial class JsonSerializationContext
    : JsonSerializerContext
{



}