namespace A2A.UnitTests.Services;

internal static class SendMessageConfigurationFactory
{

    internal static SendMessageConfiguration Create() => new()
    {
        AcceptedOutputModes = ["text", "json"],
        Blocking = true,
        HistoryLength = 10,
        PushNotificationConfig = PushNotificationConfigFactory.Create()
    };

}