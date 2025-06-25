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
/// Defines extensions for <see cref="IAsyncEnumerable{T}"/> instances
/// </summary>
public static class IAsyncEnumerableExtensions
{

    /// <summary>
    /// Enumerates the elements of an <see cref="IAsyncEnumerable{T}"/> while providing a lookahead to the next item.
    /// Useful for scenarios where you need to detect the final iteration or inspect the upcoming element.
    /// </summary>
    /// <typeparam name="T">The type of the elements in the source sequence.</typeparam>
    /// <param name="source">The source asynchronous enumerable to iterate.</param>
    /// <param name="cancellationToken">A token to cancel the enumeration operation.</param>
    /// <returns>
    /// An <see cref="IAsyncEnumerable{T}"/> of tuples, where each tuple contains the current item and the next item (or <c>default</c> if at the end of the sequence).
    /// </returns>
    public static async IAsyncEnumerable<(T current, T? next)> PeekingAsync<T>(this IAsyncEnumerable<T> source, [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        await using var enumerator = source.GetAsyncEnumerator(cancellationToken);
        if (!await enumerator.MoveNextAsync()) yield break;
        var current = enumerator.Current;
        while (await enumerator.MoveNextAsync())
        {
            yield return (current, enumerator.Current);
            current = enumerator.Current;
        }
        yield return (current, default);
    }

}
