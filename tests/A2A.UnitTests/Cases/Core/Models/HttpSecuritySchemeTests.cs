namespace A2A.UnitTests.Cases.Core.Models;

public class HttpSecuritySchemeTests
{

    [Fact]
    public void Serialize_And_Deserialize_HttpSecurityScheme_Should_Work()
    {
        //arrange
        var toSerialize = SecuritySchemeFactory.CreateHttpSecurityScheme();
        
        //act
        var json = JsonSerializer.Serialize(toSerialize, JsonSerializationContext.Default.HttpSecurityScheme);
        var deserialized = JsonSerializer.Deserialize(json, JsonSerializationContext.Default.HttpSecurityScheme);
        
        //assert
        json.Should().NotBeNullOrEmpty();
        deserialized.Should().NotBeNull();
        deserialized.Should().BeJsonEquivalentTo(toSerialize);
    }

}
