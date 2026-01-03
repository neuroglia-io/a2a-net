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

namespace A2A.Server;

/// <summary>
/// Defines the fundamentals of the context in which an A2A agent invocation occurs.
/// </summary>
public interface IA2AAgentInvocationContext
{

    /// <summary>
    /// Gets the identifier of the tenant, if any, the agent is being invoked for.
    /// </summary>
    public string? Tenant { get; }

    /// <summary>
    /// Gets the metadata associated with the invocation, if any.
    /// </summary>
    public JsonObject? Metadata { get; }

}