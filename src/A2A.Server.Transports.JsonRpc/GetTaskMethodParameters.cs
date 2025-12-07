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

namespace A2A.Server.Transports;

/// <summary>
/// Represents the parameters for the 'GetTask' JSON-RPC method.
/// </summary>
[Description("Represents the parameters for the 'GetTask' JSON-RPC method.")]
[DataContract]
public sealed record GetTaskMethodParameters
{

    /// <summary>
    /// Gets the unique identifier of the task to get.
    /// </summary>
    [Description("The unique identifier of the task to get.")]
    [Required, MinLength(1)]
    [DataMember(Order = 1, Name = "taskId"), JsonPropertyOrder(1), JsonPropertyName("taskId")]
    public required string TaskId { get; init; }

    /// <summary>
    /// Gets the maximum length, if any, of the history of the task to retrieve.
    /// </summary>
    [Description("The maximum length, if any, of the history of the task to retrieve.")]
    [DataMember(Order = 2, Name = "historyLength"), JsonPropertyOrder(2), JsonPropertyName("historyLength")]
    public uint? HistoryLength { get; init; }

}
