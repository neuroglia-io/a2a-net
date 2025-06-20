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
/// Represents the definition of an extension supported by an agent.
/// </summary>
[Description("The definition of an extension supported by an agent.")]
[DataContract]
public record AgentExtension
{

    /// <summary>
    /// Gets or sets the extension's URI.
    /// </summary>
    [Description("The extension's URI.")]
    [DataMember(Name = "uri", Order = 1), JsonPropertyName("uri"), JsonPropertyOrder(1), YamlMember(Alias = "uri", Order = 1)]
    public virtual Uri Uri { get; set; } = null!;

    /// <summary>
    /// Gets or set sa boolean indicating whether or not agent requires clients to follow some protocol logic specific to the extension.<para></para>
    /// Clients should expect failures when attempting to interact with a server that requires an extension the client does not support.
    /// </summary>
    [Description("A boolean indicating whether or not agent requires clients to follow some protocol logic specific to the extension. Clients should expect failures when attempting to interact with a server that requires an extension the client does not support.")]
    [DataMember(Name = "required", Order = 2), JsonPropertyName("required"), JsonPropertyOrder(2), YamlMember(Alias = "required", Order = 2)]
    public virtual bool Required { get; set; }

    /// <summary>
    /// Gets or sets a description, if any, of how the extension is used by the agent.
    /// </summary>
    [Description("A description, if any, of how the extension is used by the agent.")]
    [DataMember(Name = "description", Order = 3), JsonPropertyName("description"), JsonPropertyOrder(3), YamlMember(Alias = "description", Order = 3)]
    public virtual string? Description { get; set; }

    /// <summary>
    /// Gets or sets configuration parameters, if any, specific to the extension.
    /// </summary>
    [Description("Configuration parameters, if any, specific to the extension.")]
    [DataMember(Name = "params", Order = 4), JsonPropertyName("params"), JsonPropertyOrder(4), YamlMember(Alias = "params", Order = 4)]
    public virtual object? Params { get; set; }

}