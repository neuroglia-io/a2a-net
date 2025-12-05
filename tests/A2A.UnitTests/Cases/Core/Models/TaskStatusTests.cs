namespace A2A.UnitTests.Cases.Core.Models;

public class TaskStatusTests
{

    [Fact]
    public void Serialize_And_Deserialize_TaskStatus_Should_Work()
    {
        //arrange
        var toSerialize = TaskStatusFactory.Create();
        
        //act
        var json = JsonSerializer.Serialize(toSerialize, JsonSerializationContext.Default.TaskStatus);
        var deserialized = JsonSerializer.Deserialize(json, JsonSerializationContext.Default.TaskStatus);
        
        //assert
        json.Should().NotBeNullOrEmpty();
        deserialized.Should().NotBeNull();
        deserialized.Should().BeJsonEquivalentTo(toSerialize);
    }

}
