namespace A2A.Models;

/// <summary>
/// Represents an HTTP authentication security scheme.
/// </summary>
[Description("Represents an HTTP authentication security scheme.")]
[DataContract]
public sealed record HttpSecurityScheme
    : SecurityScheme
{

    /// <inheritdoc />
    [IgnoreDataMember, JsonIgnore]
    public override string Type => SecuritySchemeType.Http;

    /// <summary>
    /// Gets or sets the HTTP authentication scheme.
    /// </summary>
    [Description("The HTTP authentication scheme.")]
    [Required, MinLength(1), AllowedValues(HttpSecuritySchemeType.Basic, HttpSecuritySchemeType.Bearer)]
    [DataMember(Order = 1, Name = "scheme"), JsonPropertyOrder(1), JsonPropertyName("scheme")]
    public required string Scheme { get; init; }

    /// <summary>
    /// Gets or sets a hint to the client to identify how the bearer token is formatted. Applies only when scheme is 'bearer'.
    /// </summary>
    [Description("A hint to the client to identify how the bearer token is formatted. Applies only when scheme is 'bearer'.")]
    [DataMember(Order = 2, Name = "bearerFormat"), JsonPropertyOrder(2), JsonPropertyName("bearerFormat")]
    public string? BearerFormat { get; init; }

}