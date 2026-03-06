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
/// Defines the fundamentals of a finite state machine, which is a computational model that consists of a finite number of states, transitions between those states, and actions that can be performed based on the current state and input.
/// </summary>
public interface IFiniteStateMachine
{

    /// <summary>
    /// Gets the current state of the finite state machine. The state is represented as an object, which can be of any type, but is typically an enumeration that identifies the current state of the machine. The state is used to determine the behavior of the machine and to trigger transitions between states based on input and events.
    /// </summary>
    Enum State { get; }

    /// <summary>
    /// Gets the current model of the finite state machine. The model is represented as an object, which can be of any type, and is used to store additional data and context that may be relevant to the behavior of the machine. The model can be updated during state transitions and can be accessed by effects and other components of the machine to make decisions and perform actions based on the current state and context.
    /// </summary>
    object Model { get; set; }

    /// <summary>
    /// Creates a snapshot that captures the current state and data of the state machine.
    /// </summary>
    /// <returns>A new <see cref="StateMachineSnapshot"/>.</returns>
    StateMachineSnapshot Snapshot();

}

/// <summary>
/// Defines the fundamentals of a finite state machine, which is a computational model that consists of a finite number of states, transitions between those states, and actions that can be performed based on the current state and input.
/// </summary>
/// <typeparam name="TState">Specifies the type used to represent the state. Must be an enumeration.</typeparam>
/// <typeparam name="TModel">Specifies the type of the model associated with the state machine.</typeparam>
public interface IFiniteStateMachine<TState, TModel>
    : IFiniteStateMachine
    where TState : struct, Enum
{

    /// <summary>
    /// Gets the current state of the finite state machine. The state is represented as an object, which can be of any type, but is typically an enumeration that identifies the current state of the machine. The state is used to determine the behavior of the machine and to trigger transitions between states based on input and events.
    /// </summary>
    new TState State { get; }

    /// <summary>
    /// Gets the current model of the finite state machine. The model is represented as an object, which can be of any type, and is used to store additional data and context that may be relevant to the behavior of the machine. The model can be updated during state transitions and can be accessed by effects and other components of the machine to make decisions and perform actions based on the current state and context.
    /// </summary>
    new TModel Model { get; set; }

    /// <summary>
    /// Fires a trigger, which is an event or input that can cause the state machine to transition from one state to another.
    /// </summary>
    /// <param name="trigger">The trigger to fire.</param>
    /// <param name="context">The current execution context.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/>.</param>
    /// <returns>A new <see cref="IAsyncEnumerable{T}"/> of <see cref="TaskEvent"/>s that are produced as a result of firing the trigger.</returns>
    IAsyncEnumerable<TaskEvent> FireAsync(string trigger, IFiniteStateMachineExecutionContext<TState, TModel> context, CancellationToken cancellationToken = default);

}
