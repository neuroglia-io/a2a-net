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
using A2A.Server.Services;

namespace A2A.Server;

/// <summary>
/// Represents the definition of the agent hosted by an A2A server.
/// </summary>
public sealed record A2AHostedAgentDefinition
{

    /// <summary>
    /// Gets the type of <see cref="IA2AAgentRuntime"/> used by the hosted agent.
    /// </summary>
    public required Type RuntimeType { get; init; }

    /// <summary>
    /// Gets the agent's card.
    /// </summary>
    public required AgentCard Card { get; init; }

    /// <summary>
    /// Gets the agent's extended card, if any.
    /// </summary>
    public AgentCard? ExtendedCard { get; init; }

}