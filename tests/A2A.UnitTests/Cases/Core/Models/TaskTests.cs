namespace A2A.UnitTests.Cases.Core.Models;

public class TaskTests
{

    [Fact]
    public void Serialize_And_Deserialize_Task_Should_Work()
    {
        //arrange
        var toSerialize = Services.TaskFactory.Create();

        //act
        var json = JsonSerializer.Serialize(toSerialize, JsonSerializationContext.Default.Task);
        var deserialized = JsonSerializer.Deserialize(json, JsonSerializationContext.Default.Task);

        //assert
        json.Should().NotBeNullOrEmpty();
        deserialized.Should().NotBeNull();
        deserialized.Should().BeJsonEquivalentTo(toSerialize);
    }

}
