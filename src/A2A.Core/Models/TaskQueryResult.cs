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

namespace A2A.Models;

/// <summary>
/// Represents the result of a task query.
/// </summary>
[Description("Represents the result of a task query.")]
[DataContract]
public sealed record TaskQueryResult
{

    /// <summary>
    /// Gets a collection of tasks matching the specified criteria.
    /// </summary>
    [Description("A collection of tasks matching the specified criteria.")]
    [Required]
    [DataMember(Order = 1, Name = "tasks"), JsonPropertyOrder(1), JsonPropertyName("tasks")]
    public required IReadOnlyCollection<Task> Tasks { get; init; }

    /// <summary>
    /// Gets the token used to retrieve the next page of results, if any.
    /// </summary>
    [Description("The token used to retrieve the next page of results, if any.")]
    [Required]
    [DataMember(Order = 2, Name = "nextPageToken"), JsonPropertyOrder(2), JsonPropertyName("nextPageToken")]
    public required string NextPageToken { get; init; }

    /// <summary>
    /// Gets the requested page size.
    /// </summary>
    [Description("The requested page size.")]
    [Required]
    [DataMember(Order = 3, Name = "pageSize"), JsonPropertyOrder(3), JsonPropertyName("pageSize")]
    public required uint PageSize { get; init; }

    /// <summary>
    /// Gets the total number of tasks matching the specified criteria, before pagination.
    /// </summary>
    [Description("The total number of tasks matching the specified criteria, before pagination.")]
    [Required]
    [DataMember(Order = 4, Name = "totalSize"), JsonPropertyOrder(4), JsonPropertyName("totalSize")]
    public required uint TotalSize { get; init; }

}