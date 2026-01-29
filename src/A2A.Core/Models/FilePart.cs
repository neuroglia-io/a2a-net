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
/// Represents a part used to transport a file attachment.
/// </summary>
[Description("Represents a part used to transport a file attachment.")]
[DataContract]
public sealed record FilePart
    : Part
{

    /// <summary>
    /// Gets the file's name, if any.
    /// </summary>
    [Description("The file's name, if any.")]
    [DataMember(Order = 1, Name = "filename"), JsonPropertyOrder(1), JsonPropertyName("filename")]
    public string? FileName { get; init; }

    /// <summary>
    /// Gets the file's URL, if any. Required if 'raw' is not set.
    /// </summary>
    [Description("The file's URL, if any. Required if 'raw' is not set.")]
    [DataMember(Order = 3, Name = "url"), JsonPropertyOrder(3), JsonPropertyName("url")]
    public Uri? Url { get; init; }

    /// <summary>
    /// Gets the file's bytes, if any. Required if 'url' is not set.
    /// </summary>
    [Description("The file's bytes, if any. Required if 'url' is not set.")]
    [DataMember(Order = 4, Name = "raw"), JsonPropertyOrder(4), JsonPropertyName("raw")]
    public ReadOnlyMemory<byte>? Raw { get; init; }

}
