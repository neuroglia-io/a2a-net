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
/// Exposes defaults about the A2A Server.
/// </summary>
public static class A2AServerDefaults
{

    /// <summary>
    /// Gets the service key for the extended agent card.
    /// </summary>
    public const string ExtendedAgentCardServiceKey = "Extended";

    /// <summary>
    /// Exposes environment variable used by the A2A Server.
    /// </summary>
    public static class EnvironmentVariables     
    {

        /// <summary>
        /// The environment variable key used to store the A2A server address.
        /// </summary>
        public const string ServerAddress = "A2A_SERVER_ADDRESS";

    }

}
