namespace Neuroglia.A2A.Models;

/// <summary>
/// Represents a data part
/// </summary>
[DataContract]
public record DataPart
    : Part
{

    /// <inheritdoc/>
    [IgnoreDataMember, JsonIgnore, YamlIgnore]
    public override string Type => PartType.Data;

    /// <summary>
    /// Gets/sets the part's data
    /// </summary>
    [Required]
    [DataMember(Name = "data", Order = 1), JsonPropertyName("data"), JsonPropertyOrder(1), YamlMember(Alias = "data", Order = 1)]
    public virtual EquatableDictionary<string, object> Data { get; set; } = null!;

}
