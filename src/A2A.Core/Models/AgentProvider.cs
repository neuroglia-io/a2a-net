namespace A2A.Models;

/// <summary>
/// Represents an agent's provider information.
/// </summary>
[Description("Represents an agent's provider information.")]
[DataContract]
public sealed record AgentProvider
{

    /// <summary>
    /// Gets the name of the provider's organization.
    /// </summary>
    [Description("The name of the provider's organization.")]
    [Required, MinLength(1)]
    [DataMember(Order = 1, Name = "organization"), JsonPropertyOrder(1), JsonPropertyName("organization")]
    public required string Organization { get; init; }

    /// <summary>
    /// Gets an URL pointing to the agent provider's website or relevant information.
    /// </summary>
    [Description("An URL pointing to the agent provider's website or relevant information.")]
    [Required]
    [DataMember(Order = 2, Name = "url"), JsonPropertyOrder(2), JsonPropertyName("url")]
    public required Uri Url { get; init; }

}