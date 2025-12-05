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