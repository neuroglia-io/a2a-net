// Copyright � 2025-Present Neuroglia SRL
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

using System.Reactive.Subjects;

namespace Neuroglia.A2A.Server.Infrastructure.Services;

/// <summary>
/// Represents the default implementation of the <see cref="ITaskEventStream"/> interface
/// </summary>
public class TaskEventStream
    : ITaskEventStream
{

    /// <summary>
    /// Gets the <see cref="ISubject{RpcEvent}"/> used to stream <see cref="TaskEvent"/>s
    /// </summary>
    protected Subject<TaskEvent> Subject { get; } = new();

    /// <inheritdoc/>
    public virtual IDisposable Subscribe(IObserver<TaskEvent> observer) => Subject.Subscribe(observer);

    /// <inheritdoc/>
    public virtual void OnNext(TaskEvent value) => Subject.OnNext(value);

    /// <inheritdoc/>
    public virtual void OnCompleted() => Subject.OnCompleted();

    /// <inheritdoc/>
    public virtual void OnError(Exception error) => Subject.OnError(error);

}
