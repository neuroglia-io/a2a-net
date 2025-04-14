namespace Neuroglia.A2A.Models;

/// <summary>
/// Represents an object used to configure authentication
/// </summary>
[DataContract]
public record AuthenticationInfo
{

    /// <summary>
    /// Gets/sets the list of authentication schemes supported
    /// </summary>
    [Required, MinLength(1)]
    [DataMember(Name = "role", Order = 1), JsonPropertyName("role"), JsonPropertyOrder(1), YamlMember(Alias = "role", Order = 1)]
    public virtual EquatableList<string> Schemes { get; set; } = null!;

    /// <summary>
    /// Gets/sets the credentials, if any, used in conjunction with the specified authentication schemes
    /// </summary>
    [DataMember(Name = "credentials", Order = 2), JsonPropertyName("credentials"), JsonPropertyOrder(2), YamlMember(Alias = "credentials", Order = 2)]
    public virtual string? Credentials { get; set; }

}