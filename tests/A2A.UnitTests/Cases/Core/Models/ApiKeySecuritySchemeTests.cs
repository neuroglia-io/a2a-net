namespace A2A.UnitTests.Cases.Core.Models;

public class ApiKeySecuritySchemeTests
{

    [Fact]
    public void Serialize_And_Deserialize_ApiKeySecurityScheme_Should_Work()
    {
        //arrange
        var toSerialize = SecuritySchemeFactory.CreateApiKeySecurityScheme();

        //act
        var json = JsonSerializer.Serialize(toSerialize, JsonSerializationContext.Default.ApiKeySecurityScheme);
        var deserialized = JsonSerializer.Deserialize(json, JsonSerializationContext.Default.ApiKeySecurityScheme);

        //assert
        json.Should().NotBeNullOrEmpty();
        deserialized.Should().NotBeNull();
        deserialized.Should().BeJsonEquivalentTo(toSerialize);
    }

}
