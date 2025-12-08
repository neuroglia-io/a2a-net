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
/// Defines the fundamentals of an A2A server transport.
/// </summary>
public interface IA2AServerTransport
{

    /// <summary>
    /// Handles an incoming A2A request.
    /// </summary>
    /// <param name="httpContext">The current <see cref="HttpContext"/>.</param>
    /// <returns>A new <see cref="IResult"/> representing the outcome of the operation.</returns>
    Task<IResult> HandleAsync(HttpContext httpContext);

}