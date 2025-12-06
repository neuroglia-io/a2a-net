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
/// Represents the options used to configure a task query.
/// </summary>
[Description("Represents the options used to configure a task query.")]
[DataContract]
public sealed record TaskQueryOptions
{

    /// <summary>
    /// Gets the unique identifier of the context, if any, to filter tasks by.
    /// </summary>
    [Description("The unique identifier of the context, if any, to filter tasks by.")]
    [DataMember(Order = 1, Name = "contextId"), JsonPropertyOrder(1), JsonPropertyName("contextId")]
    public string? ContextId { get; init; }

    /// <summary>
    /// Gets the status, if any, to filter tasks by.
    /// </summary>
    [Description("The status, if any, to filter tasks by.")]
    [DataMember(Order = 2, Name = "status"), JsonPropertyOrder(2), JsonPropertyName("status")]
    public string? Status { get; init; }

    /// <summary>
    /// Gets the maximum number, if any, of tasks to return.
    /// </summary>
    [Description("The maximum number, if any, of tasks to return.")]
    [DataMember(Order = 3, Name = "pageSize"), JsonPropertyOrder(3), JsonPropertyName("pageSize")]
    public uint? PageSize { get; init; }

    /// <summary>
    /// Gets the token, if any, used to retrieve the next page of results.
    /// </summary>
    [Description("The token, if any, used to retrieve the next page of results.")]
    [DataMember(Order = 4, Name = "pageToken"), JsonPropertyOrder(4), JsonPropertyName("pageToken")]
    public string? PageToken { get; init; }

    /// <summary>
    /// Gets the maximum number of messages, if any, to include in the history.
    /// </summary>
    [Description("The maximum number of messages, if any, to include in the history.")]
    [DataMember(Order = 5, Name = "historyLength"), JsonPropertyOrder(5), JsonPropertyName("historyLength")]
    public uint? HistoryLength { get; init; }

    /// <summary>
    /// Gets the timestamp, if any, to filter tasks updated after.
    /// </summary>
    [Description("The timestamp, if any, to filter tasks updated after.")]
    [DataMember(Order = 6, Name = "lastUpdateAfter"), JsonPropertyOrder(6), JsonPropertyName("lastUpdateAfter")]
    public uint? LastUpdateAfter { get; init; }

    /// <summary>
    /// Gets a boolean indicating whether filtered tasks should include artifacts.
    /// </summary>
    [Description("A boolean indicating whether filtered tasks should include artifacts.")]
    [DataMember(Order = 7, Name = "includeArtifacts"), JsonPropertyOrder(7), JsonPropertyName("includeArtifacts")]
    public bool? IncludeArtifacts { get; init; }

    /// <summary>
    /// Gets request-specific metadata, if any.
    /// </summary>
    [Description("Request-specific metadata, if any.")]
    [DataMember(Order = 8, Name = "metadata"), JsonPropertyOrder(8), JsonPropertyName("metadata")]
    public IReadOnlyDictionary<string, JsonNode>? Metadata { get; init; }

}
