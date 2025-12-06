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

using A2A.Server.Services;
using Microsoft.SemanticKernel;

namespace A2A.Samples.SemanticKernel.Server;

public class ChatAgent(Kernel kernel)
    : IA2AAgentRuntime
{

    public Task<Models.Response> ProcessAsync(Models.Message message, CancellationToken cancellationToken = default)
    {
        return Task.FromResult<Models.Response>(message);
    }

    public IAsyncEnumerable<Models.TaskEvent> ExecuteAsync(Models.Task task, CancellationToken cancellationToken = default)
    {
        return Enumerable.Empty<Models.TaskEvent>().ToAsyncEnumerable();
    }

}
