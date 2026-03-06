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
/// Represents the default implementation of the <see cref="IState{TState, TModel}"/> interface.
/// </summary>
/// <typeparam name="TState">The type of the state enumeration.</typeparam>
/// <typeparam name="TModel">The type of the model associated with the state machine.</typeparam>
public sealed class State<TState, TModel>
    : IState<TState, TModel>
    where TState : struct, Enum
{

    /// <inheritdoc/>
    public string Name { get; init; } = string.Empty;

    /// <inheritdoc/>
    public IReadOnlyCollection<TaskEventStreamDelegate<TState, TModel>> Enter { get; init; } = [];

    /// <inheritdoc/>
    public IReadOnlyCollection<TaskEventStreamDelegate<TState, TModel>> Exit { get; init; } = [];

}
