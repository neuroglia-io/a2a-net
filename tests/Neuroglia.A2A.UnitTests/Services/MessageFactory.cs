namespace Neuroglia.A2A.UnitTests.Services;

internal static class MessageFactory
{

    internal static Message Create() => new()
    {
        Role = MessageRole.User,
        Parts = PartFactory.CreateCollection()
    };

}
