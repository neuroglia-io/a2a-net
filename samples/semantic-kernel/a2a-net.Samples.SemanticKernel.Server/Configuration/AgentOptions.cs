﻿// Copyright © 2025-Present the a2a-net Authors
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
/// Represents the options used to configure the application's agent
/// </summary>
public class AgentOptions
{

    /// <summary>
    /// Gets or sets the name of the application's AI agent
    /// </summary>
    [Required, MinLength(1)]
    public virtual string Name { get; set; } = null!;

    /// <summary>
    /// Gets or sets the description, if any, of the application's AI agent
    /// </summary>
    public virtual string? Description { get; set; }

    /// <summary>
    /// Gets or sets the version of the application's AI agent
    /// </summary>
    [Required, MinLength(1)]
    public virtual string Version { get; set; } = null!;

    /// <summary>
    /// Gets or sets the agent's instructions, if any
    /// </summary>
    public virtual string? Instructions { get; set; }

    /// <summary>
    /// Gets or sets a list containing the skills, if any, of the application's AI agent
    /// </summary>
    public virtual List<AgentSkill>? Skills { get; set; }

    /// <summary>
    /// Gets or sets the options used to configure the agent's <see cref="Microsoft.SemanticKernel.Kernel"/>
    /// </summary>
    [Required]
    public virtual AgentKernelOptions Kernel { get; set; } = null!;

}
