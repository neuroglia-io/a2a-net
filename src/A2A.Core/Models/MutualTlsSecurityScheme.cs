namespace A2A.Models;

/// <summary>
/// Represents a mutual TLS security scheme.
/// </summary>
[Description("Represents a mutual TLS security scheme.")]
[DataContract]
public sealed record MutualTlsSecurityScheme
    : SecurityScheme
{

    /// <inheritdoc/>
    [IgnoreDataMember, JsonIgnore]
    public override string Type => SecuritySchemeType.MutualTls;

}