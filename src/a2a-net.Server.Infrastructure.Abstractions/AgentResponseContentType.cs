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

namespace A2A.Server.Infrastructure;

/// <summary>
/// Enumerates all supported types of contents returned by an agent as part of its response to a task's execution
/// </summary>
public static class AgentResponseContentType
{

    /// <summary>
    /// Indicates an artifact content
    /// </summary>
    public const string Artifact = "artifact";
    /// <summary>
    /// Indicates a message content
    /// </summary>
    public const string Message = "message";

    /// <summary>
    /// Gets a new <see cref="IEnumerable{T}"/> containing all supported values
    /// </summary>
    /// <returns>An <see cref="IEnumerable{T}"/> containing all supported values</returns>
    public static IEnumerable<string> AsEnumerable()
    {
        yield return Artifact;
        yield return Message;
    }

}