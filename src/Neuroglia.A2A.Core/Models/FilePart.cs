namespace Neuroglia.A2A.Models;

/// <summary>
/// Represents a file part
/// </summary>
[DataContract]
public record FilePart
    : Part
{

    /// <inheritdoc/>
    [IgnoreDataMember, JsonIgnore, YamlIgnore]
    public override string Type => PartType.File;

    /// <summary>
    /// Gets/sets the part's text
    /// </summary>
    [Required]
    [DataMember(Name = "file", Order = 1), JsonPropertyName("file"), JsonPropertyOrder(1), YamlMember(Alias = "file", Order = 1)]
    public virtual File File { get; set; } = null!;

}
