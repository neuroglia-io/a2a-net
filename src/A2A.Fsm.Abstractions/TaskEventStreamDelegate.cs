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
/// Represents a delegate that defines the signature for a method that produces an asynchronous stream of task events based on the execution context of a finite state machine.
/// </summary>
/// <typeparam name="TState">The type of the states in the finite state machine. It must be a value type and an enumeration.</typeparam>
/// <typeparam name="TModel">The type of the model associated with the finite state machine.</typeparam>
/// <param name="context">The current execution context of the finite state machine.</param>
/// <param name="cancellationToken">A <see cref="CancellationToken"/>.</param>
/// <returns>A new <see cref="IAsyncEnumerable{T}"/> of <see cref="TaskEvent"/> instances that represent the events produced by the delegate during its execution.</returns>
public delegate IAsyncEnumerable<TaskEvent> TaskEventStreamDelegate<TState, TModel>(IFiniteStateMachineExecutionContext<TState, TModel> context, CancellationToken cancellationToken) where TState : struct, Enum;
