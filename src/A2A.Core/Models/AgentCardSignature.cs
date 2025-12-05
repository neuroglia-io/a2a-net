namespace A2A.Models;

/// <summary>
/// Represents an agent's signature.
/// </summary>
[Description("Represents an agent's signature.")]
[DataContract]
public sealed record AgentCardSignature
{

    /// <summary>
    /// Gets the protected JWS header for the signature. It always is a base64url-encoded JSON object.
    /// </summary>
    [Description("The protected JWS header for the signature. It always is a base64url-encoded JSON object.")]
    [Required, MinLength(1)]
    [DataMember(Order = 1, Name = "protected"), JsonPropertyOrder(1), JsonPropertyName("protected")]
    public required string Protected { get; init; }

    /// <summary>
    /// Gets the computed signature value. It always is a base64url-encoded string.
    /// </summary>
    [Description("The computed signature value. It always is a base64url-encoded string.")]
    [Required, MinLength(1)]
    [DataMember(Order = 2, Name = "signature"), JsonPropertyOrder(2), JsonPropertyName("signature")]
    public required string Signature { get; init; }

    /// <summary>
    /// Gets a key/value mapping, if any, containing the unprotected JWS header values.
    /// </summary>
    [Description("A key/value mapping, if any, containing the unprotected JWS header values.")]
    [DataMember(Order = 3, Name = "header"), JsonPropertyOrder(3), JsonPropertyName("header")]
    public IReadOnlyDictionary<string, JsonNode>? Header { get; set; }

}
