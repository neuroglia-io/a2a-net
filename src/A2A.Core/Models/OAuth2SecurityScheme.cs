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
/// Represents an OAuth2 security scheme.
/// </summary>
[Description("An OAuth2 security scheme.")]
[DataContract]
public sealed record OAuth2SecurityScheme
    : SecurityScheme
{

    /// <inheritdoc/>
    [IgnoreDataMember, JsonIgnore]
    public override string Type => SecuritySchemeType.OAuth2;

    /// <summary>
    /// Gets or sets the OAuth2 flow definitions for this security scheme.
    /// </summary>
    [Description("The OAuth2 flow definitions for this security scheme.")]
    [Required]
    [DataMember(Order = 1, Name = "flows"), JsonPropertyOrder(1), JsonPropertyName("flows")]
    public OAuthFlows Flows { get; set; } = null!;

}
