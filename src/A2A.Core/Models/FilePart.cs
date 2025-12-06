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
    [DataMember(Order = 1, Name = "name"), JsonPropertyOrder(1), JsonPropertyName("name")]
    public string? Name { get; init; }

    /// <summary>
    /// Gets the file's media type, if any.
    /// </summary>
    [Description("The file's media type, if any.")]
    [DataMember(Order = 2, Name = "mediaType"), JsonPropertyOrder(2), JsonPropertyName("mediaType")]
    public string? MediaType { get; init; }

    /// <summary>
    /// Gets the file's URI, if any. Required if 'bytes' is not set.
    /// </summary>
    [Description("The file's URI, if any. Required if 'bytes' is not set.")]
    [DataMember(Order = 3, Name = "fileWithUri"), JsonPropertyOrder(3), JsonPropertyName("fileWithUri")]
    public Uri? Uri { get; init; }

    /// <summary>
    /// Gets the file's bytes, if any. Required if 'uri' is not set.
    /// </summary>
    [Description("The file's bytes, if any. Required if 'uri' is not set.")]
    [DataMember(Order = 4, Name = "fileWithBytes"), JsonPropertyOrder(4), JsonPropertyName("fileWithBytes")]
    public ReadOnlyMemory<byte>? Bytes { get; init; }

}
