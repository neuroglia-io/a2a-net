namespace A2A.UnitTests.Services;

internal static class SendMessageRequestFactory
{

    internal static SendMessageRequest Create() => new()
    {
        Message = MessageFactory.Create(),
        Configuration = SendMessageConfigurationFactory.Create(),
        Metadata = new Dictionary<string, JsonNode>
        {
            ["metaKey"] = "metaValue"
        }
    };

}
