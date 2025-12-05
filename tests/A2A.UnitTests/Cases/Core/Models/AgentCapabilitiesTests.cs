namespace A2A.UnitTests.Cases.Core.Models;

public class AgentCapabilitiesTests
{     
    
    [Fact]
    public void Serialize_And_Deserialize_AgentCapabilities_Should_Work()
    {
        //arrange
        var toSerialize = AgentCapabilitiesFactory.Create();

        //act
        var json = JsonSerializer.Serialize(toSerialize, JsonSerializationContext.Default.AgentCapabilities);
        var deserialized = JsonSerializer.Deserialize(json, JsonSerializationContext.Default.AgentCapabilities);

        //assert
        json.Should().NotBeNullOrEmpty();
        deserialized.Should().NotBeNull();
        deserialized.Should().BeJsonEquivalentTo(toSerialize);
    }

}
