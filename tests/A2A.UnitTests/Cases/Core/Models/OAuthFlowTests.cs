namespace A2A.UnitTests.Cases.Core.Models;

public class OAuthFlowTests
{

    [Fact]
    public void Serialize_And_Deserialize_OAuthFlow_Should_Work()
    {
        //arrange
        var toSerialize = OAuthFlowFactory.CreateAuthorizationCodeFlow();
        
        //act
        var json = JsonSerializer.Serialize(toSerialize, JsonSerializationContext.Default.OAuthFlow);
        var deserialized = JsonSerializer.Deserialize(json, JsonSerializationContext.Default.OAuthFlow);
        
        //assert
        json.Should().NotBeNullOrEmpty();
        deserialized.Should().NotBeNull();
        deserialized.Should().BeJsonEquivalentTo(toSerialize);
    }

}
