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
/// Defines the fundamentals of a state in a finite state machine.
/// </summary>
/// <typeparam name="TState">The type of the state, which must be a value type and an enumeration.</typeparam>
/// <typeparam name="TModel">The type of the model associated with the finite state machine.</typeparam>
public interface IState<TState, TModel>
    where TState : struct, Enum
{

    /// <summary>
    /// Gets the state's name.
    /// </summary>
    string Name { get; }

    /// <summary>
    /// Gets the collection of delegates that are invoked when an event enters a specific state.
    /// </summary>
    IReadOnlyCollection<TaskEventStreamDelegate<TState, TModel>> Enter { get; }

    /// <summary>
    /// Gets the collection of delegates that are invoked when an event exits a specific state.
    /// </summary>
    IReadOnlyCollection<TaskEventStreamDelegate<TState, TModel>> Exit { get; }

}
