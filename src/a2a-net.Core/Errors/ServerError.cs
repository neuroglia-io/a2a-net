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

namespace A2A.Errors;

/// <summary>
/// Represents an implementation specific error that occurs when the server encounters an error that is not covered by the standard JSON-RPC error codes.
/// </summary>
[Description("Represents an implementation specific error that occurs when the server encounters an error that is not covered by the standard JSON-RPC error codes.")]
[DataContract]
public record ServerError
    : RpcError
{

    /// <summary>
    /// Initializes a new <see cref="ServerError"/>.
    /// </summary>
    /// <param name="errorCode">The error code.</param>
    /// <param name="message">The error message.</param>
    /// <param name="data">The data, if any, associated to the error.</param>
    public ServerError(int errorCode, string message, object? data = null)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(errorCode , - 32099);
        ArgumentOutOfRangeException.ThrowIfGreaterThan(errorCode, -32000);
        ArgumentException.ThrowIfNullOrWhiteSpace(message);
        Code = errorCode;
        Message = message;
        Data = data;
    }

}