// Copyright � 2025-Present the a2a-net Authors
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

using System.Text;

namespace A2A.Samples.SemanticKernel.Client;

/// <summary>
/// Defines extensions for <see cref="Message"/>s
/// </summary>
public static class MessageExtensions
{

    /// <summary>
    /// Converts the <see cref="Message"/> into text
    /// </summary>
    /// <param name="message">The <see cref="Message"/> to convert</param>
    /// <returns>The converted <see cref="Message"/></returns>
    public static string? ToText(this Message message)
    {
        ArgumentNullException.ThrowIfNull(message);
        if (message.Parts == null) return null;
        var textBuilder = new StringBuilder();
        foreach (var part in message.Parts) textBuilder.Append(part.ToText());
        return textBuilder.ToString();
    }

}