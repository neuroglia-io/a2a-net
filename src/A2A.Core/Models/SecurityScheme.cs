namespace A2A.Models;

/// <summary>
/// Represents a security scheme.
/// </summary>
[Description("Represents a security scheme.")]
[JsonPolymorphic(TypeDiscriminatorPropertyName = "type")]
[JsonDerivedType(typeof(ApiKeySecurityScheme), SecuritySchemeType.ApiKey)]
[JsonDerivedType(typeof(HttpSecurityScheme), SecuritySchemeType.Http)]
[JsonDerivedType(typeof(MutualTlsSecurityScheme), SecuritySchemeType.MutualTls)]
[JsonDerivedType(typeof(OAuth2SecurityScheme), SecuritySchemeType.OAuth2)]
[JsonDerivedType(typeof(OpenIdConnectSecurityScheme), SecuritySchemeType.OpenIdConnect)]
[DataContract]
[KnownType(typeof(ApiKeySecurityScheme)), KnownType(typeof(HttpSecurityScheme)), KnownType(typeof(MutualTlsSecurityScheme)), KnownType(typeof(OAuth2SecurityScheme)), KnownType(typeof(OpenIdConnectSecurityScheme))]
public abstract record SecurityScheme
{

    /// <summary>
    /// Gets the security scheme's type.
    /// </summary>
    [IgnoreDataMember, JsonIgnore]
    public abstract string Type { get; }

    /// <summary>
    /// Gets the security scheme's description, if any.
    /// </summary>
    [Description("The security scheme's description, if any.")]
    [DataMember(Order = 0, Name = "description"), JsonPropertyOrder(0), JsonPropertyName("description")]
    public string? Description { get; init; }

}
