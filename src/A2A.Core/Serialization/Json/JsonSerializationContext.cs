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
[JsonSerializable(typeof(StreamResponse))]
[JsonSerializable(typeof(Models.Task))]
[JsonSerializable(typeof(TaskEvent))]
[JsonSerializable(typeof(TaskArtifactUpdateEvent))]
[JsonSerializable(typeof(TaskQueryOptions))]
[JsonSerializable(typeof(TaskQueryResult))]
[JsonSerializable(typeof(Models.TaskStatus))]
[JsonSerializable(typeof(TaskStatusUpdateEvent))]
[JsonSerializable(typeof(TextPart))]
public partial class JsonSerializationContext
    : JsonSerializerContext
{



}