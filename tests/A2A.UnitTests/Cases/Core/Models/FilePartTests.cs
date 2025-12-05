namespace A2A.UnitTests.Cases.Core.Models;

public class FilePartTests
{

    [Fact]
    public void Serialize_And_Deserialize_UriFilePart_Should_Work()
    {
        //arrange
        var toSerialize = PartFactory.CreateUriFilePart();

        //act
        var json = JsonSerializer.Serialize(toSerialize, JsonSerializationContext.Default.FilePart);
        var deserialized = JsonSerializer.Deserialize(json, JsonSerializationContext.Default.FilePart);

        //assert
        json.Should().NotBeNullOrEmpty();
        deserialized.Should().NotBeNull();
        deserialized.Should().BeJsonEquivalentTo(toSerialize);
    }

    [Fact]
    public void Serialize_And_Deserialize_DataFilePart_Should_Work()
    {
        //arrange
        var toSerialize = PartFactory.CreateDataFilePart();

        //act
        var json = JsonSerializer.Serialize(toSerialize, JsonSerializationContext.Default.FilePart);
        var deserialized = JsonSerializer.Deserialize(json, JsonSerializationContext.Default.FilePart);

        //assert
        json.Should().NotBeNullOrEmpty();
        deserialized.Should().NotBeNull();
        deserialized.Should().BeJsonEquivalentTo(toSerialize);
    }

}
