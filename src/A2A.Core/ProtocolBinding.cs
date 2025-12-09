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
/// Enumerates the supported protocol bindings.
/// </summary>
public static class ProtocolBinding
{

    /// <summary>
    /// Indicates the GRPC protocol binding.
    /// </summary>
    public const string Grpc = "GRPC";
    /// <summary>
    /// Indicates the HTTP+JSON protocol binding.
    /// </summary>
    public const string Http = "HTTP+JSON";
    /// <summary>
    /// Indicates the JSON-RPC protocol binding.
    /// </summary>
    public const string JsonRpc = "JSONRPC";

    /// <summary>
    /// Gets all possible protocol bindings.
    /// </summary>
    public static readonly IEnumerable<string> All =
    [
        Grpc,
        Http,
        JsonRpc
    ];

}