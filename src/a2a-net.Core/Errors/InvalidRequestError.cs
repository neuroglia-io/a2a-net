﻿// Copyright � 2025-Present the a2a-net Authors
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
/// Represents an error that occurs when the request payload is invalid or fails validation
/// </summary>
[DataContract]
public record InvalidRequestError()
    : RpcError(ErrorCode, "Request payload validation error")
{

    /// <summary>
    /// Gets the error code associated with the <see cref="RpcError"/>
    /// </summary>
    public const int ErrorCode = -32600;

}
