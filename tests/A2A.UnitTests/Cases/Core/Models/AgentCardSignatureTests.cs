namespace A2A.UnitTests.Cases.Core.Models;

public class AgentCardSignatureTests
{

    [Fact]
    public void Serialize_And_Deserialize_AgentCardSignature_Should_Work()
    {
        //arrange
        var toSerialize = AgentCardSignatureFactory.Create();

        //act
        var json = JsonSerializer.Serialize(toSerialize, JsonSerializationContext.Default.AgentCardSignature);
        var deserialized = JsonSerializer.Deserialize(json, JsonSerializationContext.Default.AgentCardSignature);

        //assert
        json.Should().NotBeNullOrEmpty();
        deserialized.Should().NotBeNull();
        deserialized.Should().BeJsonEquivalentTo(toSerialize);
    }

}
