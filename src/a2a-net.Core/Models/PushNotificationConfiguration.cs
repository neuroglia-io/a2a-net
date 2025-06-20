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
/// Represents an object used to configure push notifications.
/// </summary>
[Description("An object used to configure push notifications.")]
[DataContract]
public record PushNotificationConfiguration
{

    /// <summary>
    /// Gets or sets the absolute HTTPS webhook URL for the A2A Server to POST task updates to.
    /// </summary>
    [Description("The absolute HTTPS webhook URL for the A2A Server to POST task updates to.")]
    [Required]
    [DataMember(Name = "url", Order = 1), JsonPropertyName("url"), JsonPropertyOrder(1), YamlMember(Alias = "url", Order = 1)]
    public virtual Uri Url { get; set; } = null!;

    /// <summary>
    /// Gets or sets an optional client-generated opaque token for the client's webhook receiver to validate the notification.
    /// </summary>
    [Description("An optional client-generated opaque token for the client's webhook receiver to validate the notification.")]
    [DataMember(Name = "token", Order = 2), JsonPropertyName("token"), JsonPropertyOrder(2), YamlMember(Alias = "token", Order = 2)]
    public virtual string? Token { get; set; }

    /// <summary>
    /// Gets or sets authentication details the A2A Server must use when calling the url. The client's webhook (receiver) defines these requirements.
    /// </summary>
    [Description("Authentication details the A2A Server must use when calling the url. The client's webhook (receiver) defines these requirements.")]
    [DataMember(Name = "authentication", Order = 3), JsonPropertyName("authentication"), JsonPropertyOrder(3), YamlMember(Alias = "authentication", Order = 3)]
    public virtual PushNotificationAuthenticationInfo? Authentication { get; set; }

}
