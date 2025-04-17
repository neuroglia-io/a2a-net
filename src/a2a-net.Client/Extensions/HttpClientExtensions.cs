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

namespace A2A.Client;

/// <summary>
/// Defines extensions for <see cref="HttpClient"/>s
/// </summary>
public static class HttpClientExtensions
{

    /// <summary>
    /// Retrieves the A2A discovery document from a remote agent
    /// </summary>
    /// <param name="httpClient">The <see cref="HttpClient"/> to use to perform the request</param>
    /// <param name="request">The discovery request containing the base URI and optional configuration</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/></param>
    /// <returns>The retrieved <see cref="A2ADiscoveryDocument"/></returns>
    public static async Task<A2ADiscoveryDocument> GetA2ADiscoveryDocumentAsync(this HttpClient httpClient, A2ADiscoveryDocumentRequest request, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(httpClient);
        ArgumentNullException.ThrowIfNull(request);
        try
        {
            var path = "/.well-known/agent.json";
            var endpoint = request.Address == null ? new Uri(path, UriKind.Relative) : new Uri(request.Address, path);
            var agentCard = await httpClient.GetFromJsonAsync<AgentCard>(endpoint, cancellationToken).ConfigureAwait(false);
            return new()
            {
                Endpoint = endpoint.IsAbsoluteUri ? endpoint : new(httpClient.BaseAddress!, path),
                Agents = agentCard == null ? [] : [agentCard]
            };
        }
        catch (HttpRequestException ex) when(ex.StatusCode == HttpStatusCode.NotFound)
        {
            var path = "/.well-known/agents.json";
            var endpoint = request.Address == null ? new Uri(path, UriKind.Relative) : new Uri(request.Address, path);
            var agentCards = await httpClient.GetFromJsonAsync<List<AgentCard>>(endpoint, cancellationToken).ConfigureAwait(false);
            return new()
            {
                Endpoint = endpoint.IsAbsoluteUri ? endpoint : new(httpClient.BaseAddress!, path),
                Agents = agentCards ?? []
            };
        }
    }

    /// <summary>
    /// Retrieves the A2A discovery document from a remote agent
    /// </summary>
    /// <param name="httpClient">The <see cref="HttpClient"/> to use to perform the request</param>
    /// <param name="address">The base URI of the remote server to query for discovery metadata</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/></param>
    /// <returns>The retrieved <see cref="A2ADiscoveryDocument"/></returns>
    public static Task<A2ADiscoveryDocument> GetA2ADiscoveryDocumentAsync(this HttpClient httpClient, Uri address, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(httpClient);
        ArgumentNullException.ThrowIfNull(address);
        var request = new A2ADiscoveryDocumentRequest()
        {
            Address = address
        };
        return httpClient.GetA2ADiscoveryDocumentAsync(request, cancellationToken);
    }

    /// <summary>
    /// Retrieves the A2A discovery document from a remote agent
    /// </summary>
    /// <param name="httpClient">The <see cref="HttpClient"/> to use to perform the request</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/></param>
    /// <returns>The retrieved <see cref="A2ADiscoveryDocument"/></returns>
    public static Task<A2ADiscoveryDocument> GetA2ADiscoveryDocumentAsync(this HttpClient httpClient, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(httpClient);
        var request = new A2ADiscoveryDocumentRequest(); ;
        return httpClient.GetA2ADiscoveryDocumentAsync(request, cancellationToken);
    }

}
