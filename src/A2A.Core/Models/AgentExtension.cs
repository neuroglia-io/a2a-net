namespace A2A.Models;

/// <summary>
/// Represents a protocol extension supported by an agent.
/// </summary>
[Description("Represents a protocol extension supported by an agent.")]
[DataContract]
public sealed record AgentExtension
{

    /// <summary>
    /// Gets the URI, if any, used to uniquely identify the extension.
    /// </summary>
    [Description("The URI, if any, used to uniquely identify the extension.")]
    [DataMember(Order = 1, Name = "uri"), JsonPropertyOrder(1), JsonPropertyName("uri")]
    public Uri? Uri { get; init; }

    /// <summary>
    /// Gets a human readable description of the extension, if any.
    /// </summary>
    [Description("A human readable description of the extension, if any.")]
    [DataMember(Order = 2, Name = "description"), JsonPropertyOrder(2), JsonPropertyName("description")]
    public string? Description { get; init; }

    /// <summary>
    /// Gets a boolean indicating whether the client must understand and comply with the extension's requirements.
    /// </summary>
    [Description("A boolean indicating whether the client must understand and comply with the extension's requirements.")]
    [DataMember(Order = 3, Name = "required"), JsonPropertyOrder(3), JsonPropertyName("required")]
    public bool? Required { get; init; }

    /// <summary>
    /// Gets optional, extension-specific configuration parameters.
    /// </summary>
    [Description("Optional, extension-specific configuration parameters.")]
    [DataMember(Order = 4, Name = "params"), JsonPropertyOrder(4), JsonPropertyName("params")]
    public IReadOnlyDictionary<string, JsonNode>? Params { get; init; }

}