namespace A2A.UnitTests.Cases.Core.Models;

public class PushNotificationConfigQueryResultTests
{

    [Fact]
    public void Serialize_And_Deserialize_PushNotificationConfigQueryResult_Should_Work()
    {
        //arrange
        var toSerialize = PushNotificationConfigQueryResultFactory.Create();

        //act
        var json = JsonSerializer.Serialize(toSerialize, JsonSerializationContext.Default.PushNotificationConfigQueryResult);
        var deserialized = JsonSerializer.Deserialize(json, JsonSerializationContext.Default.PushNotificationConfigQueryResult);

        //assert
        json.Should().NotBeNullOrEmpty();
        deserialized.Should().NotBeNull();
        deserialized.Should().BeJsonEquivalentTo(toSerialize);
    }

}