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

namespace A2A.Services;

/// <summary>
/// Represents the default implementation of the <see cref="IAgentSkillBuilder"/> interface.
/// </summary>
public sealed class AgentSkillBuilder
    : IAgentSkillBuilder
{

    readonly AgentSkill skill = new();

    /// <inheritdoc/>
    public IAgentSkillBuilder WithId(string id)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(id);
        skill.Id = id;
        return this;
    }

    /// <inheritdoc/>
    public IAgentSkillBuilder WithName(string name)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        skill.Name = name;
        return this;
    }

    /// <inheritdoc/>
    public IAgentSkillBuilder WithDescription(string description)
    {
        skill.Description = description;
        return this;
    }

    /// <inheritdoc/>
    public IAgentSkillBuilder WithTag(string tag)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(tag);
        skill.Tags ??= [];
        if (!skill.Tags.Contains(tag)) skill.Tags.Add(tag);
        return this;
    }

    /// <inheritdoc/>
    public IAgentSkillBuilder WithExample(string example)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(example);
        skill.Examples ??= [];
        skill.Examples.Add(example);
        return this;
    }

    /// <inheritdoc/>
    public IAgentSkillBuilder WithInputMode(string contentType)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(contentType);
        skill.InputModes ??= [];
        if (!skill.InputModes.Contains(contentType)) skill.InputModes.Add(contentType);
        return this;
    }

    /// <inheritdoc/>
    public IAgentSkillBuilder WithOutputMode(string contentType)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(contentType);
        skill.OutputModes ??= [];
        if (!skill.OutputModes.Contains(contentType)) skill.OutputModes.Add(contentType);
        return this;
    }

    /// <inheritdoc/>
    public AgentSkill Build() => skill;

}