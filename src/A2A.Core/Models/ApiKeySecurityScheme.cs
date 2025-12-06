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
/// Represents an API key security scheme.
/// </summary>
[Description("Represents an API key security scheme.")]
[DataContract]
public sealed record ApiKeySecurityScheme
    : SecurityScheme
{

    /// <inheritdoc />
    [IgnoreDataMember, JsonIgnore]
    public override string Type => SecuritySchemeType.ApiKey;

    /// <summary>
    /// Gets the name of the header or query parameter to be used for the API key.
    /// </summary>
    [Description("The name of the header or query parameter to be used for the API key.")]
    [Required, MinLength(1)]
    [DataMember(Order = 1, Name = "name"), JsonPropertyOrder(1), JsonPropertyName("name")]
    public string Name { get; set; } = null!;

    /// <summary>
    /// Gets the location of the API key.
    /// </summary>
    [Description("The location of the API key.")]
    [Required, AllowedValues(ApiKeyLocation.Cookie, ApiKeyLocation.Header, ApiKeyLocation.Query)]
    [DataMember(Order = 2, Name = "in"), JsonPropertyOrder(2), JsonPropertyName("in")]
    public string In { get; set; } = null!;

}
