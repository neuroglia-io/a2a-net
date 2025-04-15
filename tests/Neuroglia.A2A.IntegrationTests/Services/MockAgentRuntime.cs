using Neuroglia.A2A.Server.Infrastructure;
using Neuroglia.A2A.Server.Infrastructure.Services;
using System.Runtime.CompilerServices;

namespace Neuroglia.A2A.IntegrationTests.Services;

internal class MockAgentRuntime
    : IAgentRuntime
{

    public async IAsyncEnumerable<AgentResponseContent> ExecuteAsync(Models.Task task,  [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        await Task.CompletedTask;
        yield break;
    }

    public Task CancelAsync(Models.Task task, CancellationToken cancellationToken = default) => Task.CompletedTask;

}
