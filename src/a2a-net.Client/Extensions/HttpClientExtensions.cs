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

namespace A2A.Client;

/// <summary>
/// Defines extensions for <see cref="HttpClient"/>s
/// </summary>
public static class HttpClientExtensions
{

    /// <summary>
    /// Sends a GET request to the specified URL and returns the HTTP response message
    /// </summary>
    /// <param name="client">The HttpClient instance</param>
    /// <param name="url">The request URL</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/></param>
    /// <returns>The HTTP response message</returns>
    public static HttpResponseMessage Get(this HttpClient client, Uri url, CancellationToken cancellationToken = default)
    {
        var request = new HttpRequestMessage(HttpMethod.Get, url);
        return client.Send(request, cancellationToken);
    }

    /// <summary>
    /// Sends a GET request to the specified URL, expecting a JSON response, and deserializes it into the specified type
    /// </summary>
    /// <typeparam name="T">The type to deserialize the JSON response to</typeparam>
    /// <param name="client">The HttpClient instance</param>
    /// <param name="url">The request URL</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/></param>
    /// <returns>The deserialized response object</returns>
    public static T? GetFromJson<T>(this HttpClient client, Uri url, CancellationToken cancellationToken = default)
    {
        var request = new HttpRequestMessage(HttpMethod.Get, url);
        request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue(MediaTypeNames.Application.Json));
        var response = client.Send(request, cancellationToken);
        if (response.IsSuccessStatusCode)
        {
            using var reader = new StreamReader(response.Content.ReadAsStream(cancellationToken), Encoding.UTF8, leaveOpen: true);
            return JsonSerializer.Deserialize<T>(reader.ReadToEnd());
        }
        else throw new HttpRequestException($"Request failed with status code {response.StatusCode}");
    }

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
            var endpoint = MakeSingleAgentDiscoveryEndpointUri(httpClient, request.Address);
            var agentCard = await httpClient.GetFromJsonAsync<AgentCard>(endpoint, cancellationToken).ConfigureAwait(false);
            return new()
            {
                Endpoint = endpoint.IsAbsoluteUri ? endpoint : new(httpClient.BaseAddress!, endpoint.PathAndQuery),
                Agents = agentCard == null ? [] : [agentCard]
            };
        }
        catch (HttpRequestException ex) when (ex.StatusCode == HttpStatusCode.NotFound)
        {
            var endpoint = MakeRegistryDiscoveryEndpointUri(httpClient, request.Address);
            var agentCards = await httpClient.GetFromJsonAsync<List<AgentCard>>(endpoint, cancellationToken).ConfigureAwait(false);
            return new()
            {
                Endpoint = endpoint.IsAbsoluteUri ? endpoint : new(httpClient.BaseAddress!, endpoint.PathAndQuery),
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

    /// <summary>
    /// Retrieves the A2A discovery document from a remote agent
    /// </summary>
    /// <param name="httpClient">The <see cref="HttpClient"/> to use to perform the request</param>
    /// <param name="request">The discovery request containing the base URI and optional configuration</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/></param>
    /// <returns>The retrieved <see cref="A2ADiscoveryDocument"/></returns>
    public static A2ADiscoveryDocument GetA2ADiscoveryDocument(this HttpClient httpClient, A2ADiscoveryDocumentRequest request, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(httpClient);
        ArgumentNullException.ThrowIfNull(request);
        try
        {
            var endpoint = MakeSingleAgentDiscoveryEndpointUri(httpClient, request.Address);
            var agentCard = httpClient.GetFromJson<AgentCard>(endpoint, cancellationToken);
            return new()
            {
                Endpoint = endpoint.IsAbsoluteUri ? endpoint : new(httpClient.BaseAddress!, endpoint.PathAndQuery),
                Agents = agentCard == null ? [] : [agentCard]
            };
        }
        catch (HttpRequestException ex) when (ex.StatusCode == HttpStatusCode.NotFound)
        {
            var endpoint = MakeRegistryDiscoveryEndpointUri(httpClient, request.Address);
            var agentCards = httpClient.GetFromJson<List<AgentCard>>(endpoint, cancellationToken);
            return new()
            {
                Endpoint = endpoint.IsAbsoluteUri ? endpoint : new(httpClient.BaseAddress!, endpoint.PathAndQuery),
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
    public static A2ADiscoveryDocument GetA2ADiscoveryDocument(this HttpClient httpClient, Uri address, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(httpClient);
        ArgumentNullException.ThrowIfNull(address);
        var request = new A2ADiscoveryDocumentRequest()
        {
            Address = address
        };
        return httpClient.GetA2ADiscoveryDocument(request, cancellationToken);
    }

    /// <summary>
    /// Retrieves the A2A discovery document from a remote agent
    /// </summary>
    /// <param name="httpClient">The <see cref="HttpClient"/> to use to perform the request</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/></param>
    /// <returns>The retrieved <see cref="A2ADiscoveryDocument"/></returns>
    public static A2ADiscoveryDocument GetA2ADiscoveryDocument(this HttpClient httpClient, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(httpClient);
        var request = new A2ADiscoveryDocumentRequest();
        return httpClient.GetA2ADiscoveryDocument(request, cancellationToken);
    }

    private static Uri MakeSingleAgentDiscoveryEndpointUri(HttpClient client, Uri? requestAddress, string discoveryPath = ".well-known/agent.json")
    {
        var builder = new UriBuilder(requestAddress?.ToString() ?? client.BaseAddress?.ToString() ?? $"/{discoveryPath}");
        if (builder.Uri.IsAbsoluteUri)
        {
            builder.Query = requestAddress?.Query ?? client.BaseAddress?.Query;
            builder.Path = builder.Path.TrimEnd('/') + $"/{discoveryPath}";
        }

        return builder.Uri;
    }

    private static Uri MakeRegistryDiscoveryEndpointUri(HttpClient client, Uri? requestAddress, string discoveryPath = ".well-known/agents.json")
    {
        var builder = new UriBuilder(requestAddress?.ToString() ?? client.BaseAddress?.ToString() ?? $"/{discoveryPath}");
        if (builder.Uri.IsAbsoluteUri)
        {
            builder.Query = requestAddress?.Query ?? client.BaseAddress?.Query;
            builder.Path = builder.Path.TrimEnd('/') + $"/{discoveryPath}";
        }

        return builder.Uri;
    }
}
