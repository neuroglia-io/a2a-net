namespace A2A.UnitTests.Cases.Core.Models;

public class TaskQueryOptionsTests
{

    [Fact]
    public void Serialize_And_Deserialize_TaskQueryOptions_Should_Work()
    {
        //arrange
        var toSerialize = TaskQueryOptionsFactory.Create();

        //act
        var json = JsonSerializer.Serialize(toSerialize, JsonSerializationContext.Default.TaskQueryOptions);
        var deserialized = JsonSerializer.Deserialize(json, JsonSerializationContext.Default.TaskQueryOptions);

        //assert
        json.Should().NotBeNullOrEmpty();
        deserialized.Should().NotBeNull();
        deserialized.Should().BeJsonEquivalentTo(toSerialize);
    }

}
