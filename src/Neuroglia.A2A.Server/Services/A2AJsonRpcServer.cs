using Neuroglia.A2A.Requests;

namespace Neuroglia.A2A.Server.Services;

/// <summary>
/// Represents the JSON RPC server for the A2A protocol
/// </summary>
public class A2AJsonRpcServer
{

    [JsonRpcMethod("tasks/send")]
    public virtual async Task<SendTaskResponse> SendTaskAsync(SendTaskRequest request, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);
        return new();
    }

}
