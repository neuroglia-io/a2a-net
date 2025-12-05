namespace A2A.UnitTests.Cases.Core.Models;

public class OAuthFlowsTests
{

    [Fact]
    public void Serialize_And_Deserialize_OAuthFlows_Should_Work()
    {
        //arrange
        var toSerialize = OAuthFlowsFactory.Create();
        
        //act
        var json = JsonSerializer.Serialize(toSerialize, JsonSerializationContext.Default.OAuthFlows);
        var deserialized = JsonSerializer.Deserialize(json, JsonSerializationContext.Default.OAuthFlows);
        
        //assert
        json.Should().NotBeNullOrEmpty();
        deserialized.Should().NotBeNull();
        deserialized.Should().BeJsonEquivalentTo(toSerialize);
    }

}
