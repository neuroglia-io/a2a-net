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
/// Defines the fundamentals of a service used to build <see cref="FiniteStateMachineDefinitionBuilder{TState, TModel}"/> instances.
/// </summary>
/// <typeparam name="TState">The type of the states in the finite state machine. It must be a value type and an enumeration.</typeparam>
/// <typeparam name="TModel">The type of the model associated with the finite state machine.</typeparam>
public interface IFiniteStateMachineDefinitionBuilder<TState, TModel>
     where TState : struct, Enum
{

    /// <summary>
    /// Configures the finite state machine definition by adding a state to it.
    /// </summary>
    /// <param name="state">The state to add to the finite state machine definition.</param>
    /// <returns>The current <see cref="IFiniteStateMachineDefinitionBuilder{TState, TModel}"/> for method chaining.</returns>
    IFiniteStateMachineDefinitionBuilder<TState, TModel> WithState(IState<TState, TModel> state);

    /// <summary>
    /// Configures the finite state machine definition by adding a state to it using a setup action.
    /// </summary>
    /// <param name="setup">The action to configure the state builder.</param>
    /// <returns>The current <see cref="IFiniteStateMachineDefinitionBuilder{TState, TModel}"/> for method chaining.</returns>
    IFiniteStateMachineDefinitionBuilder<TState, TModel> WithState(Action<IStateBuilder<TState, TModel>> setup);

    /// <summary>
    /// Configures the finite state machine definition by adding a transition to it.
    /// </summary>
    /// <param name="transition">The transition to add to the finite state machine definition.</param>
    /// <returns>The current <see cref="IFiniteStateMachineDefinitionBuilder{TState, TModel}"/>for method chaining.</returns>
    IFiniteStateMachineDefinitionBuilder<TState, TModel> WithTransition(ITransition<TState, TModel> transition);

    /// <summary>
    /// Configures the finite state machine definition by adding a transition to it using a setup action.
    /// </summary>
    /// <param name="setup">The action to configure the transition builder.</param>
    /// <returns>The current <see cref="IFiniteStateMachineDefinitionBuilder{TState, TModel}"/> for method chaining.</returns>
    IFiniteStateMachineDefinitionBuilder<TState, TModel> WithTransition(Action<ITransitionBuilder<TState, TModel>> setup);

    /// <summary>
    /// Builds the configured <see cref="FiniteStateMachineDefinition{TState, TModel}"/>.
    /// </summary>
    /// <returns>The configured <see cref="FiniteStateMachineDefinition{TState, TModel}"/>.</returns>
    FiniteStateMachineDefinition<TState, TModel> Build();

}
