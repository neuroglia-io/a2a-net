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
/// Represents the default implementation of the <see cref="IFiniteStateMachineDefinitionBuilder{TState, TModel}"/> interface.
/// </summary>
/// <typeparam name="TState">The type of the states in the finite state machine. It must be a value type and an enumeration.</typeparam>
/// <typeparam name="TModel">The type of the model associated with the finite state machine.</typeparam>
public sealed class FiniteStateMachineDefinitionBuilder<TState, TModel>
    : IFiniteStateMachineDefinitionBuilder<TState, TModel>
     where TState : struct, Enum
{

    readonly FiniteStateMachineDefinition<TState, TModel> definition = new();

    /// <inheritdoc/>
    public IFiniteStateMachineDefinitionBuilder<TState, TModel> WithState(IState<TState, TModel> state)
    {
        definition.States.TryAdd(Enum.Parse<TState>(state.Name), state);
        return this;
    }

    /// <inheritdoc/>
    public IFiniteStateMachineDefinitionBuilder<TState, TModel> WithState(Action<IStateBuilder<TState, TModel>> setup)
    {
        var builder = new StateBuilder<TState, TModel>();
        setup(builder);
        var state = builder.Build();
        return WithState(state);
    }

    /// <inheritdoc/>
    public IFiniteStateMachineDefinitionBuilder<TState, TModel> WithTransition(ITransition<TState, TModel> transition)
    {
        if (transition.From is not null) definition.States.TryAdd(transition.From.Value, new State<TState, TModel>());
        if (transition.To is not null) definition.States.TryAdd(transition.To.Value, new State<TState, TModel>());
        var key = (transition.From, transition.Trigger);
        if (!definition.Transitions.TryGetValue(key, out var transitions))
        {
            transitions = [];
            definition.Transitions[key] = transitions;
        }
        transitions.Add(transition);
        return this;
    }

    /// <inheritdoc/>
    public IFiniteStateMachineDefinitionBuilder<TState, TModel> WithTransition(Action<ITransitionBuilder<TState, TModel>> setup)
    {
        var builder = new TransitionBuilder<TState, TModel>();
        setup(builder);
        var transition = builder.Build();
        return WithTransition(transition);
    }

    /// <inheritdoc/>
    public FiniteStateMachineDefinition<TState, TModel> Build()
    {
        return definition;
    }

}
