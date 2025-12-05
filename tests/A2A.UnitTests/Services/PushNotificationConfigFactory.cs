namespace A2A.UnitTests.Services;

internal static class PushNotificationConfigFactory
{

    internal static PushNotificationConfig Create() => new()
    {
        Id = Guid.NewGuid().ToString("N"),
        Url = new Uri("https://example.com/notify"),
        Token = Guid.NewGuid().ToString("N"),
        Authentication = AuthenticationInfoFactory.Create()
    };

}
