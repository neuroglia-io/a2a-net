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
/// Represents the default implementation of the <see cref="ITransitionBuilder{TState, TModel}"/> interface.
/// </summary>
/// <typeparam name="TState">The type of the states in the finite state machine. It must be a value type and an enumeration.</typeparam>
/// <typeparam name="TModel">The type of the model associated with the finite state machine.</typeparam>
public sealed class TransitionBuilder<TState, TModel>
    : ITransitionBuilder<TState, TModel>,
    ITransitionFromBuilder<TState, TModel>,
    ITransitionTriggeredBuilder<TState, TModel>
    where TState : struct, Enum
{

    TState? from = null;
    TState? to = null;
    string? trigger;
    GuardDelegate<TState, TModel>? guard = null;
    List<TaskEventStreamDelegate<TState, TModel>>? actions;

    /// <inheritdoc/>
    public ITransitionFromBuilder<TState, TModel> From(TState from)
    {
        this.from = from;
        return this;
    }

    /// <inheritdoc/>
    public ITransitionFromBuilder<TState, TModel> FromAny()
    {
        return this;
    }

    /// <inheritdoc/>
    public ITransitionFromBuilder<TState, TModel> To(TState to)
    {
        this.to = to;
        return this;
    }

    /// <inheritdoc/>
    public ITransitionTriggeredBuilder<TState, TModel> TriggeredBy(string trigger)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(trigger);
        this.trigger = trigger;
        return this;
    }

    /// <inheritdoc/>
    public ITransitionTriggeredBuilder<TState, TModel> TriggeredByAny()
    {
        return this;
    }

    /// <inheritdoc/>
    public ITransitionTriggeredBuilder<TState, TModel> When(GuardDelegate<TState, TModel> guard)
    {
        ArgumentNullException.ThrowIfNull(guard);
        this.guard = guard;
        return this;
    }

    /// <inheritdoc/>
    public ITransitionTriggeredBuilder<TState, TModel> Do(TaskEventStreamDelegate<TState, TModel> action)
    {
        ArgumentNullException.ThrowIfNull(action);
        actions ??= [];
        actions.Add(action);
        return this;
    }

    /// <inheritdoc/>
    public ITransitionTriggeredBuilder<TState, TModel> Do<TEffect>()
        where TEffect : IEffect<TState, TModel>
    {
        return this.Do((context, cancellationToken) =>
        {
            var effect = ActivatorUtilities.CreateInstance<TEffect>(context.Services);
            return effect.ExecuteAsync(context, cancellationToken);
        });
    }

    /// <inheritdoc/>
    public ITransition<TState, TModel> Build()
    {
        return new Transition<TState, TModel>()
        {
            From = from,
            To = to,
            Trigger = trigger,
            When = guard,
            Do = actions
        };
    }

}