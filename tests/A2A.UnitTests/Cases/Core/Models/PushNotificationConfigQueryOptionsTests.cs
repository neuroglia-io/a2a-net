namespace A2A.UnitTests.Cases.Core.Models;

public class PushNotificationConfigQueryOptionsTests
{

    [Fact]
    public void Serialize_And_Deserialize_PushNotificationConfigQueryOptions_Should_Work()
    {
        //arrange
        var toSerialize = PushNotificationConfigQueryOptionsFactory.Create();
        //act
        var json = JsonSerializer.Serialize(toSerialize, JsonSerializationContext.Default.PushNotificationConfigQueryOptions);
        var deserialized = JsonSerializer.Deserialize(json, JsonSerializationContext.Default.PushNotificationConfigQueryOptions);
        //assert
        json.Should().NotBeNullOrEmpty();
        deserialized.Should().NotBeNull();
        deserialized.Should().BeJsonEquivalentTo(toSerialize);
    }

}
