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
/// Represents the options used to configure a push notification configuration query.
/// </summary>
[Description("Represents the options used to configure a push notification configuration query.")]
[DataContract]
public sealed record TaskPushNotificationConfigQueryOptions
{

    /// <summary>
    /// Gets the unique identifier, if any, of the task the push notifications to get belong to.
    /// </summary>
    [Description("The unique identifier, if any, of the task the push notifications to get belong to.")]
    [DataMember(Order = 1, Name = "taskId"), JsonPropertyOrder(1), JsonPropertyName("taskId")]
    public string? TaskId { get; init; }

    /// <summary>
    /// Gets the maximum number, if any, of push notification configurations to return.
    /// </summary>
    [Description("The maximum number, if any, of push notification configurations to return.")]
    [DataMember(Order = 2, Name = "pageSize"), JsonPropertyOrder(2), JsonPropertyName("pageSize")]
    public uint? PageSize { get; init; }

    /// <summary>
    /// Gets the token, if any, used to retrieve the next page of results.
    /// </summary>
    [Description("The token, if any, used to retrieve the next page of results.")]
    [DataMember(Order = 3, Name = "pageToken"), JsonPropertyOrder(3), JsonPropertyName("pageToken")]
    public string? PageToken { get; init; }

    /// <summary>
    /// Gets the identifier of the tenant, if any, that owns the push notification configurations to get.
    /// </summary>
    [Description("The identifier of the tenant, if any, that owns the push notification configurations to get.")]
    [DataMember(Order = 4, Name = "tenant"), JsonPropertyOrder(4), JsonPropertyName("tenant")]
    public string? Tenant { get; init; }

}
