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

using System.Diagnostics;

namespace A2A.Samples.SemanticKernel.Server;

public class ChatAgent(Kernel kernel)
    : IA2AAgentRuntime
{ 

    public Task<Models.Response> ProcessAsync(Models.Message message, IA2AAgentInvocationContext context, CancellationToken cancellationToken = default)
    {
        var task = new Models.Task()
        {
            ContextId = message.ContextId ?? Guid.NewGuid().ToString("N"),
            History = [message]
        };
        return Task.FromResult<Models.Response>(task);
    }

    public async IAsyncEnumerable<Models.TaskEvent> ExecuteAsync(Models.Task task, [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        var chat = kernel.GetRequiredService<IChatClient>();
        var message = task.History?.LastOrDefault() ?? throw new NullReferenceException($"The history of the task with the specified id '{task.Id}' is null or empty.");
        var messageText = string.Join('\n', message.Parts.OfType<Models.TextPart>().Select(p => p.Text));
        var artifactId = Guid.NewGuid().ToString("N");
        var isFirstChunk = true;
        yield return new Models.TaskStatusUpdateEvent()
        {
            ContextId = task.ContextId,
            TaskId = task.Id,
            Status = new()
            {
                State = TaskState.Working,
                Message = new()
                {
                    ContextId = task.ContextId,
                    TaskId = task.Id,
                    Role = Role.Agent,
                    Parts =
                    [
                        new Models.TextPart()
                        {
                            Text = "Processing started by Semantic Kernel Chat Agent."
                        }
                    ]
                }
            }
        };
        var stopwatch = Stopwatch.StartNew();
        await foreach (var content in chat.GetStreamingResponseAsync(messageText, new(), cancellationToken))
        {
            yield return new Models.TaskArtifactUpdateEvent()
            {
                ContextId = task.ContextId,
                TaskId = task.Id,
                Artifact = new() 
                { 
                    ArtifactId = artifactId,
                    Parts = 
                    [            
                        new Models.TextPart() 
                        { 
                            Text = content.Text
                        }
                    ]
                },
                Append = !isFirstChunk
            };
            isFirstChunk = false;
        }
        stopwatch.Stop();
        yield return new Models.TaskStatusUpdateEvent()
        {
            ContextId = task.ContextId,
            TaskId = task.Id,
            Status = new()
            {
                State = TaskState.Completed,
                Message = new()
                {
                    ContextId = task.ContextId,
                    TaskId = task.Id,
                    Role = Role.Agent,
                    Parts =
                    [
                        new Models.TextPart()
                        {
                            Text = $"Processing completed in {stopwatch.ElapsedMilliseconds}ms."
                        }
                    ]
                }
            },
            Final = true
        };
    }

}
