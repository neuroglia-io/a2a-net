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
/// Defines the fundamentals of a service used to build <see cref="IState{TState, TModel}"/> instances.
/// </summary>
/// <typeparam name="TState">The state type.</typeparam>
/// <typeparam name="TModel">The model type.</typeparam>
public interface IStateBuilder<TState, TModel>
    where TState : struct, Enum
{

    /// <summary>
    /// Configures the name of the state being built.
    /// </summary>
    /// <param name="state">The state value to use as the name of the state being built.</param>
    /// <returns>The current <see cref="IStateBuilder{TState, TModel}"/> for method chaining.</returns>
    IStateBuilder<TState, TModel> WithName(TState state);

    /// <summary>
    /// Configures an action to be executed when entering the state being built.
    /// </summary>
    /// <param name="action">The <see cref="TaskEventStreamDelegate{TState, TModel}"/> to execute when entering the state being built.</param>
    /// <returns>The current <see cref="IStateBuilder{TState, TModel}"/> for method chaining.</returns>
    IStateBuilder<TState, TModel> OnEnter(TaskEventStreamDelegate<TState, TModel> action);

    /// <summary>
    /// Configures an effect to be executed when entering the state being built.
    /// </summary>
    /// <typeparam name="TEffect">The type of the effect to execute when entering the state.</typeparam>
    /// <returns>The current <see cref="IStateBuilder{TState, TModel}"/> for method chaining.</returns>
    IStateBuilder<TState, TModel> OnEnter<TEffect>()
        where TEffect : IEffect<TState, TModel>;

    /// <summary>
    /// Configures an action to be executed when exiting the state being built.
    /// </summary>
    /// <param name="action">The <see cref="TaskEventStreamDelegate{TState, TModel}"/> to execute when exiting the state being built.</param>
    /// <returns>The current <see cref="IStateBuilder{TState, TModel}"/> for method chaining.</returns>
    IStateBuilder<TState, TModel> OnExit(TaskEventStreamDelegate<TState, TModel> action);
    
    /// <summary>
    /// Configures an effect to be executed when exiting the state being built.
    /// </summary>
    /// <typeparam name="TEffect">The type of the effect to execute when exiting the state.</typeparam>
    /// <returns>The current <see cref="IStateBuilder{TState, TModel}"/> for method chaining.</returns>
    IStateBuilder<TState, TModel> OnExit<TEffect>()
        where TEffect : IEffect<TState, TModel>;

    /// <summary>
    /// Builds the configured <see cref="IState{TState, TModel}"/>.
    /// </summary>
    IState<TState, TModel> Build();
}