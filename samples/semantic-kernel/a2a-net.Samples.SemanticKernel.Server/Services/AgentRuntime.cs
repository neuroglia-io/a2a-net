// Copyright © 2025-Present the a2a-net Authors
//
// Licensed under the Apache License, Version 2.0 (the "License"),
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

namespace A2A.Samples.SemanticKernel.Server.Services;

/// <summary>
/// Represents a <see cref="Microsoft.SemanticKernel.Kernel"/> based implementation of the <see cref="IAgentRuntime"/> interface
/// </summary>
public class AgentRuntime
    : IAgentRuntime
{

    /// <summary>
    /// Initializes a new <see cref="AgentRuntime"/>
    /// </summary>
    /// <param name="options">The service used to access the current <see cref="ApplicationOptions"/></param>
    public AgentRuntime(IOptions<ApplicationOptions> options)
    {
        Options = options.Value;
        var kernelBuilder = Kernel.CreateBuilder();
        kernelBuilder.AddOpenAIChatCompletion(Options.Agent.Kernel.Model, Options.Agent.Kernel.ApiKey);
        Kernel = kernelBuilder.Build();
    }

    /// <summary>
    /// Gets the current <see cref="ApplicationOptions"/>
    /// </summary>
    protected ApplicationOptions Options { get; set; }

    /// <summary>
    /// Gets the <see cref="Microsoft.SemanticKernel.Kernel"/> to use
    /// </summary>
    protected Kernel Kernel { get; }

    /// <summary>
    /// Gets a <see cref="ConcurrentDictionary{TKey, TValue}"/> that contains an <see cref="ChatHistory"/> per id mapping of all active A2A sessions
    /// </summary>
    protected ConcurrentDictionary<string, ChatHistory> Sessions { get; } = [];

    /// <summary>
    /// Gets a <see cref="ConcurrentDictionary{TKey, TValue}"/> that contains an <see cref="CancellationTokenSource"/> per id mapping of all active A2A tasks
    /// </summary>
    protected ConcurrentDictionary<string, CancellationTokenSource> Tasks { get; } = [];

    /// <summary>
    /// Gets the service used to perform chat completion
    /// </summary>
    protected IChatCompletionService ChatCompletionService => Kernel.GetRequiredService<IChatCompletionService>();

    /// <inheritdoc/>
    public virtual async IAsyncEnumerable<AgentResponseContent> ExecuteAsync(TaskRecord task, [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(task);
        var cancellationTokenSource = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
        Tasks.AddOrUpdate(task.Id, cancellationTokenSource, (id, existing) =>
        {
            if (!existing.IsCancellationRequested)
            {
                existing.Cancel();
                try
                {
                    existing.Dispose();
                }
                catch (ObjectDisposedException) { }
            }
            return cancellationTokenSource;
        });
        if (!Sessions.TryGetValue(task.ContextId, out var session) || session == null)
        {
            session = string.IsNullOrWhiteSpace(Options.Agent.Instructions) ? [] : new ChatHistory(Options.Agent.Instructions);
            Sessions.AddOrUpdate(task.ContextId, session, (id, existing) => existing);
        }
        session.AddUserMessage(task.Message.ToText() ?? string.Empty);
        var executionSettings = new PromptExecutionSettings();
        string? currentRole = null;
        var currentContent = new StringBuilder();
        EquatableDictionary<string, object>? metadata = null;
        uint index = 0;
        await foreach (var (content, next) in ChatCompletionService.GetStreamingChatMessageContentsAsync(session, executionSettings, Kernel, cancellationTokenSource.Token).PeekingAsync(cancellationTokenSource.Token))
        {
            var role = content.Role?.ToString();
            if (role != null && role != currentRole)
            {
                if (currentContent.Length > 0 && currentRole != null)
                {
                    yield return new AgentResponseContent(new Artifact
                    {
                        Index = index++,
                        Metadata = metadata,
                        Parts = [new TextPart(currentContent.ToString())],
                        LastChunk = true
                    });
                    currentContent.Clear();
                    metadata = null;
                }
                currentRole = role;
            }
            if (!string.IsNullOrEmpty(content.Content)) currentContent.Append(content.Content);
            if (content.Metadata is { Count: > 0 })
            {
                metadata ??= [];
                foreach (var kvp in content.Metadata) metadata[kvp.Key] = kvp.Value!;
            }
            if (next == null || (!string.IsNullOrWhiteSpace(next?.Role?.ToString()) && next?.Role?.ToString() != currentRole && currentContent.Length > 0 && currentRole != null))
            {
                yield return new AgentResponseContent(new Artifact
                {
                    Index = index++,
                    Metadata = metadata,
                    Parts = [new TextPart(currentContent.ToString())],
                    LastChunk = true
                });
                currentContent.Clear();
                metadata = null;
            }
        }
    }

    /// <inheritdoc/>
    public virtual System.Threading.Tasks.Task CancelAsync(string taskId, CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(taskId);
        if (Tasks.TryRemove(taskId, out var cancellationTokenSource) && cancellationTokenSource != null) return cancellationTokenSource.CancelAsync(); 
        else return System.Threading.Tasks.Task.CompletedTask;
    }

}