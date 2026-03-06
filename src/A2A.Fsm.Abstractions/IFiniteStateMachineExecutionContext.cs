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
/// Defines the fundamentals of the execution context for a finite state machine, which provides access to the current task, the finite state machine instance, and any services that may be required during the execution of states, transitions and effects.
/// </summary>
/// <typeparam name="TState">The state type.</typeparam>
/// <typeparam name="TModel">The model type.</typeparam>
public interface IFiniteStateMachineExecutionContext<TState, TModel>
    where TState : struct, Enum
{

    /// <summary>
    /// Gets the current <see cref="Models.Task"/>.
    /// </summary>  
    Models.Task Task { get; }

    /// <summary>
    /// Gets the <see cref="IFiniteStateMachine{TState, TModel}"/> instance that is currently executing the state, transition or effect.
    /// </summary>
    IFiniteStateMachine<TState, TModel> StateMachine { get; }

    /// <summary>
    /// Gets the current <see cref="IServiceProvider"/>.
    /// </summary>
    IServiceProvider Services { get; }

}