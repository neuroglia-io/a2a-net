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

#pragma warning disable IDE0130 // Namespace does not match folder structure
using A2A.Client.Transports;

namespace A2A.Client;

/// <summary>
/// Defines extensions for <see cref="IA2AClientBuilder"/>s.
/// </summary>
public static class A2AClientBuilderExtensions
{

    /// <summary>
    /// Configures the <see cref="IA2AClientBuilder"/> to use the HTTP transport.
    /// </summary>
    /// <param name="builder">The <see cref="IA2AClientBuilder"/> to configure.</param>
    /// <param name="configureClient">An <see cref="Action{T1, T2}"/> used to configure the underlying <see cref="HttpClient"/>.</param>
    /// <param name="configureClientBuilder"> An <see cref="Action{T}"/>, if any, used to configure the <see cref="IHttpClientBuilder"/> used to build the underlying <see cref="HttpClient"/>.</param>
    /// <returns>The configured <see cref="IA2AClientBuilder"/>.</returns>
    public static IA2AClientBuilder UseHttpTransport(this IA2AClientBuilder builder, Action<IServiceProvider, HttpClient> configureClient, Action<IHttpClientBuilder>? configureClientBuilder = null)
    {
        var httpClientBuilder = builder.Services.AddHttpClient<IA2AClientTransport, A2AHttpClientTransport>((provider, httpClient) =>
        {
            httpClient.DefaultRequestHeaders.Add("A2A-Version", A2AProtocolVersion.Latest);
            configureClient(provider, httpClient);
        });
        configureClientBuilder?.Invoke(httpClientBuilder);
        return builder.UseTransport<A2AHttpClientTransport>();
    }

    /// <summary>
    /// Configures the <see cref="IA2AClientBuilder"/> to use the HTTP transport.
    /// </summary>
    /// <param name="builder">The <see cref="IA2AClientBuilder"/> to configure.</param>
    /// <param name="configureClient">An <see cref="Action{T}"/> used to configure the underlying <see cref="HttpClient"/>.</param>
    /// <param name="configureClientBuilder"> An <see cref="Action{T}"/>, if any, used to configure the <see cref="IHttpClientBuilder"/> used to build the underlying <see cref="HttpClient"/>.</param>
    /// <returns>The configured <see cref="IA2AClientBuilder"/>.</returns>
    public static IA2AClientBuilder UseHttpTransport(this IA2AClientBuilder builder, Action<HttpClient> configureClient, Action<IHttpClientBuilder>? configureClientBuilder = null) => UseHttpTransport(builder, (_, httpClient) => configureClient(httpClient), configureClientBuilder);

    /// <summary>
    /// Configures the <see cref="IA2AClientBuilder"/> to use the HTTP transport.
    /// </summary>
    /// <param name="builder">The <see cref="IA2AClientBuilder"/> to configure.</param>
    /// <param name="baseAddress">The based address of the server to connect to.</param>
    /// <param name="configureClientBuilder"> An <see cref="Action{T}"/>, if any, used to configure the <see cref="IHttpClientBuilder"/> used to build the underlying <see cref="HttpClient"/>.</param>
    /// <returns>The configured <see cref="IA2AClientBuilder"/>.</returns>
    public static IA2AClientBuilder UseHttpTransport(this IA2AClientBuilder builder, Uri baseAddress, Action<IHttpClientBuilder>? configureClientBuilder = null) => UseHttpTransport(builder, httpClient => httpClient.BaseAddress = baseAddress, configureClientBuilder);

}
