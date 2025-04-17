// Copyright � 2025-Present the a2a-net Authors
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

namespace A2A.Server;

/// <summary>
/// Defines extensions for <see cref="TaskRecord"/>s
/// </summary>
public static class TaskRecordExtensions
{

    /// <summary>
    /// Converts the <see cref="TaskRecord"/> into a new <see cref="Models.Task"/>
    /// </summary>
    /// <param name="taskRecord">The <see cref="TaskRecord"/> to convert</param>
    /// <param name="stateTransitionHistory">A boolean indicating whether or not the agent exposes status change history for tasks</param>
    /// <param name="historyLength">The maximum length number of state transitions, if any, to be retrieved</param>
    /// <returns>A new <see cref="Models.Task"/></returns>
    public static Models.Task AsTask(this TaskRecord taskRecord, bool stateTransitionHistory = false, uint? historyLength = null) => new()
    {
        Id = taskRecord.Id,
        SessionId = taskRecord.SessionId,
        Status = taskRecord.Status,
        Artifacts = taskRecord.Artifacts,
        History = stateTransitionHistory ? historyLength.HasValue && taskRecord.History != null ? [..taskRecord.History.TakeLast((int)historyLength.Value)] : taskRecord.History : null,
        Metadata = taskRecord.Metadata,
    };

}
