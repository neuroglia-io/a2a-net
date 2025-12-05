namespace A2A.UnitTests.Services;

internal static class PartFactory
{

    internal static DataPart CreateDataPart() => new()
    {
        Data = new()
        {
            ["key"] = "value"
        },
        Metadata = new Dictionary<string, JsonNode>
        {
            ["metaKey"] = "metaValue"
        }
    };

    internal static FilePart CreateUriFilePart() => new()
    {
        Name = "example.txt",
        MediaType = "text/plain",
        Uri = new Uri("https://example.com/example.txt"),
        Metadata = new Dictionary<string, JsonNode>
        {
            ["metaKey"] = "metaValue"
        }
    };

    internal static FilePart CreateDataFilePart() => new()
    {
        Name = "example.txt",
        MediaType = "text/plain",
        Bytes = Encoding.UTF8.GetBytes("This is an example file content."),
        Metadata = new Dictionary<string, JsonNode>
        {
            ["metaKey"] = "metaValue"
        }
    };

    internal static TextPart CreateTextPart() => new()
    {
        Text = "This is an example text part.",
        Metadata = new Dictionary<string, JsonNode>
        {
            ["metaKey"] = "metaValue"
        }
    };

}
