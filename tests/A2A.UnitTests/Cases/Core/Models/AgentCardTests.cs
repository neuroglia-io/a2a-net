namespace A2A.UnitTests.Cases.Core.Models;

public class AgentCardTests
{

    [Fact]
    public void Serialize_And_Deserialize_AgentCard_Should_Work()
    {
        //arrange
        var toSerialize = AgentCardFactory.Create();

        //act
        var json = JsonSerializer.Serialize(toSerialize, JsonSerializationContext.Default.AgentCard);
        var deserialized = JsonSerializer.Deserialize(json, JsonSerializationContext.Default.AgentCard);

        //assert
        json.Should().NotBeNullOrEmpty();
        deserialized.Should().NotBeNull();
        deserialized.Should().BeJsonEquivalentTo(toSerialize);
    }

}
