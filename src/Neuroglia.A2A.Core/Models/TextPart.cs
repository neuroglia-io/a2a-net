namespace Neuroglia.A2A.Models;

/// <summary>
/// Represents a text part
/// </summary>
[DataContract]
public record TextPart
    : Part
{

    /// <summary>
    /// Initializes a new <see cref="TextPart"/>
    /// </summary>
    public TextPart() { }

    /// <summary>
    /// Initializes a new <see cref="TextPart"/>
    /// </summary>
    /// <param name="text">The part's text</param>
    public TextPart(string text) 
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(text);
        Text = text;
    }

    /// <inheritdoc/>
    [IgnoreDataMember, JsonIgnore, YamlIgnore]
    public override string Type => PartType.Text;

    /// <summary>
    /// Gets/sets the part's text
    /// </summary>
    [Required]
    [DataMember(Name = "text", Order = 1), JsonPropertyName("text"), JsonPropertyOrder(1), YamlMember(Alias = "text", Order = 1)]
    public virtual string Text { get; set; } = null!;

}
