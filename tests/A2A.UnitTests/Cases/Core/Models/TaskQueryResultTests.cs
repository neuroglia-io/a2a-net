namespace A2A.UnitTests.Cases.Core.Models;

public class TaskQueryResultTests
{

    [Fact]
    public void Serialize_And_Deserialize_TaskQueryResult_Should_Work()
    {
        //arrange
        var toSerialize = TaskQueryResultFactory.Create();
        
        //act
        var json = JsonSerializer.Serialize(toSerialize, JsonSerializationContext.Default.TaskQueryResult);
        var deserialized = JsonSerializer.Deserialize(json, JsonSerializationContext.Default.TaskQueryResult);
        
        //assert
        json.Should().NotBeNullOrEmpty();
        deserialized.Should().NotBeNull();
        deserialized.Should().BeJsonEquivalentTo(toSerialize);
    }

}