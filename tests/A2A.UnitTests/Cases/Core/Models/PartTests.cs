namespace A2A.UnitTests.Cases.Core.Models;

public class PartTests
{

    [Fact]
    public void Serialize_And_Deserialize_Part_Should_Work()
    {
        //arrange
        var toSerialize = PartFactory.CreateTextPart();

        //act
        var json = JsonSerializer.Serialize(toSerialize, JsonSerializationContext.Default.Part);
        var deserialized = JsonSerializer.Deserialize(json, JsonSerializationContext.Default.Part);

        //assert
        json.Should().NotBeNullOrEmpty();
        deserialized.Should().NotBeNull();
        deserialized.Should().BeJsonEquivalentTo(toSerialize);
    }

}
