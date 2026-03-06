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
/// Defines the fundamentals of a transition builder, which is responsible for defining the transitions between states in a finite state machine.
/// </summary>
/// <typeparam name="TState">The state type.</typeparam>
/// <typeparam name="TModel">The model type.</typeparam>
public interface ITransitionBuilder<TState, TModel>
    where TState : struct, Enum
{

    /// <summary>
    /// Configures the transition to originate from the specified state within the state machine.
    /// </summary>
    /// <param name="from">The state from which the transition is initiated.</param>
    /// <returns>The <see cref="ITransitionFromBuilder{TState, TModel}"/> instance for further configuration.</returns>
    ITransitionFromBuilder<TState, TModel> From(TState from);

    /// <summary>
    /// Configures a transition that can be triggered from any state in the state machine.
    /// </summary>
    /// <returns>A new <see cref="ITransitionFromBuilder{TState, TModel}"/> that can be used to further define the transition.</returns>
    ITransitionFromBuilder<TState, TModel> FromAny();

}
