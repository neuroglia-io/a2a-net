namespace Neuroglia.A2A.UnitTests.Services;

internal static class FileFactory
{

    internal static Models.File Create() => new()
    {
        Name = "fake-file",
        MimeType = "fake-mime-type",
        Bytes = "fake-file-content",
        Uri = new("https://fake.com")
    };

}

