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
/// Represents the default implementation of the <see cref="IFiniteStateMachine{TState, TModel}"/> interface.
/// </summary>
/// <typeparam name="TState">The type of the states in the finite state machine. It must be a value type and an enumeration.</typeparam>
/// <typeparam name="TModel">The type of the model associated with the finite state machine.</typeparam>
/// <param name="definition">The definition of the finite state machine.</param>
/// <param name="state">The initial state of the finite state machine.</param>
/// <param name="model">The initial model associated with the finite state machine.</param>
public sealed class FiniteStateMachine<TState, TModel>(FiniteStateMachineDefinition<TState, TModel> definition, TState state, TModel model)
    : IFiniteStateMachine<TState, TModel>
    where TState : struct, Enum
{

    /// <inheritdoc/>
    public TState State { get; private set; } = state;

    /// <inheritdoc/>
    public TModel Model { get; set; } = model;

    Enum IFiniteStateMachine.State => State;

    object IFiniteStateMachine.Model
    {
        get => Model!;
        set => Model = (TModel)value;
    }

    /// <inheritdoc/>
    public async IAsyncEnumerable<TaskEvent> FireAsync(string trigger, IFiniteStateMachineExecutionContext<TState, TModel> context, [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        var transition = ResolveTransition(trigger, context);
        if (transition is null) yield break;
        if (transition.To is null || (transition.From is not null && EqualityComparer<TState>.Default.Equals(transition.From.Value, transition.To.Value)))
        {
            if (transition.Do is not null) foreach (var effect in transition.Do) await foreach (var e in effect(context, cancellationToken).WithCancellation(cancellationToken)) yield return e;
            yield break;
        }
        var from = State;
        var to = transition.To.Value;
        if (definition.States.TryGetValue(from, out var state)) foreach (var exit in state.Exit) await foreach (var e in exit(context, cancellationToken).WithCancellation(cancellationToken)) yield return e;
        if (transition.Do is not null) foreach (var effect in transition.Do) await foreach (var e in effect(context, cancellationToken).WithCancellation(cancellationToken)) yield return e;
        State = to;
        if (definition.States.TryGetValue(to, out state)) foreach (var entry in state.Enter) await foreach (var e in entry(context, cancellationToken).WithCancellation(cancellationToken)) yield return e;
    }

    ITransition<TState, TModel>? ResolveTransition(string trigger, IFiniteStateMachineExecutionContext<TState, TModel> context)
    {
        ITransition<TState, TModel>? transition = null;
        if (definition.Transitions.TryGetValue((State, trigger), out var stateTransitions))
        {
            foreach (var candidate in stateTransitions)
            {
                if (candidate.When is null || candidate.When(context))
                {
                    transition = candidate;
                    break;
                }
            }
        }
        if (transition is null && definition.Transitions.TryGetValue((null, trigger), out var anyTransitions))
        {
            foreach (var candidate in anyTransitions)
            {
                if (candidate.When is null || candidate.When(context))
                {
                    transition = candidate;
                    break;
                }
            }
        }
        if (transition is null && definition.Transitions.TryGetValue((State, null), out anyTransitions))
        {
            foreach (var candidate in anyTransitions)
            {
                if (candidate.When is null || candidate.When(context))
                {
                    transition = candidate;
                    break;
                }
            }
        }
        if (transition is null && definition.Transitions.TryGetValue((null, null), out anyTransitions))
        {
            foreach (var candidate in anyTransitions)
            {
                if (candidate.When is null || candidate.When(context))
                {
                    transition = candidate;
                    break;
                }
            }
        }
        return transition;
    }

    /// <inheritdoc/>
    public StateMachineSnapshot Snapshot()
    {
        return new()
        {
            State = JsonSerializer.SerializeToNode(State)!,
            Model = JsonSerializer.SerializeToNode(Model)!
        };
    }

    /// <summary>
    /// Loads a finite state machine from the specified snapshot.
    /// </summary>
    /// <param name="definition">The definition of the finite state machine.</param>
    /// <param name="snapshot">The snapshot containing the state and model data to restore the finite state machine from.</param>
    /// <returns>A new <see cref="FiniteStateMachine{TState, TModel}"/>.</returns>
    public static FiniteStateMachine<TState, TModel> Load(FiniteStateMachineDefinition<TState, TModel> definition, StateMachineSnapshot snapshot)
    {
        ArgumentNullException.ThrowIfNull(definition);
        ArgumentNullException.ThrowIfNull(snapshot);
        var state = JsonSerializer.Deserialize<TState>(snapshot.State)!;
        var context = JsonSerializer.Deserialize<TModel>(snapshot.Model)!;
        return new FiniteStateMachine<TState, TModel>(definition, state, context);
    }

}
