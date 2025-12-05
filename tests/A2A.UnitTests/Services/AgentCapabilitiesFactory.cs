namespace A2A.UnitTests.Services;

internal static class AgentCapabilitiesFactory
{

    internal static AgentCapabilities Create() => new()
    {
        Streaming = true,
        PushNotifications = true,
        StateTransitionHistory = true,
        Extensions =
        [
            AgentExtensionFactory.Create()
        ]
    };

}