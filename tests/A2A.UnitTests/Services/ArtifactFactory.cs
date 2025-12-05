namespace A2A.UnitTests.Services;

internal static class ArtifactFactory
{

    internal static Artifact Create() => new()
    {
        ArtifactId = Guid.NewGuid().ToString("N"),
        Name = "Sample Artifact",
        Description = "This is a sample artifact for testing purposes.",
        Parts =
        [
            PartFactory.CreateDataPart(),
            PartFactory.CreateDataFilePart(),
            PartFactory.CreateUriFilePart(),
            PartFactory.CreateTextPart(),
        ],
        Metadata = new Dictionary<string, JsonNode>
        {
            ["artifactMetaKey"] = "artifactMetaValue"
        },
        Extensions =
        [
            new Uri("https://example.com/extension1"),
            new Uri("https://example.com/extension2"),
        ],
    };

}
