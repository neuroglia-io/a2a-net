namespace A2A.UnitTests.Cases.Core.Models;

public class AgentSkillTests
{

    [Fact]
    public void Serialize_And_Deserialize_AgentSkill_Should_Work()
    {
        //arrange
        var toSerialize = AgentSkillFactory.Create();

        //act
        var json = JsonSerializer.Serialize(toSerialize, JsonSerializationContext.Default.AgentSkill);
        var deserialized = JsonSerializer.Deserialize(json, JsonSerializationContext.Default.AgentSkill);

        //assert
        json.Should().NotBeNullOrEmpty();
        deserialized.Should().NotBeNull();
        deserialized.Should().BeJsonEquivalentTo(toSerialize);
    }

}
