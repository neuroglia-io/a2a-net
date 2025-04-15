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

namespace Neuroglia.A2A.Server.Infrastructure.Services;

/// <summary>
/// Defines the fundamentals of a service used to interact with an AI agent to execute tasks
/// </summary>
public interface IAgentRuntime
{

    /// <summary>
    /// Executes the specified task and streams the content produced by the agent
    /// </summary>
    /// <param name="task">The task to execute</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/></param>
    /// <returns>A new <see cref="IAsyncEnumerable{T}"/> used to stream the content produced by the agent during the task's execution</returns>
    IAsyncEnumerable<AgentResponseContent> ExecuteAsync(Models.Task task, CancellationToken cancellationToken = default);

    /// <summary>
    /// Cancels the specified task's execution
    /// </summary>
    /// <param name="task">The task to cancel.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/></param>
    /// <returns>A new awaitable <see cref="Task"/></returns>
    System.Threading.Tasks.Task CancelAsync(Models.Task task, CancellationToken cancellationToken = default);

}