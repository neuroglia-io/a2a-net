namespace A2A.UnitTests.Cases.Core.Models;

public class TextPartTests
{

    [Fact]
    public void Serialize_And_Deserialize_TextPart_Should_Work()
    {
        //arrange
        var toSerialize = PartFactory.CreateTextPart();

        //act
        var json = JsonSerializer.Serialize(toSerialize, JsonSerializationContext.Default.TextPart);
        var deserialized = JsonSerializer.Deserialize(json, JsonSerializationContext.Default.TextPart);

        //assert
        json.Should().NotBeNullOrEmpty();
        deserialized.Should().NotBeNull();
        deserialized.Should().BeJsonEquivalentTo(toSerialize);
    }

}
