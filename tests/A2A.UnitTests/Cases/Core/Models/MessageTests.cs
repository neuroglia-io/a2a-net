namespace A2A.UnitTests.Cases.Core.Models;

public class MessageTests
{

    [Fact]
    public void Serialize_And_Deserialize_Message_Should_Work()
    {
        //arrange
        var toSerialize = MessageFactory.Create();

        //act
        var json = JsonSerializer.Serialize(toSerialize, JsonSerializationContext.Default.Message);
        var deserialized = JsonSerializer.Deserialize(json, JsonSerializationContext.Default.Message);

        //assert
        json.Should().NotBeNullOrEmpty();
        deserialized.Should().NotBeNull();
        deserialized.Should().BeJsonEquivalentTo(toSerialize);
    }

}