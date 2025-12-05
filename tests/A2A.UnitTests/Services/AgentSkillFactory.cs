namespace A2A.UnitTests.Services;

internal static class AgentSkillFactory
{

    internal static AgentSkill Create() => new()
    {
        Id = Guid.NewGuid().ToString("N"),
        Name = "Sample Skill",
        Description = "This is a sample skill for testing purposes.",
        Tags = ["sample", "test", "skill"],
        Examples = 
        [
            "Example input 1 for the skill.",
            "Example input 2 for the skill."
        ],
        InputModes = ["text", "voice"],
        OutputModes = ["text", "image"],
        Security = 
        [
            new Dictionary<string, string[]>
            {
                ["apiKey"] = Array.Empty<string>()
            }
        ]
    };

}