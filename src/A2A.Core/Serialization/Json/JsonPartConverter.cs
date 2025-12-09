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
/// Represents a JSON converter used to read and write <see cref="Part"/>s.
/// </summary>
public sealed class JsonPartConverter 
    : JsonConverter<Part>
{

    /// <inheritdoc/>
    public override Part? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        using var doc = JsonDocument.ParseValue(ref reader);
        var root = doc.RootElement;
        if (root.TryGetProperty("text", out _)) return root.Deserialize(JsonSerializationContext.Default.TextPart);
        if (root.TryGetProperty("data", out _)) return root.Deserialize(JsonSerializationContext.Default.DataPart);
        if (root.TryGetProperty("fileWithBytes", out _) || root.TryGetProperty("fileWithUri", out _) || root.TryGetProperty("name", out _) || root.TryGetProperty("mediaType", out _)) return root.Deserialize(JsonSerializationContext.Default.FilePart);
        throw new JsonException("Unable to determine Part subtype (no discriminator and no known properties found).");
    }

    /// <inheritdoc/>
    public override void Write(Utf8JsonWriter writer, Part value, JsonSerializerOptions options)
    {
        switch (value)
        {
            case DataPart dataPart:
                JsonSerializer.Serialize(writer, dataPart, JsonSerializationContext.Default.DataPart);
                break;
            case FilePart filePart:
                JsonSerializer.Serialize(writer, filePart, JsonSerializationContext.Default.FilePart);
                break;
            case TextPart textPart:
                JsonSerializer.Serialize(writer, textPart, JsonSerializationContext.Default.TextPart);
                break;
            default:
                throw new JsonException($"The specified part type '{value.GetType().FullName}' is not supported for serialization.");
        }
    }

}
