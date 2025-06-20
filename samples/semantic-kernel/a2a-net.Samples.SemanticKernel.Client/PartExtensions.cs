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

using System.Text;
using System.Text.Json;

namespace A2A.Samples.SemanticKernel.Client;

/// <summary>
/// Defines extensions for <see cref="Part"/>s
/// </summary>
public static class PartExtensions
{

    /// <summary>
    /// Converts the <see cref="Part"/> into text
    /// </summary>
    /// <param name="part">The <see cref="Part"/> to convert</param>
    /// <returns>The converted <see cref="Part"/></returns>
    public static string ToText(this Part part)
    {
        ArgumentNullException.ThrowIfNull(part);
        switch (part)
        {
            case TextPart textPart:
                return textPart.Text;
            case FilePart filePart:
                var fileContentBuilder = new StringBuilder();
                fileContentBuilder.AppendLine("----- FILE -----");
                if (!string.IsNullOrWhiteSpace(filePart.File.Name)) fileContentBuilder.AppendLine($"Name    : {filePart.File.Name}");
                if (!string.IsNullOrWhiteSpace(filePart.File.MimeType)) fileContentBuilder.AppendLine($"MIME    : {filePart.File.MimeType}");
                if (!string.IsNullOrWhiteSpace(filePart.File.Bytes)) fileContentBuilder.AppendLine($"Size    : {Convert.FromBase64String(filePart.File.Bytes).Length}");
                else if (filePart.File.Uri is not null) fileContentBuilder.AppendLine($"URI     : {filePart.File.Uri}");
                fileContentBuilder.AppendLine("----------------");
                return fileContentBuilder.ToString();
            case DataPart dataPart:
                var jsonContentBuilder = new StringBuilder();
                jsonContentBuilder.AppendLine("```json");
                jsonContentBuilder.AppendLine(JsonSerializer.Serialize(dataPart.Data));
                jsonContentBuilder.AppendLine("```");
                return jsonContentBuilder.ToString();
            default:
                throw new NotSupportedException($"The specified part type '{part.Kind ?? "None"}' is not supported");
        }
    }

}
