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
[Description("Represents an object used to describe an agent's capabilities.")]
[DataContract]
public sealed record AgentCapabilities
{

    /// <summary>
    /// Gets a boolean indicating whether the agent supports streaming responses.
    /// </summary>
    [Description("A boolean indicating whether the agent supports streaming responses.")]
    [DataMember(Order = 1, Name = "streaming"), JsonPropertyOrder(1), JsonPropertyName("streaming")]
    public bool? Streaming { get; set; }

    /// <summary>
    /// Gets a boolean indicating whether the agent supports push notifications.
    /// </summary>
    [Description("A boolean indicating whether the agent supports push notifications.")]
    [DataMember(Order = 2, Name = "pushNotifications"), JsonPropertyOrder(2), JsonPropertyName("pushNotifications")]
    public bool? PushNotifications { get; set; }

    /// <summary>
    /// Gets a boolean indicating whether the agent maintains a state transition history.
    /// </summary>
    [Description("A boolean indicating whether the agent maintains a state transition history.")]
    [DataMember(Order = 3, Name = "stateTransitionHistory"), JsonPropertyOrder(3), JsonPropertyName("stateTransitionHistory")]
    public bool? StateTransitionHistory { get; set; }

    /// <summary>
    /// Gets a collection containing the agent's extensions, if any.
    /// </summary>
    [Description("A collection containing the agent's extensions, if any.")]
    [DataMember(Order = 4, Name = "extensions"), JsonPropertyOrder(4), JsonPropertyName("extensions")]
    public ICollection<AgentExtension>? Extensions { get; set; }

}
