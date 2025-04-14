namespace Neuroglia.A2A.UnitTests.Services;

internal static class PushNotificationConfigurationFactory
{

    internal static PushNotificationConfiguration Create() => new()
    {
        Url = new("https://fake.com"),
        Token = "fake-token",
        Authentication = AuthenticationInfoFactory.Create()
    };

}