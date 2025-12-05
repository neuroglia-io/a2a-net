namespace A2A.UnitTests.Cases.Core.Models;

public class TaskArtifactUpdateEventTests
{

    [Fact]
    public void Serialize_And_Deserialize_TaskArtifactUpdateEvent_Should_Work()
    {
        //arrange
        var toSerialize = TaskArtifactUpdateEventFactory.Create();

        //act
        var json = JsonSerializer.Serialize(toSerialize, JsonSerializationContext.Default.TaskArtifactUpdateEvent);
        var deserialized = JsonSerializer.Deserialize(json, JsonSerializationContext.Default.TaskArtifactUpdateEvent);

        //assert
        json.Should().NotBeNullOrEmpty();
        deserialized.Should().NotBeNull();
        deserialized.Should().BeJsonEquivalentTo(toSerialize);
    }

}