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
/// Represents a request to set or update a push notification configuration.
/// </summary>
[Description("Represents a request to set or update a push notification configuration.")]
[DataContract]
public sealed record SetOrUpdatePushNotificationConfigRequest
{

    /// <summary>
    /// Gets the identifier of the tenant, if any, to which the task to set or update the push notification configuration belongs.
    /// </summary>
    [Description("The identifier of the tenant, if any, to which the task to set or update the push notification configuration belongs.")]
    [DataMember(Order = 1, Name = "tenant"), JsonPropertyOrder(1), JsonPropertyName("tenant")]
    public string? Tenant { get; init; }

    /// <summary>
    /// Gets a reference to the parent task to set or update the push notification configuration for.
    /// </summary>
    [Description("A reference to the parent task to set or update the push notification configuration for.")]
    [Required, MinLength(1)]
    [DataMember(Order = 2, Name = "parent"), JsonPropertyOrder(2), JsonPropertyName("parent")]
    public required string Parent { get; init; }

    /// <summary>
    /// Gets the unique identifier of the push notification configuration to set or update.
    /// </summary>
    [Description("The unique identifier of the push notification configuration to set or update.")]
    [Required, MinLength(1)]
    [DataMember(Order = 3, Name = "configId"), JsonPropertyOrder(3), JsonPropertyName("configId")]
    public required string ConfigId { get; init; }

    /// <summary>
    /// Gets the push notification configuration to set or update.
    /// </summary>
    [Description("The push notification configuration to set or update.")]
    [Required]
    [DataMember(Order = 4, Name = "config"), JsonPropertyOrder(4), JsonPropertyName("config")]
    public required PushNotificationConfig Config { get; init; }

}
