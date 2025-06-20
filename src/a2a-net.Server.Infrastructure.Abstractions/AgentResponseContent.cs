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

namespace A2A.Server.Infrastructure;

/// <summary>
/// Represents content returned by an agent as part of its response to a task's execution
/// </summary>
[DataContract]
public record AgentResponseContent
{

    /// <summary>
    /// Initializes a new <see cref="AgentResponseContent"/>
    /// </summary>
    [JsonConstructor]
    protected AgentResponseContent() { }

    /// <summary>
    /// Initializes a new <see cref="AgentResponseContent"/>
    /// </summary>
    /// <param name="artifact">The artifact produced by the agent</param>
    public AgentResponseContent(Artifact artifact)
    {
        ArgumentNullException.ThrowIfNull(artifact);
        Artifact = artifact;
    }

    /// <summary>
    /// Initializes a new <see cref="AgentResponseContent"/>
    /// </summary>
    /// <param name="message">The message produced by the agent</param>
    public AgentResponseContent(Message message)
    {
        ArgumentNullException.ThrowIfNull(message);
        Message = message;
    }

    /// <summary>
    /// Gets the type of the response content, indicating whether it is an artifact or a message
    /// </summary>
    [IgnoreDataMember, JsonIgnore, YamlIgnore]
    public virtual string Type => Artifact != null ? AgentResponseContentType.Artifact : AgentResponseContentType.Message;

    /// <summary>
    /// Gets the artifact produced by the agent, if any
    /// </summary>
    [DataMember(Name = "notifications", Order = 1), JsonInclude, JsonPropertyName("notifications"), JsonPropertyOrder(1), YamlMember(Alias = "notifications", Order = 1)]
    public virtual Artifact? Artifact { get; protected set; }

    /// <summary>
    /// Gets the message produced by the agent, if any
    /// </summary>
    [DataMember(Name = "message", Order = 1), JsonInclude, JsonPropertyName("message"), JsonPropertyOrder(1), YamlMember(Alias = "message", Order = 1)]
    public virtual Message? Message { get; protected set; }

}
