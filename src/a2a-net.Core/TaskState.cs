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

namespace A2A;

/// <summary>
/// Enumerates all possible task states
/// </summary>
public static class TaskState
{

    /// <summary>
    /// Indicates that the task has been submitted and is pending executing
    /// </summary>
    public const string Submitted = "submitted";
    /// <summary>
    /// Indicates that the task is being executed
    /// </summary>
    public const string Working = "working";
    /// <summary>
    /// Indicates that the task requires input
    /// </summary>
    public const string InputRequired = "input-required";
    /// <summary>
    /// Indicates that the task ran to completion
    /// </summary>
    public const string Completed = "completed";
    /// <summary>
    /// Indicates that the task has been cancelled
    /// </summary>
    public const string Cancelled = "cancelled";
    /// <summary>
    /// Indicates that the task has failed
    /// </summary>
    public const string Failed = "failed";
    /// <summary>
    /// Indicates that the task ask terminated due to rejection by remote agent.
    /// </summary>
    public const string Rejected = "rejected";
    /// <summary>
    /// Indicates that the task requires authentication before it can be executed.
    /// </summary>
    public const string AuthRequired = "auth-required";
    /// <summary>
    /// Indicates that the task is an unknown status
    /// </summary>
    public const string Unknown = "unknown";

    /// <summary>
    /// Gets a new <see cref="IEnumerable{T}"/> containing all supported values
    /// </summary>
    /// <returns>An <see cref="IEnumerable{T}"/> containing all supported values</returns>
    public static IEnumerable<string> AsEnumerable()
    {
        yield return Submitted;
        yield return Working;
        yield return InputRequired;
        yield return Completed;
        yield return Cancelled;
        yield return Failed;
        yield return Rejected;
        yield return AuthRequired;
        yield return Unknown;
    }

}
