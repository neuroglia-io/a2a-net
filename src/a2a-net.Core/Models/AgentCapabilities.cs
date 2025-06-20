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
/// Represents an object used to describe an agent's capabilities.
/// </summary>
[Description("An object used to describe an agent's capabilities.")]
[DataContract]
public record AgentCapabilities
{

    /// <summary>
    /// Gets or sets a boolean indicating whether or not the agent supports streaming.
    /// </summary>
    [Description("A boolean indicating whether or not the agent supports streaming.")]
    [DataMember(Name = "streaming", Order = 1), JsonPropertyName("streaming"), JsonPropertyOrder(1), YamlMember(Alias = "streaming", Order = 1)]
    public virtual bool Streaming { get; set; }

    /// <summary>
    /// Gets or sets a boolean indicating whether or not the agent can notify updates to client using push notifications.
    /// </summary>
    [Description("A boolean indicating whether or not the agent can notify updates to client using push notifications.")]
    [DataMember(Name = "pushNotifications", Order = 2), JsonPropertyName("pushNotifications"), JsonPropertyOrder(2), YamlMember(Alias = "pushNotifications", Order = 2)]
    public virtual bool PushNotifications { get; set; }

    /// <summary>
    /// Gets or sets a boolean indicating whether or not the agent exposes status change history for tasks.
    /// </summary>
    [Description("A boolean indicating whether or not the agent exposes status change history for tasks.")]
    [DataMember(Name = "stateTransitionHistory", Order = 3), JsonPropertyName("stateTransitionHistory"), JsonPropertyOrder(3), YamlMember(Alias = "stateTransitionHistory", Order = 3)]
    public virtual bool StateTransitionHistory { get; set; }

    /// <summary>
    /// Gets or sets a list of extensions, if any, supported by this agent.
    /// </summary>
    [Description("A list of extensions, if any, supported by this agent.")]
    [DataMember(Name = "extensions", Order = 4), JsonPropertyName("extensions"), JsonPropertyOrder(4), YamlMember(Alias = "extensions", Order = 4)]
    public virtual EquatableList<AgentExtension>? Extensions { get; set; }

}
