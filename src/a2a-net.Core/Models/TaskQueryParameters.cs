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
/// Represents the parameters of a task query.
/// </summary>
[Description("Represents the parameters of a task query.")]
[DataContract]
public record TaskQueryParameters
    : RpcRequestParameters
{

    /// <summary>
    /// Gets or sets the id of the task to query.
    /// </summary>
    [Description("The id of the task to query.")]
    [Required, MinLength(1)]
    [DataMember(Name = "id", Order = 1), JsonPropertyName("id"), JsonPropertyOrder(1), YamlMember(Alias = "id", Order = 1)]
    public virtual string Id { get; set; } = null!;

    /// <summary>
    /// Gets or sets the length, if any, of the task message history to get.
    /// </summary>
    [Description("The length, if any, of the task message history to get.")]
    [DataMember(Name = "historyLength", Order = 2), JsonPropertyName("historyLength"), JsonPropertyOrder(2), YamlMember(Alias = "historyLength", Order = 2)]
    public virtual uint? HistoryLength { get; set; } = null!;

}
