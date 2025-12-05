namespace A2A.Models;

/// <summary>
/// Represents a part used to transport structured JSON data.
/// </summary>
[Description("Represents a part used to transport structured JSON data.")]
[DataContract]
public sealed record DataPart
    : Part
{

    /// <summary>
    /// Gets a JSON object containing arbitrary data.
    /// </summary>
    [Description("A JSON object containing arbitrary data.")]
    [Required]
    [DataMember(Order = 1, Name = "data"), JsonPropertyOrder(1), JsonPropertyName("data")]
    public required JsonObject Data { get; init; }

}