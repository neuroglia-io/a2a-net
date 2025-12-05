namespace A2A.Models;

/// <summary>
/// Represents an OAuth2 security scheme.
/// </summary>
[Description("An OAuth2 security scheme.")]
[DataContract]
public sealed record OAuth2SecurityScheme
    : SecurityScheme
{

    /// <inheritdoc/>
    [IgnoreDataMember, JsonIgnore]
    public override string Type => SecuritySchemeType.OAuth2;

    /// <summary>
    /// Gets or sets the OAuth2 flow definitions for this security scheme.
    /// </summary>
    [Description("The OAuth2 flow definitions for this security scheme.")]
    [Required]
    [DataMember(Order = 1, Name = "flows"), JsonPropertyOrder(1), JsonPropertyName("flows")]
    public required OAuthFlows Flows { get; set; } = default!;

}
