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
/// Represents information about push notification authentication.
/// </summary>
[Description("Represents information about push notification authentication.")]
[DataContract]
public sealed record AuthenticationInfo
{

    /// <summary>
    /// Gets a collection of schemes used for authentication.
    /// </summary>
    [Description("A collection of schemes used for authentication.")]
    [Required, MinLength(1)]
    [DataMember(Order = 1, Name = "schemes"), JsonPropertyOrder(1), JsonPropertyName("schemes")]
    public required IReadOnlyCollection<string> Schemes { get; init; }

    /// <summary>
    /// Gets credentials used for authentication, if any.
    /// </summary>
    [Description("Credentials used for authentication, if any.")]
    [DataMember(Order = 2, Name = "credentials"), JsonPropertyOrder(2), JsonPropertyName("credentials")]
    public string? Credentials { get; init; }

}