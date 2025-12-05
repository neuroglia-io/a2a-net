namespace A2A.UnitTests.Cases.Core.Models;

public class AgentProviderTests
{

    [Fact]
    public void Serialize_And_Deserialize_AgentProvider_Should_Work()
    {
        //arrange
        var toSerialize = AgentProviderFactory.Create();

        //act
        var json = JsonSerializer.Serialize(toSerialize, JsonSerializationContext.Default.AgentProvider);
        var deserialized = JsonSerializer.Deserialize(json, JsonSerializationContext.Default.AgentProvider);

        //assert
        json.Should().NotBeNullOrEmpty();
        deserialized.Should().NotBeNull();
        deserialized.Should().BeJsonEquivalentTo(toSerialize);
    }

}
