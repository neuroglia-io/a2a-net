namespace A2A.UnitTests.Cases.Core.Models;

public class DataPartTests
{

    [Fact]
    public void Serialize_And_Deserialize_DataPart_Should_Work()
    {
        //arrange
        var toSerialize = PartFactory.CreateDataPart();

        //act
        var json = JsonSerializer.Serialize(toSerialize, JsonSerializationContext.Default.DataPart);
        var deserialized = JsonSerializer.Deserialize(json, JsonSerializationContext.Default.DataPart);

        //assert
        json.Should().NotBeNullOrEmpty();
        deserialized.Should().NotBeNull();
        deserialized.Should().BeJsonEquivalentTo(toSerialize);
    }

}   
