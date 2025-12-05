namespace A2A.Models;

/// <summary>
/// Represents an API key security scheme.
/// </summary>
[Description("Represents an API key security scheme.")]
[DataContract]
public sealed record ApiKeySecurityScheme
    : SecurityScheme
{

    /// <inheritdoc />
    [IgnoreDataMember, JsonIgnore]
    public override string Type => SecuritySchemeType.ApiKey;

    /// <summary>
    /// Gets the name of the header or query parameter to be used for the API key.
    /// </summary>
    [Description("The name of the header or query parameter to be used for the API key.")]
    [Required, MinLength(1)]
    [DataMember(Order = 1, Name = "name"), JsonPropertyOrder(1), JsonPropertyName("name")]
    public required string Name { get; init; }

    /// <summary>
    /// Gets the location of the API key.
    /// </summary>
    [Description("The location of the API key.")]
    [Required, AllowedValues(ApiKeyLocation.Cookie, ApiKeyLocation.Header, ApiKeyLocation.Query)]
    [DataMember(Order = 2, Name = "in"), JsonPropertyOrder(2), JsonPropertyName("in")]
    public required string In { get; set; }

}
