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
/// Represents an object used to configure task-related push notifications.
/// </summary>
[Description("Represents an object used to configure task-related push notifications.")]
[DataContract]
public record TaskPushNotificationConfiguration
    : RpcRequestParameters
{

    /// <summary>
    /// Gets or sets the ID of the task to configure push notifications for, or retrieve configuration from.
    /// </summary>
    [Description("The ID of the task to configure push notifications for, or retrieve configuration from.")]
    [Required, MinLength(1)]
    [DataMember(Name = "id", Order = 1), JsonPropertyName("id"), JsonPropertyOrder(1), YamlMember(Alias = "id", Order = 1)]
    public virtual string Id { get; set; } = null!;

    /// <summary>
    /// Gets or sets the push notification configuration.
    /// </summary>
    [Description("The push notification configuration.")]
    [DataMember(Name = "pushNotificationConfig", Order = 2), JsonPropertyName("pushNotificationConfig"), JsonPropertyOrder(2), YamlMember(Alias = "pushNotificationConfig", Order = 2)]
    public virtual PushNotificationConfiguration? PushNotificationConfig { get; set; }

}