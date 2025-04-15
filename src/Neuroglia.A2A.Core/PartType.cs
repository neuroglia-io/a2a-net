// Copyright � 2025-Present Neuroglia SRL
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

namespace Neuroglia.A2A;

/// <summary>
/// Enumerates all types of parts
/// </summary>
public static class PartType
{

    /// <summary>
    /// Indicates a data part
    /// </summary>
    public const string Data = "data";
    /// <summary>
    /// Indicates a file part
    /// </summary>
    public const string File = "file";
    /// <summary>
    /// Indicates a text part
    /// </summary>
    public const string Text = "text";
  
    /// <summary>
    /// Gets a new <see cref="IEnumerable{T}"/> containing all supported values
    /// </summary>
    /// <returns>An <see cref="IEnumerable{T}"/> containing all supported values</returns>
    public static IEnumerable<string> AsEnumerable()
    {
        yield return Data;
        yield return File;
        yield return Text;
    }

}