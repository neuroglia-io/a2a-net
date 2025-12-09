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
/// Enumerates all supported API key locations.
/// </summary>
public static class ApiKeyLocation
{

    /// <summary>
    /// The API key is passed in a cookie.
    /// </summary>
    public const string Cookie = "cookie";
    /// <summary>
    /// The API key is passed in the request header.
    /// </summary>
    public const string Header = "header";
    /// <summary>
    /// The API key is passed in the query string.
    /// </summary>
    public const string Query = "query";

    /// <summary>
    /// Gets all possible API key locations.
    /// </summary>
    public static readonly IEnumerable<string> All =
    [
        Cookie,
        Header,
        Query
    ];

}
