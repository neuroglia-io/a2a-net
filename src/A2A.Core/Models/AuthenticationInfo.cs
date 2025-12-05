namespace A2A.Models;

/// <summary>
/// Represents information about push notification authentication.
/// </summary>
[Description("Represents information about push notification authentication.")]
[DataContract]
public sealed record AuthenticationInfo
{

    /// <summary>
    /// Gets a collection of schemes used for authentication.
    /// </summary>
    [Description("A collection of schemes used for authentication.")]
    [Required, MinLength(1)]
    [DataMember(Order = 1, Name = "schemes"), JsonPropertyOrder(1), JsonPropertyName("schemes")]
    public required IReadOnlyCollection<string> Schemes { get; init; }

    /// <summary>
    /// Gets credentials used for authentication, if any.
    /// </summary>
    [Description("Credentials used for authentication, if any.")]
    [DataMember(Order = 2, Name = "credentials"), JsonPropertyOrder(2), JsonPropertyName("credentials")]
    public string? Credentials { get; init; }

}