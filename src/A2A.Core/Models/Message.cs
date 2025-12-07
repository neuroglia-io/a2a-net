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

namespace A2A.Models;

/// <summary>
/// Represents a message.
/// </summary>
[Description("Represents a message.")]
[DataContract]
public sealed record Message
    : Response
{

    /// <summary>
    /// Gets the message's unique identifier.
    /// </summary>
    [Description("The message's unique identifier.")]
    [Required, MinLength(1)]
    [DataMember(Order = 1, Name = "messageId"), JsonPropertyOrder(1), JsonPropertyName("messageId")]
    public string MessageId { get; init; } = Guid.NewGuid().ToString("N");

    /// <summary>
    /// Gets the unique identifier of the context, if any, the message is associated with.
    /// </summary>
    [Description("The unique identifier of the context, if any, the message is associated with.")]
    [DataMember(Order = 2, Name = "contextId"), JsonPropertyOrder(2), JsonPropertyName("contextId")]
    public string? ContextId { get; init; }

    /// <summary>
    /// Gets the unique identifier of the task, if any, the message is associated with.
    /// </summary>
    [Description("The unique identifier of the task, if any, the message is associated with.")]
    [DataMember(Order = 3, Name = "taskId"), JsonPropertyOrder(3), JsonPropertyName("taskId")]
    public string? TaskId { get; init; }

    /// <summary>
    /// Gets the role of the message sender.
    /// </summary>
    [Description("The role of the message sender.")]
    [Required, AllowedValues(A2A.Role.Agent, A2A.Role.Unspecified, A2A.Role.User)]
    [DataMember(Order = 4, Name = "role"), JsonPropertyOrder(4), JsonPropertyName("role")]
    public required string Role { get; init; }

    /// <summary>
    /// Gets a collection containing the parts the message is composed of.
    /// </summary>
    [Description("A collection containing the parts the message is composed of.")]
    [Required, MinLength(1)]
    [DataMember(Order = 5, Name = "parts"), JsonPropertyOrder(5), JsonPropertyName("parts")]
    public required IReadOnlyCollection<Part> Parts { get; init; }

    /// <summary>
    /// Gets a key/value mapping, if any, containing metadata associated with the message.
    /// </summary>
    [Description("A key/value mapping, if any, containing metadata associated with the message.")]
    [DataMember(Order = 6, Name = "metadata"), JsonPropertyOrder(6), JsonPropertyName("metadata")]
    public JsonObject? Metadata { get; init; }

    /// <summary>
    /// Gets a collection containing the URIs of the extensions, if any, used to generate the message.
    /// </summary>
    [Description("A collection containing the URIs of the extensions, if any, used to generate the message.")]
    [DataMember(Order = 7, Name = "extensions"), JsonPropertyOrder(7), JsonPropertyName("extensions")]
    public IReadOnlyCollection<Uri>? Extensions { get; init; }

    /// <summary>
    /// Gets a collection containing the unique identifiers of the tasks, if any, referenced by the message.
    /// </summary>
    [Description("A collection containing the unique identifiers of the tasks, if any, referenced by the message.")]
    [DataMember(Order = 8, Name = "referencedTaskIds"), JsonPropertyOrder(8), JsonPropertyName("referencedTaskIds")]
    public IReadOnlyCollection<string>? ReferencedTaskIds { get; init; }

}
