namespace A2A.UnitTests.Cases.Core.Models;

public class OAuth2SecuritySchemeTests
{

    [Fact]
    public void Serialize_And_Deserialize_OAuth2SecurityScheme_Should_Work()
    {
        //arrange
        var toSerialize = SecuritySchemeFactory.CreateOAuth2SecurityScheme();
        
        //act
        var json = JsonSerializer.Serialize(toSerialize, JsonSerializationContext.Default.OAuth2SecurityScheme);
        var deserialized = JsonSerializer.Deserialize(json, JsonSerializationContext.Default.OAuth2SecurityScheme);
        
        //assert
        json.Should().NotBeNullOrEmpty();
        deserialized.Should().NotBeNull();
        deserialized.Should().BeJsonEquivalentTo(toSerialize);
    }

}
