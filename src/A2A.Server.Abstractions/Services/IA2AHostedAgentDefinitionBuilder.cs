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

using A2A.Models;

namespace A2A.Server.Services;

/// <summary>
/// Defines the fundamentals of a service used to configure the agent hosted by an A2A server.
/// </summary>
public interface IA2AHostedAgentDefinitionBuilder
{

    /// <summary>
    /// Configures the hosted agent's card.
    /// </summary>
    /// <param name="card">The agent'ss card.</param>
    /// <returns>The configured <see cref="IA2AHostedAgentDefinitionBuilder"/>.</returns>
    IA2AHostedAgentDefinitionBuilder WithCard(AgentCard card);

    /// <summary>
    /// Configures the hosted agent's card.
    /// </summary>
    /// <param name="setup">An <see cref="Action{T}"/> used to configure the agent's card.</param>
    /// <returns>The configured <see cref="IA2AHostedAgentDefinitionBuilder"/>.</returns>
    IA2AHostedAgentDefinitionBuilder WithCard(Action<IAgentCardBuilder> setup);

    /// <summary>
    /// Configures the agent definition to use the specified extended card.
    /// </summary>
    /// <param name="extendedCard">The extended card to associate with the agent definition.</param>
    /// <returns>The configured <see cref="IA2AHostedAgentDefinitionBuilder"/>.</returns>
    IA2AHostedAgentDefinitionBuilder WithExtendedCard(AgentCard extendedCard);

    /// <summary>
    /// Configures the agent definition to use an extended card built using the provided setup action.
    /// </summary>
    /// <param name="setup">An <see cref="Action{T}"/> used to configure the extended card.</param>
    /// <returns>The configured <see cref="IA2AHostedAgentDefinitionBuilder"/>.</returns>
    IA2AHostedAgentDefinitionBuilder WithExtendedCard(Action<IAgentCardBuilder> setup);

    /// <summary>
    /// Configures the hosted agent to use the specified <see cref="IA2AAgentRuntime"/>.
    /// </summary>
    /// <typeparam name="TRuntime">The type of <see cref="IA2AAgentRuntime"/> to use.</typeparam>
    /// <returns>The configured <see cref="IA2AHostedAgentDefinitionBuilder"/>.</returns>
    IA2AHostedAgentDefinitionBuilder UseRuntime<TRuntime>()
        where TRuntime : class, IA2AAgentRuntime;

    /// <summary>
    /// Builds the configured <see cref="A2AHostedAgentDefinition"/>.
    /// </summary>
    /// <returns>The configured <see cref="A2AHostedAgentDefinition"/>.</returns>
    A2AHostedAgentDefinition Build();

}
