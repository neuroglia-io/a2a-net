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

namespace A2A.Fsm;

/// <summary>
/// Represents the definition of a finite state machine.
/// </summary>
/// <typeparam name="TState">The type of the states in the finite state machine. It must be a value type and an enumeration.</typeparam>
/// <typeparam name="TModel">The type of the model associated with the finite state machine.</typeparam>
public sealed class FiniteStateMachineDefinition<TState, TModel>
     where TState : struct, Enum
{

    /// <summary>
    /// Gets a state transition map that defines the transitions between states in the finite state machine. The keys of the dictionary are tuples consisting of the source state and the trigger that causes the transition, while the values are collections of transitions that can occur from the source state when the specified trigger is activated.
    /// </summary>
    public IDictionary<(TState? From, string? Trigger), ICollection<ITransition<TState, TModel>>> Transitions { get; init; } = new Dictionary<(TState? From, string? Trigger), ICollection<ITransition<TState, TModel>>>();

    /// <summary>
    /// Gets a state map that defines the states in the finite state machine. The keys of the dictionary are the states, while the values are the corresponding state definitions that contain information about the state's behavior and associated actions.
    /// </summary>
    public IDictionary<TState, IState<TState, TModel>> States { get; init; } = new Dictionary<TState, IState<TState, TModel>>();

}
