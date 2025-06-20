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

namespace A2A.Server.Infrastructure.Services;

/// <summary>
/// Represents the default implementation of the <see cref="IAgentSkillBuilder"/> interface
/// </summary>
public class AgentSkillBuilder
    : IAgentSkillBuilder
{

    /// <summary>
    /// Gets the skill to build and configure
    /// </summary>
    protected AgentSkill Skill { get; } = new();

    /// <inheritdoc/>
    public virtual IAgentSkillBuilder WithId(string id)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(id);
        Skill.Id = id;
        return this;
    }

    /// <inheritdoc/>
    public virtual IAgentSkillBuilder WithName(string name)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        Skill.Name = name;
        return this;
    }

    /// <inheritdoc/>
    public virtual IAgentSkillBuilder WithDescription(string description)
    {
        Skill.Description = description;
        return this;
    }

    /// <inheritdoc/>
    public virtual IAgentSkillBuilder WithTag(string tag)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(tag);
        Skill.Tags ??= [];
        if (!Skill.Tags.Contains(tag)) Skill.Tags.Add(tag);
        return this;
    }

    /// <inheritdoc/>
    public virtual IAgentSkillBuilder WithExample(string example)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(example);
        Skill.Examples ??= [];
        Skill.Examples.Add(example);
        return this;
    }

    /// <inheritdoc/>
    public virtual IAgentSkillBuilder WithInputMode(string contentType)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(contentType);
        Skill.InputModes ??= [];
        if (!Skill.InputModes.Contains(contentType)) Skill.InputModes.Add(contentType);
        return this;
    }

    /// <inheritdoc/>
    public virtual IAgentSkillBuilder WithOutputMode(string contentType)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(contentType);
        Skill.OutputModes ??= [];
        if (!Skill.OutputModes.Contains(contentType)) Skill.OutputModes.Add(contentType);
        return this;
    }

    /// <inheritdoc/>
    public virtual AgentSkill Build() => Skill;

}