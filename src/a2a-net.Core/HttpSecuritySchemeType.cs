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
/// Enumerates all supported HTTP security scheme types.
/// </summary>
public static class HttpSecuritySchemeType
{

    /// <summary>
    /// The HTTP Basic authentication scheme.
    /// </summary>
    public const string Basic = "basic";
    /// <summary>
    /// The HTTP Bearer authentication scheme.
    /// </summary>
    public const string Bearer = "bearer";

    /// <summary>
    /// Gets an <see cref="IEnumerable{T}"/> containing all supported values.
    /// </summary>
    /// <returns>A new <see cref="IEnumerable{T}"/> containing all supported values.</returns>
    public static IEnumerable<string> AsEnumerable()
    {
        yield return Basic;
        yield return Bearer;
    }

}