namespace A2A.Models;

/// <summary>
/// Represents a distinct capability or function that an agent can perform.
/// </summary>
[Description("Represents a distinct capability or function that an agent can perform.")]
[DataContract]
public sealed record AgentSkill
{

    /// <summary>
    /// Gets the skill's unique identifier.
    /// </summary>
    [Description("The skill's unique identifier.")]
    [Required, MinLength(1)]
    [DataMember(Order = 1, Name = "id"), JsonPropertyOrder(1), JsonPropertyName("id")]
    public string Id { get; init; } = Guid.NewGuid().ToString("N");

    /// <summary>
    /// Gets the skill's human readable name.
    /// </summary>
    [Description("The skill's human readable name.")]
    [Required, MinLength(1)]
    [DataMember(Order = 2, Name = "name"), JsonPropertyOrder(2), JsonPropertyName("name")]
    public required string Name { get; init; }

    /// <summary>
    /// Gets the skill's human readable description.
    /// </summary>
    [Description("The skill's human readable description.")]
    [Required, MinLength(1)]
    [DataMember(Order = 3, Name = "description"), JsonPropertyOrder(3), JsonPropertyName("description")]
    public required string Description { get; init; }

    /// <summary>
    /// Gets a collection containing the tags associated with the skill.
    /// </summary>
    [Description("A collection containing the tags associated with the skill.")]
    [Required, MinLength(1)]
    [DataMember(Order = 4, Name = "tags"), JsonPropertyOrder(4), JsonPropertyName("tags")]
    public required IReadOnlyCollection<string> Tags { get; init; }

    /// <summary>
    /// Gets a collection containing example prompts or scenarios that this skill can handle.
    /// </summary>
    [Description("A collection containing example prompts or scenarios that this skill can handle.")]
    [DataMember(Order = 5, Name = "examples"), JsonPropertyOrder(5), JsonPropertyName("examples")]
    public IReadOnlyCollection<string>? Examples { get; init; }

    /// <summary>
    /// Gets a collection containing the input modes supported by the skill.
    /// </summary>
    [Description("A collection containing the input modes supported by the skill.")]
    [DataMember(Order = 6, Name = "inputModes"), JsonPropertyOrder(6), JsonPropertyName("inputModes")]
    public IReadOnlyCollection<string>? InputModes { get; init; }

    /// <summary>
    /// Gets a collection containing the output modes supported by the skill.
    /// </summary>
    [Description("A collection containing the output modes supported by the skill.")]
    [DataMember(Order = 7, Name = "outputModes"), JsonPropertyOrder(7), JsonPropertyName("outputModes")]
    public IReadOnlyCollection<string>? OutputModes { get; init; }

    /// <summary>
    /// Gets a collection containing the security requirements, if any, needed to utilize the skill.
    /// </summary>
    [Description("A collection containing the security requirements, if any, needed to utilize the skill.")]
    [DataMember(Order = 8, Name = "security"), JsonPropertyOrder(8), JsonPropertyName("security")]
    public IReadOnlyCollection<IReadOnlyDictionary<string, string[]>>? Security { get; set; }

}
