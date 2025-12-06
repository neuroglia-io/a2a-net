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
/// Represents a protocol extension supported by an agent.
/// </summary>
[Description("Represents a protocol extension supported by an agent.")]
[DataContract]
public sealed record AgentExtension
{

    /// <summary>
    /// Gets the URI, if any, used to uniquely identify the extension.
    /// </summary>
    [Description("The URI, if any, used to uniquely identify the extension.")]
    [DataMember(Order = 1, Name = "uri"), JsonPropertyOrder(1), JsonPropertyName("uri")]
    public Uri? Uri { get; init; }

    /// <summary>
    /// Gets a human readable description of the extension, if any.
    /// </summary>
    [Description("A human readable description of the extension, if any.")]
    [DataMember(Order = 2, Name = "description"), JsonPropertyOrder(2), JsonPropertyName("description")]
    public string? Description { get; init; }

    /// <summary>
    /// Gets a boolean indicating whether the client must understand and comply with the extension's requirements.
    /// </summary>
    [Description("A boolean indicating whether the client must understand and comply with the extension's requirements.")]
    [DataMember(Order = 3, Name = "required"), JsonPropertyOrder(3), JsonPropertyName("required")]
    public bool? Required { get; init; }

    /// <summary>
    /// Gets optional, extension-specific configuration parameters.
    /// </summary>
    [Description("Optional, extension-specific configuration parameters.")]
    [DataMember(Order = 4, Name = "params"), JsonPropertyOrder(4), JsonPropertyName("params")]
    public IReadOnlyDictionary<string, JsonNode>? Params { get; init; }

}