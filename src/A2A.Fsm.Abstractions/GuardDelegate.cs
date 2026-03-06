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
/// Represents a delegate that defines a guard condition for a state transition in a finite state machine.
/// </summary>
/// <typeparam name="TState">The type of the states in the finite state machine. It must be a value type and an enumeration.</typeparam>
/// <typeparam name="TModel">The type of the model associated with the finite state machine.</typeparam>
/// <param name="context">The current execution context of the finite state machine.</param>
/// <returns>A boolean value indicating whether the guard condition is satisfied, allowing the state transition to occur.</returns>
public delegate bool GuardDelegate<TState, TModel>(IFiniteStateMachineExecutionContext<TState, TModel> context) where TState : struct, Enum;
