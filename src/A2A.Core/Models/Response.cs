using A2A.Serialization.Json;

namespace A2A.Models;

/// <summary>
/// Represents a response from an A2A service.
/// </summary>
[Description("Represents a response from an A2A service.")]
[JsonConverter(typeof(JsonResponseConverter))]
[DataContract]
[KnownType(typeof(Message)), KnownType(typeof(Task))]
public abstract record Response
{



}
