namespace A2A.UnitTests.Services;

internal static class AgentExtensionFactory
{

    internal static AgentExtension Create() => new()
    {
        Uri = new Uri("https://example.com/agent-extension"),
        Description = "Sample Agent Extension",
        Required = true,
        Params = new Dictionary<string, JsonNode>
        {
            ["param1"] = "value1",
            ["param2"] = 42
        }
    };

}