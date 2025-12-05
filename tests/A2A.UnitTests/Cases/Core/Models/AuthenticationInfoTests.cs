namespace A2A.UnitTests.Cases.Core.Models;

public class AuthenticationInfoTests
{     
    
    [Fact]
    public void Serialize_And_Deserialize_AuthenticationInfo_Should_Work()
    {
        //arrange
        var toSerialize = AuthenticationInfoFactory.Create();
        
        //act
        var json = JsonSerializer.Serialize(toSerialize, JsonSerializationContext.Default.AuthenticationInfo);
        var deserialized = JsonSerializer.Deserialize(json, JsonSerializationContext.Default.AuthenticationInfo);
        
        //assert
        json.Should().NotBeNullOrEmpty();
        deserialized.Should().NotBeNull();
        deserialized.Should().BeJsonEquivalentTo(toSerialize);
    }

}