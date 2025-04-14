namespace Neuroglia.A2A.Models;

/// <summary>
/// Represents a fully formed piece of content exchanged between a client and a remote agent as part of a message or an artifact
/// </summary>
[DataContract]
[JsonPolymorphic(TypeDiscriminatorPropertyName = "type")]
[JsonDerivedType(typeof(DataPart), PartType.Data)]
[JsonDerivedType(typeof(FilePart), PartType.File)]
[JsonDerivedType(typeof(TextPart), PartType.Text)]
public abstract record Part
{

    /// <summary>
    /// Gets the part's type
    /// </summary>
    public abstract string Type { get; }

    /// <summary>
    /// Gets/sets a key/value mapping that contains the message's additional properties, if any
    /// </summary>
    [DataMember(Name = "metadata", Order = 99), JsonPropertyName("metadata"), JsonPropertyOrder(99), YamlMember(Alias = "metadata", Order = 99)]
    public virtual EquatableDictionary<string, object>? Metadata { get; set; }

}
