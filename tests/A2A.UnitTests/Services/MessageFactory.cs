namespace A2A.UnitTests.Services;

internal static class MessageFactory
{

    internal static Message Create() => new()
    {
        MessageId = Guid.NewGuid().ToString("N"),
        ContextId = Guid.NewGuid().ToString("N"),
        TaskId = Guid.NewGuid().ToString("N"),
        Role = Role.User,
        Parts =
        [
            PartFactory.CreateDataPart(),
            PartFactory.CreateDataFilePart(),
            PartFactory.CreateUriFilePart(),
            PartFactory.CreateTextPart(),
        ],
        Metadata = new Dictionary<string, JsonNode>
        {
            ["metaKey"] = "metaValue"
        },
        Extensions =
        [
            new Uri("https://example.com/extension1"),
            new Uri("https://example.com/extension2"),
        ],
        ReferencedTaskIds =
        [
            Guid.NewGuid().ToString("N"),
            Guid.NewGuid().ToString("N"),
        ]
    };

}
