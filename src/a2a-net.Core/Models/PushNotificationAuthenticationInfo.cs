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
/// Represents an object used to define authentication for push notifications.
/// </summary>
[Description("Represents an object used to define authentication for push notifications.")]
[DataContract]
public record PushNotificationAuthenticationInfo
{

    /// <summary>
    /// Gets or sets the list of authentication schemes supported.
    /// </summary>
    [Description("The list of authentication schemes supported.")]
    [Required, MinLength(1)]
    [DataMember(Name = "role", Order = 1), JsonPropertyName("role"), JsonPropertyOrder(1), YamlMember(Alias = "role", Order = 1)]
    public virtual EquatableList<string> Schemes { get; set; } = null!;

    /// <summary>
    /// Gets or sets the credentials, if any, used in conjunction with the specified authentication schemes.
    /// </summary>
    [Description("The credentials, if any, used in conjunction with the specified authentication schemes.")]
    [DataMember(Name = "credentials", Order = 2), JsonPropertyName("credentials"), JsonPropertyOrder(2), YamlMember(Alias = "credentials", Order = 2)]
    public virtual string? Credentials { get; set; }

}