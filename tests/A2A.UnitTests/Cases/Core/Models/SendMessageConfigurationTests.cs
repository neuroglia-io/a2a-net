namespace A2A.UnitTests.Cases.Core.Models;

public class SendMessageConfigurationTests
{

    [Fact]
    public void Serialize_And_Deserialize_SendMessageConfiguration_Should_Work()
    {
        //arrange
        var toSerialize = SendMessageConfigurationFactory.Create();
        
        //act
        var json = JsonSerializer.Serialize(toSerialize, JsonSerializationContext.Default.SendMessageConfiguration);
        var deserialized = JsonSerializer.Deserialize(json, JsonSerializationContext.Default.SendMessageConfiguration);
        
        //assert
        json.Should().NotBeNullOrEmpty();
        deserialized.Should().NotBeNull();
        deserialized.Should().BeJsonEquivalentTo(toSerialize);
    }

}