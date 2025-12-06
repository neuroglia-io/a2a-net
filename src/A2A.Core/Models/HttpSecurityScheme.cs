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
/// Represents an HTTP authentication security scheme.
/// </summary>
[Description("Represents an HTTP authentication security scheme.")]
[DataContract]
public sealed record HttpSecurityScheme
    : SecurityScheme
{

    /// <inheritdoc />
    [IgnoreDataMember, JsonIgnore]
    public override string Type => SecuritySchemeType.Http;

    /// <summary>
    /// Gets or sets the HTTP authentication scheme.
    /// </summary>
    [Description("The HTTP authentication scheme.")]
    [Required, MinLength(1), AllowedValues(HttpSecuritySchemeType.Basic, HttpSecuritySchemeType.Bearer)]
    [DataMember(Order = 1, Name = "scheme"), JsonPropertyOrder(1), JsonPropertyName("scheme")]
    public string Scheme { get; set; } = null!;

    /// <summary>
    /// Gets or sets a hint to the client to identify how the bearer token is formatted. Applies only when scheme is 'bearer'.
    /// </summary>
    [Description("A hint to the client to identify how the bearer token is formatted. Applies only when scheme is 'bearer'.")]
    [DataMember(Order = 2, Name = "bearerFormat"), JsonPropertyOrder(2), JsonPropertyName("bearerFormat")]
    public string? BearerFormat { get; set; }

}