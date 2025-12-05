namespace A2A.UnitTests.Services;

internal static class AgentInterfaceFactory
{

    internal static AgentInterface Create() => new()
    {
        ProtocolBinding = A2A.ProtocolBinding.Grpc,
        Url = new Uri("https://example.com/agentinterface")
    };

}