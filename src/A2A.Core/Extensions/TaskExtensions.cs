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

#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace A2A;

/// <summary>
/// Defines extension methods for <see cref="Task"/>.
/// </summary>
public static class TaskExtensions
{

    /// <summary>
    /// Projects the task based on the specified query options.
    /// </summary>
    /// <param name="task">The task to project.</param>
    /// <param name="queryOptions">The query options.</param>
    /// <returns>The projected task.</returns>
    public static Models.Task Project(this Models.Task task, TaskQueryOptions queryOptions)
    {
        if (queryOptions.HistoryLength.HasValue && task.History is not null)
        {
            var n = (int)Math.Min(int.MaxValue, queryOptions.HistoryLength.Value);
            if (n >= 0 && task.History.Count > n) task = task with
            {
                History = [.. task.History.TakeLast(n)]
            };
        }
        if (queryOptions.IncludeArtifacts.HasValue && queryOptions.IncludeArtifacts.Value == false) task = task with
        {
            Artifacts = null
        };
        return task;
    }

}
