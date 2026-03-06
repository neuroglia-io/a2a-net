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
/// Defines the fundamentals of a transition from builder, which is responsible for defining the source state and trigger conditions for a transition in a finite state machine.
/// </summary>
/// <typeparam name="TState">The state type.</typeparam>
/// <typeparam name="TModel">The model type.</typeparam>
public interface ITransitionFromBuilder<TState, TModel>
    where TState : struct, Enum
{

    /// <summary>
    /// Configures the transition to be triggered by the specified trigger.
    /// </summary>
    /// <param name="trigger">The trigger that will initiate the transition.</param>
    /// <returns>The <see cref="ITransitionTriggeredBuilder{TState, TModel}"/> instance for further configuration.</returns>
    ITransitionTriggeredBuilder<TState, TModel> TriggeredBy(string trigger);

    /// <summary>
    /// Configures the transition to be triggered by any trigger, allowing the transition to be initiated by any event or condition that occurs in the state machine.
    /// </summary>
    /// <returns>The <see cref="ITransitionTriggeredBuilder{TState, TModel}"/> instance for further configuration.</returns>
    ITransitionTriggeredBuilder<TState, TModel> TriggeredByAny();

    /// <summary>
    /// Configures the transition to lead to the specified target state when triggered, defining the destination state for the transition in the finite state machine.
    /// </summary>
    /// <param name="to">The target state to which the transition will lead when triggered.</param>
    /// <returns>A new <see cref="ITransitionFromBuilder{TState, TModel}"/> that can be used to further define the transition.</returns>
    ITransitionFromBuilder<TState, TModel> To(TState to);

}
