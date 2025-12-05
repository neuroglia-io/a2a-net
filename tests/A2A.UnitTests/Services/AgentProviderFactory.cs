namespace A2A.UnitTests.Services;

internal static class AgentProviderFactory
{

    internal static AgentProvider Create() => new()
    {
        Organization = "Sample Organization",
        Url = new Uri("https://example.com/provider")
    };

}