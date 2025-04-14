using Neuroglia.A2A.Events;
using System.Text.Json;

namespace Neuroglia.A2A.Serialization.Json;

/// <summary>
/// Represents the <see cref="JsonConverter"/> used to serialize/deserialize <see cref="RpcEvent"/>s
/// </summary>
public class RpcEventJsonConverter 
    : JsonConverter<RpcEvent>
{

    /// <inheritdoc/>
    public override RpcEvent? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        using var doc = JsonDocument.ParseValue(ref reader);
        var root = doc.RootElement;
        if (root.TryGetProperty("status", out _)) return JsonSerializer.Deserialize<TaskStatusUpdateEvent>(root.GetRawText(), options);
        if (root.TryGetProperty("artifact", out _)) return JsonSerializer.Deserialize<TaskArtifactUpdateEvent>(root.GetRawText(), options);
        throw new JsonException("Unable to determine event type: no known discriminator property found.");
    }

    /// <inheritdoc/>
    public override void Write(Utf8JsonWriter writer, RpcEvent value, JsonSerializerOptions options) => JsonSerializer.Serialize(writer, value, value.GetType(), options);

}
