namespace A2A.UnitTests.Cases.Core.Models;

public class TaskStatusUpdateEventTests
{

    [Fact]
    public void Serialize_And_Deserialize_TaskStatusUpdateEvent_Should_Work()
    {
        //arrange
        var toSerialize = TaskStatusUpdateEventFactory.Create();

        //act
        var json = JsonSerializer.Serialize(toSerialize, JsonSerializationContext.Default.TaskStatusUpdateEvent);
        var deserialized = JsonSerializer.Deserialize(json, JsonSerializationContext.Default.TaskStatusUpdateEvent);

        //assert
        json.Should().NotBeNullOrEmpty();
        deserialized.Should().NotBeNull();
        deserialized.Should().BeJsonEquivalentTo(toSerialize);
    }

}
