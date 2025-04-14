using System.Text.Json;

namespace Neuroglia.A2A.UnitTests.Services;

internal static class MetadataFactory
{

    internal static EquatableDictionary<string, object> Create() => new()
    {
        { "fakeMetadata", JsonSerializer.SerializeToElement("fakeMetadataValue") }
    };

}