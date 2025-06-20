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
/// Represents an object used to describe an agent's provider.
/// </summary>
[Description("An object used to describe an agent's provider.")]
[DataContract]
public record AgentProvider
{

    /// <summary>
    /// Gets or sets the name of the organization that provides or maintains the agent.
    /// </summary>
    [Description("The name of the organization that provides or maintains the agent.")]
    [Required, MinLength(1)]
    [DataMember(Name = "organization", Order = 1), JsonPropertyName("organization"), JsonPropertyOrder(1), YamlMember(Alias = "organization", Order = 1)]
    public virtual string Organization { get; set; } = null!;

    /// <summary>
    /// Gets or sets a url, if any, referencing the official website of the agent's organization or provider.
    /// </summary>
    [Description("A url, if any, referencing the official website of the agent's organization or provider.")]
    [DataMember(Name = "url", Order = 2), JsonPropertyName("url"), JsonPropertyOrder(2), YamlMember(Alias = "url", Order = 2)]
    public virtual Uri? Url { get; set; }

}