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

namespace A2A.Models;

/// <summary>
/// Represents a task
/// </summary>
[DataContract]
public record Task
{

    /// <summary>
    /// Gets or sets the task's unique identifier
    /// </summary>
    [Required, MinLength(1)]
    [DataMember(Name = "id", Order = 1), JsonPropertyName("id"), JsonPropertyOrder(1), YamlMember(Alias = "id", Order = 1)]
    public virtual string Id { get; set; } = null!;

    /// <summary>
    /// Gets or sets the unique identifier of the session holding the task
    /// </summary>
    [Required, MinLength(1)]
    [DataMember(Name = "sessionId", Order = 2), JsonPropertyName("sessionId"), JsonPropertyOrder(2), YamlMember(Alias = "sessionId", Order = 2)]
    public virtual string SessionId { get; set; } = null!;

    /// <summary>
    /// Gets or sets the task's status
    /// </summary>
    [Required]
    [DataMember(Name = "status", Order = 3), JsonPropertyName("status"), JsonPropertyOrder(3), YamlMember(Alias = "status", Order = 3)]
    public virtual TaskStatus Status { get; set; } = null!;

    /// <summary>
    /// Gets or sets the history of all the task's messages
    /// </summary>
    [DataMember(Name = "history", Order = 4), JsonPropertyName("history"), JsonPropertyOrder(4), YamlMember(Alias = "history", Order = 4)]
    public virtual EquatableList<Message>? History { get; set; }

    /// <summary>
    /// Gets or sets a collection of the artifacts, if any, created by the agent
    /// </summary>
    [DataMember(Name = "artifacts", Order = 5), JsonPropertyName("artifacts"), JsonPropertyOrder(5), YamlMember(Alias = "artifacts", Order = 5)]
    public virtual EquatableList<Artifact>? Artifacts { get; set; }

    /// <summary>
    /// Gets or sets a key/value mapping that contains the task's additional properties, if any
    /// </summary>
    [DataMember(Name = "metadata", Order = 99), JsonPropertyName("metadata"), JsonPropertyOrder(99), YamlMember(Alias = "metadata", Order = 99)]
    public virtual EquatableDictionary<string, object>? Metadata { get; set; }

}