namespace Neuroglia.A2A.Models;

/// <summary>
/// Represents a message
/// </summary>
[DataContract]
public record Message
{

    /// <summary>
    /// Gets/sets the message's role
    /// </summary>
    [Required, AllowedValues(MessageRole.Agent, MessageRole.User)]
    [DataMember(Name = "role", Order = 1), JsonPropertyName("role"), JsonPropertyOrder(1), YamlMember(Alias = "role", Order = 1)]
    public virtual string Role { get; set; } = null!;

    /// <summary>
    /// Gets/sets a list containing the message's parts, if any
    /// </summary>
    [DataMember(Name = "parts", Order = 2), JsonPropertyName("parts"), JsonPropertyOrder(2), YamlMember(Alias = "parts", Order = 2)]
    public virtual EquatableList<Part>? Parts { get; set; }

    /// <summary>
    /// Gets/sets a key/value mapping that contains the message's additional properties, if any
    /// </summary>
    [DataMember(Name = "metadata", Order = 3), JsonPropertyName("metadata"), JsonPropertyOrder(3), YamlMember(Alias = "metadata", Order = 3)]
    public virtual EquatableDictionary<string, object>? Metadata { get; set; }

}
