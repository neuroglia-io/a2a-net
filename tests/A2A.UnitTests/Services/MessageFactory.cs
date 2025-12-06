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

internal static class MessageFactory
{

    internal static Message Create() => new()
    {
        MessageId = Guid.NewGuid().ToString("N"),
        ContextId = Guid.NewGuid().ToString("N"),
        TaskId = Guid.NewGuid().ToString("N"),
        Role = Role.User,
        Parts =
        [
            PartFactory.CreateDataPart(),
            PartFactory.CreateDataFilePart(),
            PartFactory.CreateUriFilePart(),
            PartFactory.CreateTextPart(),
        ],
        Metadata = new Dictionary<string, JsonNode>
        {
            ["metaKey"] = "metaValue"
        },
        Extensions =
        [
            new Uri("https://example.com/extension1"),
            new Uri("https://example.com/extension2"),
        ],
        ReferencedTaskIds =
        [
            Guid.NewGuid().ToString("N"),
            Guid.NewGuid().ToString("N"),
        ]
    };

}
