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
/// Defines the fundamentals of a transition triggered builder, which is responsible for defining the trigger conditions, guard conditions, actions, and effects for a transition in a finite state machine.
/// </summary>
/// <typeparam name="TState">The state type.</typeparam>
/// <typeparam name="TModel">The model type.</typeparam>
public interface ITransitionTriggeredBuilder<TState, TModel>
    where TState : struct, Enum
{

    /// <summary>
    /// Configures the transition to be triggered only when the specified guard condition is satisfied.
    /// </summary>
    /// <param name="guard">The guard condition that must be satisfied for the transition to be triggered.</param>
    /// <returns>The <see cref="ITransitionTriggeredBuilder{TState, TModel}"/> instance for further configuration.</returns>
    ITransitionTriggeredBuilder<TState, TModel> When(GuardDelegate<TState, TModel> guard);

    /// <summary>
    /// Configures the transition to execute the specified action when triggered, allowing for side effects or operations to be performed as part of the transition.
    /// </summary>
    /// <param name="action">The action to be executed when the transition is triggered, represented as a delegate that produces a stream of task events.</param>
    /// <returns>The <see cref="ITransitionTriggeredBuilder{TState, TModel}"/> instance for further configuration.</returns>
    ITransitionTriggeredBuilder<TState, TModel> Do(TaskEventStreamDelegate<TState, TModel> action);

    /// <summary>
    /// Configures the transition to execute the specified effect when triggered.
    /// </summary>
    /// <typeparam name="TEffect">The type of the effect to be executed when the transition is triggered.</typeparam>
    /// <returns>The <see cref="ITransitionTriggeredBuilder{TState, TModel}"/> instance for further configuration.</returns>
    ITransitionTriggeredBuilder<TState, TModel> Do<TEffect>()
        where TEffect : IEffect<TState, TModel>;

    /// <summary>
    /// Builds the configured <see cref="ITransitionBuilder{TState, TModel}"/>.
    /// </summary>
    /// <returns>An instance of <see cref="ITransition{TState, TModel}"/> representing the transition configuration.</returns>
    ITransition<TState, TModel> Build();

}