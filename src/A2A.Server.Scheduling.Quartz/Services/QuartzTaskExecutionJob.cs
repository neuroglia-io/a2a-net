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
/// Represents a Quartz job used to execute an A2A task.
/// </summary>
/// <param name="server">The current A2A protocol server.</param>
public sealed class QuartzTaskExecutionJob(IA2AProtocolServer server)
    : IJob
{

    /// <summary>
    /// Gets the key of the job data used to store the task identifier.
    /// </summary>
    public const string TaskId = "taskId";

    /// <inheritdoc />
    public async Task Execute(IJobExecutionContext context)
    {
        var taskId = context.MergedJobDataMap.GetString(TaskId) ?? throw new NullReferenceException($"The required '{TaskId}' job data is missing.");
        await server.ExecuteTaskAsync(taskId, context.CancellationToken).ConfigureAwait(false);
    }

}