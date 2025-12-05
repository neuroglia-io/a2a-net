namespace A2A.UnitTests.Cases.Core.Models;

public class OpenIdConnectSecuritySchemeTests
{

    [Fact]
    public void Serialize_And_Deserialize_OpenIdConnectSecurityScheme_Should_Work()
    {
        //arrange
        var toSerialize = SecuritySchemeFactory.CreateOpenIdConnectSecurityScheme();
        
        //act
        var json = JsonSerializer.Serialize(toSerialize, JsonSerializationContext.Default.OpenIdConnectSecurityScheme);
        var deserialized = JsonSerializer.Deserialize(json, JsonSerializationContext.Default.OpenIdConnectSecurityScheme);
        
        //assert
        json.Should().NotBeNullOrEmpty();
        deserialized.Should().NotBeNull();
        deserialized.Should().BeJsonEquivalentTo(toSerialize);
    }

}
