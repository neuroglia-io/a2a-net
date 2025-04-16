// Copyright � 2025-Present the a2a-net Authors
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
/// Represents the <see cref="JsonConverter"/> used to serialize/deserialize <see cref="TaskEvent"/>s
/// </summary>
public class TaskEventJsonConverter 
    : JsonConverter<TaskEvent>
{

    /// <inheritdoc/>
    public override TaskEvent? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        using var doc = JsonDocument.ParseValue(ref reader);
        var root = doc.RootElement;
        if (root.TryGetProperty("status", out _)) return JsonSerializer.Deserialize<TaskStatusUpdateEvent>(root.GetRawText(), options);
        if (root.TryGetProperty("artifact", out _)) return JsonSerializer.Deserialize<TaskArtifactUpdateEvent>(root.GetRawText(), options);
        throw new JsonException("Unable to determine event type: no known discriminator property found.");
    }

    /// <inheritdoc/>
    public override void Write(Utf8JsonWriter writer, TaskEvent value, JsonSerializerOptions options) => JsonSerializer.Serialize(writer, value, value.GetType(), options);

}
