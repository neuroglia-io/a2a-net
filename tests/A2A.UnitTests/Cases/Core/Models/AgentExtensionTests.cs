namespace A2A.UnitTests.Cases.Core.Models;

public class AgentExtensionTests
{

    [Fact]
    public void Serialize_And_Deserialize_AgentExtension_Should_Work()
    {
        //arrange
        var toSerialize = AgentExtensionFactory.Create();

        //act
        var json = JsonSerializer.Serialize(toSerialize, JsonSerializationContext.Default.AgentExtension);
        var deserialized = JsonSerializer.Deserialize(json, JsonSerializationContext.Default.AgentExtension);

        //assert
        json.Should().NotBeNullOrEmpty();
        deserialized.Should().NotBeNull();
        deserialized.Should().BeJsonEquivalentTo(toSerialize);
    }

}
