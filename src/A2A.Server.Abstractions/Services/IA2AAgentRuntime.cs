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

namespace A2A.Server.Services;

/// <summary>
/// Defines the fundamentals of a service used to interact with an A2A agent.
/// </summary>
public interface IA2AAgentRuntime
{

    /// <summary>
    /// Processes the specified message.
    /// </summary>
    /// <param name="message">The message to process.</param>
    /// <param name="context">The context in which the agent is being invoked.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/>.</param>
    /// <returns>A new <see cref="Models.Response"/> resulting from the processed message.</returns>
    Task<Models.Response> ProcessAsync(Models.Message message, IA2AAgentInvocationContext context, CancellationToken cancellationToken = default);

    /// <summary>
    /// Executes the specified task.
    /// </summary>
    /// <param name="task">The task to execute.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/>.</param>
    /// <returns>A stream of <see cref="Models.TaskEvent"/>s resulting from the specified task execution.</returns>
    IAsyncEnumerable<Models.TaskEvent> ExecuteAsync(Models.Task task, CancellationToken cancellationToken = default);

}
