﻿namespace Neuroglia.A2A.Models;

/// <summary>
/// Represents a file
/// </summary>
[DataContract]
public record File
{

    /// <summary>
    /// Gets/sets the file's name, if any
    /// </summary>
    [DataMember(Name = "name", Order = 1), JsonPropertyName("name"), JsonPropertyOrder(1), YamlMember(Alias = "name", Order = 1)]
    public virtual string? Name { get; set; }

    /// <summary>
    /// Gets/sets the file's MIME type, if any
    /// </summary>
    [DataMember(Name = "mimeType", Order = 2), JsonPropertyName("mimeType"), JsonPropertyOrder(2), YamlMember(Alias = "mimeType", Order = 2)]
    public virtual string? MimeType { get; set; }

    /// <summary>
    /// Gets/sets the file's base64 encoded content, if any
    /// </summary>
    [DataMember(Name = "bytes", Order = 3), JsonPropertyName("bytes"), JsonPropertyOrder(3), YamlMember(Alias = "bytes", Order = 3)]
    public virtual string? Bytes { get; set; }

    /// <summary>
    /// Gets/sets the file's uri, if any
    /// </summary>
    [DataMember(Name = "uri", Order = 4), JsonPropertyName("uri"), JsonPropertyOrder(4), YamlMember(Alias = "uri", Order = 4)]
    public virtual Uri? Uri { get; set; }

}