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
/// Represents the result of a push notification configuration query.
/// </summary>
[Description("Represents the result of a push notification configuration query.")]
[DataContract]
public sealed record PushNotificationConfigQueryResult
{

    /// <summary>
    /// Gets a collection containing the push notification configurations matching the specified criteria.
    /// </summary>
    [Description("A collection containing the push notification configurations matching the specified criteria.")]
    [Required]
    [DataMember(Order = 1, Name = "configs"), JsonPropertyOrder(1), JsonPropertyName("configs")]
    public required IReadOnlyList<TaskPushNotificationConfig> Configs { get; init; }

    /// <summary>
    /// Gets the token, if any, used to retrieve the next page of results.
    /// </summary>
    [Description("The token, if any, used to retrieve the next page of results.")]
    [DataMember(Order = 2, Name = "nextPageToken"), JsonPropertyOrder(2), JsonPropertyName("nextPageToken")]
    public string? NextPageToken { get; init; }

}