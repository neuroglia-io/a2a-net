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
/// Defines the fundamentals of an effect, which is a unit of work that can be executed as part of a state transition in a finite state machine. Effects are responsible for performing actions, such as updating the model, triggering events, or executing side effects, based on the current state and model of the finite state machine. They can be asynchronous and can yield multiple events during their execution.
/// </summary>
/// <typeparam name="TState">The state type.</typeparam>
/// <typeparam name="TModel">The model type.</typeparam>
public interface IEffect<TState, TModel>
    where TState : struct, Enum
{

    /// <summary>
    /// Executes the effect asynchronously, yielding a sequence of task events that can be processed by the finite state machine.
    /// </summary>
    /// <param name="context">The current execution context.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/>.</param>
    /// <returns>A new <see cref="IAsyncEnumerable{T}"/> of <see cref="TaskEvent"/> instances that represent the events produced by the effect during its execution.</returns>
    IAsyncEnumerable<TaskEvent> ExecuteAsync(IFiniteStateMachineExecutionContext<TState, TModel> context, CancellationToken cancellationToken = default);

}
