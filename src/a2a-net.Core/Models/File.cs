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
/// Represents a file data, used within a FilePart.
/// </summary>
[Description("Represents a file data, used within a FilePart.")]
[DataContract]
public record File
{

    /// <summary>
    /// Gets or sets the file's name, if any.
    /// </summary>
    [Description("The file's name, if any.")]
    [DataMember(Name = "name", Order = 1), JsonPropertyName("name"), JsonPropertyOrder(1), YamlMember(Alias = "name", Order = 1)]
    public virtual string? Name { get; set; }

    /// <summary>
    /// Gets or sets the file's MIME type, if any.
    /// </summary>
    [Description("The file's MIME type, if any.")]
    [DataMember(Name = "mimeType", Order = 2), JsonPropertyName("mimeType"), JsonPropertyOrder(2), YamlMember(Alias = "mimeType", Order = 2)]
    public virtual string? MimeType { get; set; }

    /// <summary>
    /// Gets or sets the file's base64 encoded content, if any.
    /// </summary>
    [Description("The file's base64 encoded content, if any.")]
    [DataMember(Name = "bytes", Order = 3), JsonPropertyName("bytes"), JsonPropertyOrder(3), YamlMember(Alias = "bytes", Order = 3)]
    public virtual string? Bytes { get; set; }

    /// <summary>
    /// Gets or sets the file's uri, if any.
    /// </summary>
    [Description("The file's uri, if any.")]
    [DataMember(Name = "uri", Order = 3), JsonPropertyName("uri"), JsonPropertyOrder(3), YamlMember(Alias = "uri", Order = 3)]
    public virtual Uri? Uri { get; set; }

}