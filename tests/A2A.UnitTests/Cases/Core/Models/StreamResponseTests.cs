namespace A2A.UnitTests.Cases.Core.Models;

public class StreamResponseTests
{

    [Fact]
    public void Serialize_And_Deserialize_StreamResponse_Should_Work()
    {
        //arrange
        var toSerialize = StreamResponseFactory.CreateTaskResponse();

        //act
        var json = JsonSerializer.Serialize(toSerialize, JsonSerializationContext.Default.StreamResponse);
        var deserialized = JsonSerializer.Deserialize(json, JsonSerializationContext.Default.StreamResponse);

        //assert
        json.Should().NotBeNullOrEmpty();
        deserialized.Should().NotBeNull();
        deserialized.Should().BeJsonEquivalentTo(toSerialize);
    }

}