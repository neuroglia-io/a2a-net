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
/// Represents the default implementation of the <see cref="IStateBuilder{TState, TModel}"/> interface, which is responsible for building state definitions for a finite state machine.
/// </summary>
/// <typeparam name="TState">The type of the state enumeration.</typeparam>
/// <typeparam name="TModel">The type of the model associated with the state machine.</typeparam>
public sealed class StateBuilder<TState, TModel>
    : IStateBuilder<TState, TModel>
    where TState : struct, Enum
{

    string? name = null;
    readonly List<TaskEventStreamDelegate<TState, TModel>> enterActions = [];
    readonly List<TaskEventStreamDelegate<TState, TModel>> exitActions = [];

    /// <inheritdoc/>
    public IStateBuilder<TState, TModel> WithName(TState state)
    {
        name = Enum.GetName(state)!.ToString();
        return this;
    }

    /// <inheritdoc/>
    public IStateBuilder<TState, TModel> OnEnter(TaskEventStreamDelegate<TState, TModel> action)
    {
        ArgumentNullException.ThrowIfNull(action);
        enterActions.Add(action);
        return this;
    }

    /// <inheritdoc/>
    public IStateBuilder<TState, TModel> OnEnter<TEffect>()
        where TEffect : IEffect<TState, TModel>
    {
        enterActions.Add((context, cancellationToken) =>
        {
            var effect = ActivatorUtilities.CreateInstance<TEffect>(context.Services);
            return effect.ExecuteAsync(context, cancellationToken);
        });
        return this;
    }

    /// <inheritdoc/>
    public IStateBuilder<TState, TModel> OnExit(TaskEventStreamDelegate<TState, TModel> action)
    {
        ArgumentNullException.ThrowIfNull(action);
        exitActions.Add(action);
        return this;
    }

    /// <inheritdoc/>
    public IStateBuilder<TState, TModel> OnExit<TEffect>()
        where TEffect : IEffect<TState, TModel>
    {
        exitActions.Add((context, cancellationToken) =>
        {
            var effect = ActivatorUtilities.CreateInstance<TEffect>(context.Services);
            return effect.ExecuteAsync(context, cancellationToken);
        });
        return this;
    }

    /// <inheritdoc/>
    public IState<TState, TModel> Build()
    {
        if (string.IsNullOrWhiteSpace(name)) throw new InvalidOperationException("State name must be specified.");
        return new State<TState, TModel>()
        {
            Name = name,
            Enter = enterActions,
            Exit = exitActions
        };
    }

}