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
/// Represents the parameters for the 'GetPushNotificationConfig' JSON-RPC method.
/// </summary>
[Description("Represents the parameters for the 'GetPushNotificationConfig' JSON-RPC method.")]
[DataContract]
public sealed record GetPushNotificationConfigMethodParameters
{

    /// <summary>
    /// Gets the unique identifier of the task for which to retrieve the push notification configuration.
    /// </summary>
    [Description("The unique identifier of the task for which to retrieve the push notification configuration.")]
    [Required, MinLength(1)]
    [DataMember(Order = 1, Name = "taskId"), JsonPropertyOrder(1), JsonPropertyName("taskId")]
    public required string TaskId { get; init; }

    /// <summary>
    /// Gets the unique identifier of the push notification configuration to retrieve.
    /// </summary>
    [Description("The unique identifier of the push notification configuration to retrieve.")]
    [Required, MinLength(1)]
    [DataMember(Order = 2, Name = "configId"), JsonPropertyOrder(2), JsonPropertyName("configId")]
    public required string ConfigId { get; init; }

    /// <summary>
    /// Gets the identifier of the tenant, if any, the task to which the push notification configuration to get belongs.
    /// </summary>
    [Description("The identifier of the tenant, if any, the task to which the push notification configuration to get belongs.")]
    [DataMember(Order = 3, Name = "tenant"), JsonPropertyOrder(3), JsonPropertyName("tenant")]
    public string? Tenant { get; init; }

}
