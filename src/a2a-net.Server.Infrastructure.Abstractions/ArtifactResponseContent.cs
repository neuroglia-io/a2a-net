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

using System.ComponentModel;

namespace A2A.Server.Infrastructure;

/// <summary>
/// Represents an agent response that includes an artifact.
/// </summary>
[Description("Represents an agent response that includes an artifact.")]
[DataContract]
public record ArtifactResponseContent 
    : AgentResponseContent
{

    /// <summary>
    /// Initializes a new <see cref="ArtifactResponseContent"/>.
    /// </summary>
    public ArtifactResponseContent() { }

    /// <summary>
    /// Initializes a new <see cref="ArtifactResponseContent"/>.
    /// </summary>
    /// <param name="artifact">The artifact produced by the agent.</param>
    /// <param name="append">Indicates whether the artifact update should be appended to the existing artifact.</param>
    /// <param name="lastChunk">Indicates whether this is the last chunk of the artifact update.</param>
    public ArtifactResponseContent(Artifact artifact, bool append, bool lastChunk)
    {
        ArgumentNullException.ThrowIfNull(artifact);
        Artifact = artifact;
        Append = append;
        LastChunk = lastChunk;
    }

    /// <inheritdoc />
    [IgnoreDataMember, JsonIgnore, YamlIgnore]
    public override string Type => AgentResponseContentType.Artifact;

    /// <summary>
    /// Gets the artifact produced by the agent.
    /// </summary>
    [Description("The artifact produced by the agent.")]
    [DataMember(Name = "artifact", Order = 1), JsonPropertyName("artifact"), JsonPropertyOrder(1), YamlMember(Alias = "artifact", Order = 1)]
    public virtual Artifact Artifact { get; init; } = null!;

    /// <summary>
    /// Gets or sets a value indicating whether the artifact update should be appended to the existing artifact.
    /// </summary>
    [Description("Indicates whether the artifact update should be appended to the existing artifact.")]
    [DataMember(Name = "append", Order = 2), JsonPropertyName("append"), JsonPropertyOrder(2), YamlMember(Alias = "append", Order = 2)]
    public virtual bool Append { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether this is the last chunk of the artifact update.
    /// </summary>
    [Description("Indicates whether this is the last chunk of the artifact update.")]
    [DataMember(Name = "lastChunk", Order = 3), JsonPropertyName("lastChunk"), JsonPropertyOrder(3), YamlMember(Alias = "lastChunk", Order = 3)]
    public virtual bool LastChunk { get; set; }

}
