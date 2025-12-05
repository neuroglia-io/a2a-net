namespace A2A.UnitTests.Cases.Core.Models;

public class AgentInterfaceTests
{

    [Fact]
    public void Serialize_And_Deserialize_AgentInterface_Should_Work()
    {
        //arrange
        var toSerialize = AgentInterfaceFactory.Create();

        //act
        var json = JsonSerializer.Serialize(toSerialize, JsonSerializationContext.Default.AgentInterface);
        var deserialized = JsonSerializer.Deserialize(json, JsonSerializationContext.Default.AgentInterface);

        //assert
        json.Should().NotBeNullOrEmpty();
        deserialized.Should().NotBeNull();
        deserialized.Should().BeJsonEquivalentTo(toSerialize);
    }

}
