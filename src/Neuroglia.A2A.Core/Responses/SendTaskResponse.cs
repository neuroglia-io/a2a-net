namespace Neuroglia.A2A.Responses;

/// <summary>
/// Represents the response to a send task request
/// </summary>
[DataContract]
public record SendTaskResponse
    : Response<Models.Task>
{



}