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
/// Defines the fundamentals of a state transition in a finite state machine. A transition represents a change from one state to another, triggered by a specific event or condition.
/// </summary>
/// <typeparam name="TState">The type of the states in the finite state machine. It must be a value type and an enumeration.</typeparam>
/// <typeparam name="TModel">The type of the model associated with the finite state machine.</typeparam>
public interface ITransition<TState, TModel>
    where TState : struct, Enum
{

    /// <summary>
    /// Gets the source state of the transition, if any. If null, it indicates a transition that can occur from any state (i.e., a global transition).
    /// </summary>
    TState? From { get; }

    /// <summary>
    /// Gets the destination state of the transition, if any. If null, it indicates an internal transition that does not change the state.
    /// </summary>
    TState? To { get; }

    /// <summary>
    /// Gets the trigger that causes the transition, if any. If null, it indicates a transition that can occur with any trigger (i.e., a triggerless transition).
    /// </summary>
    string? Trigger { get; }

    /// <summary>
    /// Gets the delegate, if any, that defines the conditions under which the guard is applied.
    /// </summary>
    GuardDelegate<TState, TModel>? When { get; }

    /// <summary>
    /// Gets a collection of delegates, if any, that define the actions to be performed during the transition.
    /// </summary>
    IReadOnlyCollection<TaskEventStreamDelegate<TState, TModel>>? Do { get; }

}