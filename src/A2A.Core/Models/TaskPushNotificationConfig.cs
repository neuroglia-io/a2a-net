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
/// Represents an object used to configure push notifications for task updates.
/// </summary>
[Description("Represents an object used to configure push notifications for task updates.")]
[DataContract]
public sealed record TaskPushNotificationConfig
{

    /// <summary>
    /// Gets the name of the task push notification configuration. Follows the format 'tasks/{task_id}/pushNotificationConfigs/{config_id}'.
    /// </summary>
    [Description("The name of the task push notification configuration. Follows the format 'tasks/{task_id}/pushNotificationConfigs/{config_id}'.")]
    [Required, MinLength(1)]
    [DataMember(Order = 1, Name = "name"), JsonPropertyOrder(1), JsonPropertyName("name")]
    public required string Name { get; init; }

    /// <summary>
    /// Gets the push notification configuration.
    /// </summary>
    [Description("The push notification configuration.")]
    [Required]
    [DataMember(Order = 2, Name = "config"), JsonPropertyOrder(2), JsonPropertyName("config")]
    public required PushNotificationConfig PushNotificationConfig { get; init; }

}
