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
/// Defines the fundamentals of a service used to build an agent's skill
/// </summary>
public interface IAgentSkillBuilder
{

    /// <summary>
    /// Configures the skill's unique identifier
    /// </summary>
    /// <param name="id">The skill's unique identifier</param>
    /// <returns>The configured <see cref="IAgentSkillBuilder"/></returns>
    IAgentSkillBuilder WithId(string id);

    /// <summary>
    /// Configures the skill's name
    /// </summary>
    /// <param name="name">The skill's name</param>
    /// <returns>The configured <see cref="IAgentSkillBuilder"/></returns>
    IAgentSkillBuilder WithName(string name);

    /// <summary>
    /// Configures the skill's description
    /// </summary>
    /// <param name="description">The skill's description</param>
    /// <returns>The configured <see cref="IAgentSkillBuilder"/></returns>
    IAgentSkillBuilder WithDescription(string description);

    /// <summary>
    /// Adds the specified tag to the skill to configure
    /// </summary>
    /// <param name="tag">The tag to add</param>
    /// <returns>The configured <see cref="IAgentSkillBuilder"/></returns>
    IAgentSkillBuilder WithTag(string tag);

    /// <summary>
    /// Adds a new example scenarios that the skill can perform
    /// </summary>
    /// <param name="example">The example to add</param>
    /// <returns>The configured <see cref="IAgentSkillBuilder"/></returns>
    IAgentSkillBuilder WithExample(string example);

    /// <summary>
    /// Configures the skill to support the specified MIME type as input
    /// </summary>
    /// <param name="contentType">The MIME type to support</param>
    /// <returns>The configured <see cref="IAgentSkillBuilder"/></returns>
    IAgentSkillBuilder WithInputMode(string contentType);

    /// <summary>
    /// Configures the skill to support the specified MIME type as output
    /// </summary>
    /// <param name="contentType">The MIME type to support</param>
    /// <returns>The configured <see cref="IAgentSkillBuilder"/></returns>
    IAgentSkillBuilder WithOutputMode(string contentType);

    /// <summary>
    /// Builds the configured <see cref="AgentSkill"/>
    /// </summary>
    /// <returns>A new <see cref="AgentSkill"/></returns>
    AgentSkill Build();

}