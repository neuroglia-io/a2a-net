namespace A2A.UnitTests.Services;

internal static class PushNotificationConfigQueryOptionsFactory
{

    internal static PushNotificationConfigQueryOptions Create() => new()
    {
        TaskId = Guid.NewGuid().ToString("N"),
        PageSize = 50,
        PageToken = Guid.NewGuid().ToString("N")
    };

}
