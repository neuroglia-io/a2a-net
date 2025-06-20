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
/// Represents the configuration for a <see cref="SendMessageRequest"/>.
/// </summary>
[Description("Represents the configuration for a SendMessageRequest.")]
[DataContract]
public record SendMessageRequestConfiguration
{

    /// <summary>
    /// Gets or sets a list of output modes, if any, that the agent is allowed to use when responding to the message.
    /// </summary>
    [Description("A list of output modes, if any, that the agent is allowed to use when responding to the message.")]
    [DataMember(Name = "acceptedOutputModes", Order = 1), JsonPropertyName("acceptedOutputModes"), JsonPropertyOrder(1), YamlMember(Alias = "acceptedOutputModes", Order = 1)]
    public virtual EquatableList<string>? AcceptedOutputModes { get; set; }

    /// <summary>
    /// Gets or sets the maximum number, if any, of recent messages to be retrieved.
    /// </summary>
    [Description("The maximum number, if any, of recent messages to be retrieved.")]
    [DataMember(Name = "historyLength", Order = 2), JsonPropertyName("historyLength"), JsonPropertyOrder(2), YamlMember(Alias = "historyLength", Order = 2)]
    public virtual uint? HistoryLength { get; set; }

    /// <summary>
    /// Gets or sets the push notification configuration, if any, to be used for the request.
    /// </summary>
    [Description("The push notification configuration, if any, to be used for the request.")]
    [DataMember(Name = "pushNotificationConfig", Order = 3), JsonPropertyName("pushNotificationConfig"), JsonPropertyOrder(3), YamlMember(Alias = "pushNotificationConfig", Order = 3)]
    public virtual PushNotificationConfiguration? PushNotificationConfig { get; set; }

    /// <summary>
    /// Gets or sets a boolean indicating whether or not the server should block the request until a response is available.
    /// </summary>
    [Description("A boolean indicating whether or not the server should block the request until a response is available.")]
    [DataMember(Name = "blocking", Order = 4), JsonPropertyName("blocking"), JsonPropertyOrder(4), YamlMember(Alias = "blocking", Order = 4)]
    public virtual bool Blocking { get; set; }

}
