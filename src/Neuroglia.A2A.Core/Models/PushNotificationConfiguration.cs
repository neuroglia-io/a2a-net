// Copyright � 2025-Present Neuroglia SRL
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

namespace Neuroglia.A2A.Models;

/// <summary>
/// Represents an object used to configure push notifications
/// </summary>
[DataContract]
public record PushNotificationConfiguration
{

    /// <summary>
    /// Gets/sets the endpoint URL to which the push notification should be sent
    /// </summary>
    [Required]
    [DataMember(Name = "url", Order = 1), JsonPropertyName("url"), JsonPropertyOrder(1), YamlMember(Alias = "url", Order = 1)]
    public virtual Uri Url { get; set; } = null!;

    /// <summary>
    /// Gets/sets a token;, if any, that uniquely identifies the task or session associated with the push notification
    /// </summary>
    [DataMember(Name = "token", Order = 2), JsonPropertyName("token"), JsonPropertyOrder(2), YamlMember(Alias = "token", Order = 2)]
    public virtual string? Token { get; set; }

    /// <summary>
    /// Gets/sets information, if any, about the authentication used to push notification to the configured endpoint
    /// </summary>
    [DataMember(Name = "authentication", Order = 3), JsonPropertyName("authentication"), JsonPropertyOrder(3), YamlMember(Alias = "authentication", Order = 3)]
    public virtual AuthenticationInfo? Authentication { get; set; }

}
