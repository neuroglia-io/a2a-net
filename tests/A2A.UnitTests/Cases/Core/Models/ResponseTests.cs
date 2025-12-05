namespace A2A.UnitTests.Cases.Core.Models;

public class ResponseTests
{

    [Fact]
    public void Serialize_And_Deserialize_Response_Should_Work()
    {
        //arrange
        var toSerialize = Services.TaskFactory.Create();
        
        //act
        var json = JsonSerializer.Serialize(toSerialize, JsonSerializationContext.Default.Response);
        var deserialized = JsonSerializer.Deserialize(json, JsonSerializationContext.Default.Response);
        
        //assert
        json.Should().NotBeNullOrEmpty();
        deserialized.Should().NotBeNull();
        deserialized.Should().BeJsonEquivalentTo(toSerialize);
    }

}
