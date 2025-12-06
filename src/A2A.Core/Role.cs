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

namespace A2A;

/// <summary>
/// Enumerates the possible roles for a message sender.
/// </summary>
public static class Role
{

    /// <summary>
    /// Indicates the message was sent by an agent. In other words, the message has been sent by the server to the client.
    /// </summary>
    public const string Agent = "ROLE_AGENT";
    /// <summary>
    /// Indicates the message role is unspecified.
    /// </summary>
    public const string Unspecified = "ROLE_UNSPECIFIED";
    /// <summary>
    /// Indicates the message was sent by a user. In other words, the message has been sent by the client to the server.
    /// </summary>
    public const string User = "ROLE_USER";

    /// <summary>
    /// Gets all possible roles.
    /// </summary>
    public static readonly IEnumerable<string> All =
    [
        Agent,
        Unspecified,
        User
    ];

}
