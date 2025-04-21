// Copyright � 2025-Present the a2a-net Authors
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

namespace A2A.Samples.SemanticKernel.Server.Configuration;

/// <summary>
/// Represents the options used to configure the agent's <see cref="Kernel"/>
/// </summary>
public class AgentKernelOptions
{

    /// <summary>
    /// Gets/sets the id of the model used by the application's <see cref="Kernel"/>
    /// </summary>
    [Required, MinLength(1)]
    public virtual string Model { get; set; } = "gpt-4o";

    /// <summary>
    /// Gets/sets the API key used to authenticate on the chat completion API used by the application's <see cref="Kernel"/>
    /// </summary>
    [Required, MinLength(1)]
    public virtual string ApiKey { get; set; } = null!;

}
