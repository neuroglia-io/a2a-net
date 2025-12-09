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

using A2A.Models;

namespace A2A.Client;

/// <summary>
/// Represents the result of an A2A agent discovery operation
/// </summary>
public sealed class A2ADiscoveryDocument
{

    /// <summary>
    /// Gets the endpoint from which the discovery document was retrieved
    /// </summary>
    public required Uri Endpoint { get; init; }

    /// <summary>
    /// Gets a list contained the discovered <see cref="AgentCard"/> returned by the remote agent
    /// </summary>
    public required AgentCard Agent { get; init; }

}