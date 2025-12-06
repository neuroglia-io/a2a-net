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
/// Represents a JSON converter used to read and write <see cref="Response"/>s.
/// </summary>
public sealed class JsonResponseConverter
    : JsonConverter<Response>
{

    /// <inheritdoc/>
    public override Response? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        using var doc = JsonDocument.ParseValue(ref reader);
        var root = doc.RootElement;
        if (root.TryGetProperty("messageId", out _)) return root.Deserialize(JsonSerializationContext.Default.Message);
        if (root.TryGetProperty("id", out _) && root.TryGetProperty("status", out _)) return root.Deserialize(JsonSerializationContext.Default.Task);
        throw new JsonException("Unable to determine the specified output type (no discriminator and no known properties found).");
    }

    /// <inheritdoc/>
    public override void Write(Utf8JsonWriter writer, Response value, JsonSerializerOptions options)
    {
        switch (value)
        {
            case Message message:
                JsonSerializer.Serialize(writer, message, JsonSerializationContext.Default.Message);
                break;
            case Models.Task task:
                JsonSerializer.Serialize(writer, task, JsonSerializationContext.Default.Task);
                break;
            default:
                throw new JsonException($"The specified output type '{value.GetType().FullName}' is not supported for serialization.");
        }
    }

}