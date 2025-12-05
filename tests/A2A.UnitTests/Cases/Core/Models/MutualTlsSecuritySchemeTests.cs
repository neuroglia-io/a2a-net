namespace A2A.UnitTests.Cases.Core.Models;

public class MutualTlsSecuritySchemeTests
{

    [Fact]
    public void Serialize_And_Deserialize_MutualTlsSecurityScheme_Should_Work()
    {
        //arrange
        var toSerialize = SecuritySchemeFactory.CreateMutualTlsSecurityScheme();
        
        //act
        var json = JsonSerializer.Serialize(toSerialize, JsonSerializationContext.Default.MutualTlsSecurityScheme);
        var deserialized = JsonSerializer.Deserialize(json, JsonSerializationContext.Default.MutualTlsSecurityScheme);
        
        //assert
        json.Should().NotBeNullOrEmpty();
        deserialized.Should().NotBeNull();
        deserialized.Should().BeJsonEquivalentTo(toSerialize);
    }

}
