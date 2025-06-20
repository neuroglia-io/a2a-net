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
/// Represents an error indicating that the specified task cannot be canceled.
/// </summary>
[Description("Represents an error indicating that the specified task cannot be canceled.")]
[DataContract]
public record TaskNotCancellableError()
    : RpcError(ErrorCode, "Task cannot be canceled")
{

    /// <summary>
    /// Gets the error code associated with the <see cref="RpcError"/>.
    /// </summary>
    public const int ErrorCode = -32002;

}
