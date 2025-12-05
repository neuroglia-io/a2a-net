namespace A2A.UnitTests.Cases.Core.Models;

public class SendMessageRequestTests
{

    [Fact]
    public void Serialize_And_Deserialize_SendMessageRequest_Should_Work()
    {
        //arrange
        var toSerialize = SendMessageRequestFactory.Create();
        
        //act
        var json = JsonSerializer.Serialize(toSerialize, JsonSerializationContext.Default.SendMessageRequest);
        var deserialized = JsonSerializer.Deserialize(json, JsonSerializationContext.Default.SendMessageRequest);
        
        //assert
        json.Should().NotBeNullOrEmpty();
        deserialized.Should().NotBeNull();
        deserialized.Should().BeJsonEquivalentTo(toSerialize);
    }

}
