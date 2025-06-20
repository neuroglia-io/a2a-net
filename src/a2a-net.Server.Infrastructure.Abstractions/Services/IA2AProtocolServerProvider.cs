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

namespace A2A.Server.Infrastructure.Services;

/// <summary>
/// Defines the fundamentals of a service used to provide <see cref="IA2AProtocolServer"/>s
/// </summary>
public interface IA2AProtocolServerProvider
{

    /// <summary>
    /// Gets the <see cref="IA2AProtocolServer"/> with the specified name
    /// </summary>
    /// <param name="name">The name of the <see cref="IA2AProtocolServer"/> to get</param>
    /// <returns>The <see cref="IA2AProtocolServer"/> with the specified name</returns>
    IA2AProtocolServer? Get(string name);

}