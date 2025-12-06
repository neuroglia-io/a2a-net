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
public sealed record PushNotificationConfig
{

    /// <summary>
    /// Gets the configuration's unique identifier, if any.
    /// </summary>
    [Description("The configuration's unique identifier, if any.")]
    [DataMember(Order = 1, Name = "id"), JsonPropertyOrder(1), JsonPropertyName("id")]
    public string? Id { get; init; }

    /// <summary>
    /// Gets the URL to which to send push notifications.
    /// </summary>
    [Description("The URL to which to send push notifications.")]
    [DataMember(Order = 2, Name = "url"), JsonPropertyOrder(2), JsonPropertyName("url")]
    public required Uri Url { get; init; }

    /// <summary>
    /// Gets the token used to authenticate push notifications, if any.
    /// </summary>
    [Description("The token used to authenticate push notifications, if any.")]
    [DataMember(Order = 3, Name = "token"), JsonPropertyOrder(3), JsonPropertyName("token")]
    public string? Token { get; init; }

    /// <summary>
    /// Gets information about the authentication used for push notifications, if any.
    /// </summary>
    [Description("Information about the authentication used for push notifications, if any.")]
    [DataMember(Order = 4, Name = "authentication"), JsonPropertyOrder(4), JsonPropertyName("authentication")]
    public AuthenticationInfo? Authentication { get; init; }

}
