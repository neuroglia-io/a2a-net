using A2A.Serialization.Json;

namespace A2A.Models;

/// <summary>
/// Represents a container for a section of communication content.
/// </summary>
[Description("Represents a container for a section of communication content.")]
[JsonConverter(typeof(JsonPartConverter))]
[DataContract]
[KnownType(typeof(DataPart)), KnownType(typeof(FilePart)), KnownType(typeof(TextPart))]
public abstract record Part
{

    /// <summary>
    /// Gets a key/value mapping, if any, containing metadata associated with the part.
    /// </summary>
    [Description("A key/value mapping, if any, containing metadata associated with the part.")]
    [DataMember(Order = 99, Name = "metadata"), JsonPropertyOrder(99), JsonPropertyName("metadata")]
    public IReadOnlyDictionary<string, JsonNode>? Metadata { get; init; }

}
