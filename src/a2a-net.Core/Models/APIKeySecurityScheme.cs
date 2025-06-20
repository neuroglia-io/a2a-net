// Copyright � 2025-Present the a2a-net Authors
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
[DataContract]
public record APIKeySecurityScheme 
    : SecurityScheme
{

    /// <inheritdoc />
    [IgnoreDataMember, JsonIgnore, YamlIgnore]
    public override string Type => SecuritySchemeType.ApiKey;

    /// <summary>
    /// Gets or sets the name of the header or query parameter to be used for the API key.
    /// </summary>
    [Required, MinLength(1)]
    [DataMember(Name = "name", Order = 1), JsonPropertyName("name"), JsonPropertyOrder(1), YamlMember(Alias = "name", Order = 1)]
    public virtual string Name { get; set; } = null!;

    /// <summary>
    /// Gets or sets the location of the API key (header, query, etc.).
    /// </summary>
    [Required, MinLength(1), AllowedValues(ApiKeyLocation.Cookie, ApiKeyLocation.Header, ApiKeyLocation.Query)]
    [DataMember(Name = "in", Order = 2), JsonPropertyName("in"), JsonPropertyOrder(2), YamlMember(Alias = "in", Order = 2)]
    public virtual string In { get; set; } = null!;

}
