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
using A2A.Services;

namespace A2A.Server.Services;

/// <summary>
/// Represents the default implementation of the <see cref="IA2AHostedAgentDefinitionBuilder"/> interface.
/// </summary>
public sealed class A2AHostedAgentDefinitionBuilder
    : IA2AHostedAgentDefinitionBuilder
{

    AgentCard? card;
    AgentCard? extendedCard;
    Type? runtimeType;

    /// <inheritdoc/>
    public IA2AHostedAgentDefinitionBuilder WithCard(AgentCard card)
    {
        ArgumentNullException.ThrowIfNull(card);
        this.card = card;
        return this;
    }

    /// <inheritdoc/>
    public IA2AHostedAgentDefinitionBuilder WithCard(Action<IAgentCardBuilder> setup)
    {
        ArgumentNullException.ThrowIfNull(setup);
        var cardBuilder = new AgentCardBuilder();
        setup(cardBuilder);
        card = cardBuilder.Build();
        return this;
    }

    /// <inheritdoc/>
    public IA2AHostedAgentDefinitionBuilder WithExtendedCard(AgentCard extendedCard)
    {
        ArgumentNullException.ThrowIfNull(card);
        this.extendedCard = extendedCard;
        return this;
    }

    /// <inheritdoc/>
    public IA2AHostedAgentDefinitionBuilder WithExtendedCard(Action<IAgentCardBuilder> setup)
    {
        ArgumentNullException.ThrowIfNull(setup);
        var cardBuilder = new AgentCardBuilder();
        setup(cardBuilder);
        extendedCard = cardBuilder.Build();
        return this;
    }

    /// <inheritdoc/>
    public IA2AHostedAgentDefinitionBuilder UseRuntime<TRuntime>()
        where TRuntime : class, IA2AAgentRuntime
    {
        runtimeType = typeof(TRuntime);
        return this;
    }

    /// <inheritdoc/>
    public A2AHostedAgentDefinition Build()
    {
        if (card == null) throw new NullReferenceException("The agent card must be configured");
        if (runtimeType == null) throw new NullReferenceException("The agent runtime must be configured");
        return new()
        {
            RuntimeType = runtimeType,
            Card = card,
            ExtendedCard = extendedCard
        };
    }

}