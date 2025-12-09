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

internal static class AgentSkillFactory
{

    internal static AgentSkill Create() => new()
    {
        Id = Guid.NewGuid().ToString("N"),
        Name = "Sample Skill",
        Description = "This is a sample skill for testing purposes.",
        Tags = ["sample", "test", "skill"],
        Examples = 
        [
            "Example input 1 for the skill.",
            "Example input 2 for the skill."
        ],
        InputModes = ["text", "voice"],
        OutputModes = ["text", "image"],
        Security = 
        [
            new Dictionary<string, string[]>
            {
                ["apiKey"] = Array.Empty<string>()
            }
        ]
    };

}