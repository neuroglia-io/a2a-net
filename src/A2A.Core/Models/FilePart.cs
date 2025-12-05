namespace A2A.Models;

/// <summary>
/// Represents a part used to transport a file attachment.
/// </summary>
[Description("Represents a part used to transport a file attachment.")]
[DataContract]
public sealed record FilePart
    : Part
{

    /// <summary>
    /// Gets the file's name, if any.
    /// </summary>
    [Description("The file's name, if any.")]
    [DataMember(Order = 1, Name = "name"), JsonPropertyOrder(1), JsonPropertyName("name")]
    public string? Name { get; init; }

    /// <summary>
    /// Gets the file's media type, if any.
    /// </summary>
    [Description("The file's media type, if any.")]
    [DataMember(Order = 2, Name = "mediaType"), JsonPropertyOrder(2), JsonPropertyName("mediaType")]
    public string? MediaType { get; init; }

    /// <summary>
    /// Gets the file's URI, if any. Required if 'bytes' is not set.
    /// </summary>
    [Description("The file's URI, if any. Required if 'bytes' is not set.")]
    [DataMember(Order = 3, Name = "fileWithUri"), JsonPropertyOrder(3), JsonPropertyName("fileWithUri")]
    public Uri? Uri { get; init; }

    /// <summary>
    /// Gets the file's bytes, if any. Required if 'uri' is not set.
    /// </summary>
    [Description("The file's bytes, if any. Required if 'uri' is not set.")]
    [DataMember(Order = 4, Name = "fileWithBytes"), JsonPropertyOrder(4), JsonPropertyName("fileWithBytes")]
    public ReadOnlyMemory<byte>? Bytes { get; init; }

}
