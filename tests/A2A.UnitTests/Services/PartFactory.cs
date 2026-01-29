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

namespace A2A.UnitTests.Services;

internal static class PartFactory
{

    internal static DataPart CreateDataPart() => new()
    {
        Data = new JsonObject()
        {
            ["key"] = "value"
        },
        Metadata = new Dictionary<string, JsonNode>
        {
            ["metaKey"] = "metaValue"
        }
    };

    internal static FilePart CreateUriFilePart() => new()
    {
        FileName = "example.txt",
        MediaType = "text/plain",
        Url = new Uri("https://example.com/example.txt"),
        Metadata = new Dictionary<string, JsonNode>
        {
            ["metaKey"] = "metaValue"
        }
    };

    internal static FilePart CreateDataFilePart() => new()
    {
        FileName = "example.txt",
        MediaType = "text/plain",
        Raw = Encoding.UTF8.GetBytes("This is an example file content."),
        Metadata = new Dictionary<string, JsonNode>
        {
            ["metaKey"] = "metaValue"
        }
    };

    internal static TextPart CreateTextPart() => new()
    {
        Text = "This is an example text part.",
        Metadata = new Dictionary<string, JsonNode>
        {
            ["metaKey"] = "metaValue"
        }
    };

}
