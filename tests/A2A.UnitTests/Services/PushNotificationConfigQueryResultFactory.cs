namespace A2A.UnitTests.Services;

internal static class PushNotificationConfigQueryResultFactory
{

    internal static PushNotificationConfigQueryResult Create() => new()
    {
        Configs = [PushNotificationConfigFactory.Create()],
        NextPageToken = Guid.NewGuid().ToString("N")
    };

}