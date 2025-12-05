namespace A2A.UnitTests.Cases.Core.Models;

public class PushNotificationConfigTests
{     
    
    [Fact]
    public void Serialize_And_Deserialize_PushNotificationConfig_Should_Work()
    {
        //arrange
        var toSerialize = PushNotificationConfigFactory.Create();

        //act
        var json = JsonSerializer.Serialize(toSerialize, JsonSerializationContext.Default.PushNotificationConfig);
        var deserialized = JsonSerializer.Deserialize(json, JsonSerializationContext.Default.PushNotificationConfig);

        //assert
        json.Should().NotBeNullOrEmpty();
        deserialized.Should().NotBeNull();
        deserialized.Should().BeJsonEquivalentTo(toSerialize);
    }

}
