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

namespace A2A.Server.Transports;

/// <summary>
/// Enumerates all supported JSON-RPC error codes.
/// </summary>
public static class JsonRpcErrorCode
{

    /// <summary>
    /// Indicates an invalid request.
    /// </summary>
    public const int InvalidRequest = -32600;
    /// <summary>
    /// Indicates that the method does not exist.
    /// </summary>
    public const int MethodNotFound = -32601;
    /// <summary>
    /// Indicates that the invalid parameters were provided.
    /// </summary>
    public const int InvalidParams = -32602;
    /// <summary>
    /// Indicates an internal error.
    /// </summary>
    public const int InternalError = -32603;
    /// <summary>
    /// Indicates a parse error.
    /// </summary>
    public const int ParseError = -32700;

}