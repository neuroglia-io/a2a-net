namespace A2A.Models;

/// <summary>
/// Represents a part used to transport human-readable text.
/// </summary>
[Description("Represents a part used to transport human-readable text.")]
[DataContract]
public sealed record TextPart 
    : Part
{

    /// <summary>
    /// Gets the part's text content.
    /// </summary>
    [Description("The part's text content.")]
    [Required, MinLength(1)]
    [DataMember(Order = 1, Name = "text"), JsonPropertyOrder(1), JsonPropertyName("text")]
    public required string Text { get; init; }

}
