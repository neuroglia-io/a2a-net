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

internal static class AgentCardFactory
{

    internal static AgentCard Create() => new()
    {
        ProtocolVersion = "1.0",
        Name = "Sample Agent",
        Description = "This is a sample agent for testing purposes.",
        Version = "1.0.0",
        Provider = AgentProviderFactory.Create(),
        IconUrl = new Uri("https://example.com/icon.png"),
        DocumentationUrl = new Uri("https://example.com/docs"),
        Interfaces = 
        [ 
            AgentInterfaceFactory.Create() 
        ],
        Capabilities = AgentCapabilitiesFactory.Create(),
        Skills = 
        [
            AgentSkillFactory.Create()
        ],
        DefaultInputModes = ["text", "voice"],
        DefaultOutputModes = ["text", "image"],
        SecuritySchemes = new Dictionary<string, SecurityScheme>
        {
            ["apiKeyScheme"] = SecuritySchemeFactory.CreateApiKeySecurityScheme()
        },
        Security = 
        [ 
            new Dictionary<string, string[]>
            {
                ["apiKey"] = Array.Empty<string>()
            }
        ],
        Signatures = 
        [ 
            AgentCardSignatureFactory.Create() 
        ],
        SupportsAuthenticatedExtendedCard = true
    };

}