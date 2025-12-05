namespace A2A.UnitTests.Cases.Core.Models;

public class ArtifactTests
{     
    
    [Fact]
    public void Serialize_And_Deserialize_Artifact_Should_Work()
    {
        //arrange
        var toSerialize = ArtifactFactory.Create();

        //act
        var json = JsonSerializer.Serialize(toSerialize, JsonSerializationContext.Default.Artifact);
        var deserialized = JsonSerializer.Deserialize(json, JsonSerializationContext.Default.Artifact);

        //assert
        json.Should().NotBeNullOrEmpty();
        deserialized.Should().NotBeNull();
        deserialized.Should().BeJsonEquivalentTo(toSerialize);
    }

}
