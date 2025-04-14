using System.Text.Json;

namespace Neuroglia.A2A.UnitTests.Services;

internal static class PartFactory
{

    internal static FilePart CreateFilePart() => new()
    {
        File = FileFactory.Create()
    };

    internal static DataPart CreateDataPart() => new()
    {
        Data = new()
        {
            { "fake-data", JsonSerializer.SerializeToElement("fake-data-value") }
        }
    };

    internal static TextPart CreateTextPart() => new()
    {
        Text = "fake-text"
    };

    internal static EquatableList<Part> CreateCollection() => [CreateFilePart(), CreateDataPart(), CreateTextPart()];

}
