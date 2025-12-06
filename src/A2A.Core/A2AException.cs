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
/// Represents an A2A-specific exception.
/// </summary>
public sealed class A2AException
    : Exception
{

    /// <summary>
    /// Initializes a new instance of the <see cref="A2AException"/> class.
    /// </summary>
    /// <param name="code">The error code associated with the exception.</param>
    /// <param name="message">The error message, if any, associated with the exception.</param>
    public A2AException(int code, string? message = null)
        : base(message)
    {

    }

    /// <summary>
    /// Gets the error code associated with the exception.
    /// </summary>
    public int Code { get; }

}
