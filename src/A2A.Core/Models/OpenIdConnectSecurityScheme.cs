namespace A2A.Models;

/// <summary>
/// Represents an OpenID Connect security scheme as defined by the OpenAPI Specification.
/// </summary>
[Description("An OpenID Connect security scheme as defined by the OpenAPI Specification.")]
[DataContract]
public sealed record OpenIdConnectSecurityScheme
    : SecurityScheme
{

    /// <inheritdoc/>
    [IgnoreDataMember, JsonIgnore]
    public override string Type => SecuritySchemeType.OpenIdConnect;

    /// <summary>
    /// Gets or sets the OpenID Connect URL to discover OAuth2 configuration values.
    /// </summary>
    [Description("The OpenID Connect URL to discover OAuth2 configuration values.")]
    [Required, MinLength(1)]
    [DataMember(Order = 1, Name = "openIdConnectUrl"), JsonPropertyOrder(1), JsonPropertyName("openIdConnectUrl")]
    public required Uri OpenIdConnectUrl { get; init; }

}