namespace Neuroglia.A2A.Models;

/// <summary>
/// Represents an object used to describe an agent's provider 
/// </summary>
[DataContract]
public record AgentProvider
{

    /// <summary>
    /// Gets/sets the name of the organization that provides or maintains the agent
    /// </summary>
    [Required, MinLength(1)]
    [DataMember(Name = "organization", Order = 1), JsonPropertyName("organization"), JsonPropertyOrder(1), YamlMember(Alias = "organization", Order = 1)]
    public virtual string Organization { get; set; } = null!;

    /// <summary>
    /// Gets/sets a url, if any, referencing the official website of the agent's organization or provider
    /// </summary>
    [DataMember(Name = "url", Order = 2), JsonPropertyName("organization"), JsonPropertyOrder(2), YamlMember(Alias = "organization", Order = 2)]
    public virtual Uri? Url { get; set; }

}