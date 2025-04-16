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

namespace A2A.Client.Services;

/// <summary>
/// Represents the default HTTP implementation of the <see cref="IA2AProtocolClient"/> interface
/// </summary>
/// <param name="options">The service used to access the current <see cref="A2AProtocolClientOptions"/></param>
/// <param name="httpClient">The service used to perform HTTP requests</param>
public class A2AProtocolHttpClient(IOptions<A2AProtocolClientOptions> options, HttpClient httpClient)
    : IA2AProtocolClient, IDisposable
{

    bool _disposed;

    /// <summary>
    /// Gets the current <see cref="A2AProtocolClientOptions"/>
    /// </summary>
    protected A2AProtocolClientOptions Options { get; } = options.Value;

    /// <summary>
    /// Gets the service used to perform HTTP requests
    /// </summary>
    protected HttpClient HttpClient { get; } = httpClient;

    /// <inheritdoc/>
    public virtual async Task<RpcResponse<Models.Task>> SendTaskAsync(SendTaskRequest request, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);
        using var httpResponse = await HttpClient.PostAsJsonAsync(Options.Endpoint, request, cancellationToken).ConfigureAwait(false);
        httpResponse.EnsureSuccessStatusCode();
        return (await httpResponse.Content.ReadFromJsonAsync<RpcResponse<Models.Task>>(cancellationToken).ConfigureAwait(false))!;
    }

    /// <inheritdoc/>
    public virtual async IAsyncEnumerable<RpcResponse<TaskEvent>> SendTaskStreamingAsync(SendTaskStreamingRequest request, [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);
        using var content = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, MediaTypeNames.Application.Json);
        using var httpRequest = new HttpRequestMessage(HttpMethod.Post, Options.Endpoint) { Content = content };
        httpRequest.EnableWebAssemblyStreamingResponse();
        using var httpResponse = await HttpClient.SendAsync(httpRequest, cancellationToken).ConfigureAwait(false);
        httpResponse.EnsureSuccessStatusCode();
        var httpResponseStream = await httpResponse.Content.ReadAsStreamAsync(cancellationToken).ConfigureAwait(false);
        using var streamReader = new StreamReader(await httpResponse.Content.ReadAsStreamAsync(cancellationToken).ConfigureAwait(false));
        while (!streamReader.EndOfStream)
        {
            var sseMessage = await streamReader.ReadLineAsync(cancellationToken).ConfigureAwait(false);
            if (string.IsNullOrWhiteSpace(sseMessage)) continue;
            var json = sseMessage["data: ".Length..].Trim();
            var e = JsonSerializer.Deserialize<RpcResponse<TaskEvent>>(json)!;
            yield return e;
        }
    }

    /// <inheritdoc/>
    public virtual async IAsyncEnumerable<RpcResponse<TaskEvent>> ResubscribeToTaskAsync(TaskResubscriptionRequest request, [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);
        using var content = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, MediaTypeNames.Application.Json);
        using var httpRequest = new HttpRequestMessage(HttpMethod.Post, Options.Endpoint) { Content = content };
        httpRequest.EnableWebAssemblyStreamingResponse();
        using var httpResponse = await HttpClient.SendAsync(httpRequest, cancellationToken).ConfigureAwait(false);
        httpResponse.EnsureSuccessStatusCode();
        var httpResponseStream = await httpResponse.Content.ReadAsStreamAsync(cancellationToken).ConfigureAwait(false);
        using var streamReader = new StreamReader(await httpResponse.Content.ReadAsStreamAsync(cancellationToken).ConfigureAwait(false));
        while (!streamReader.EndOfStream)
        {
            var sseMessage = await streamReader.ReadLineAsync(cancellationToken).ConfigureAwait(false);
            if (string.IsNullOrWhiteSpace(sseMessage)) continue;
            var json = sseMessage["data: ".Length..].Trim();
            var e = JsonSerializer.Deserialize<RpcResponse<TaskEvent>>(json)!;
            yield return e;
        }
    }

    /// <inheritdoc/>
    public virtual async Task<RpcResponse<Models.Task>> GetTaskAsync(GetTaskRequest request, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);
        using var httpResponse = await HttpClient.PostAsJsonAsync(Options.Endpoint, request, cancellationToken).ConfigureAwait(false);
        httpResponse.EnsureSuccessStatusCode();
        return (await httpResponse.Content.ReadFromJsonAsync<RpcResponse<Models.Task>>(cancellationToken).ConfigureAwait(false))!;
    }

    /// <inheritdoc/>
    public virtual async Task<RpcResponse<Models.Task>> CancelTaskAsync(CancelTaskRequest request, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);
        ArgumentNullException.ThrowIfNull(request);
        using var httpResponse = await HttpClient.PostAsJsonAsync(Options.Endpoint, request, cancellationToken).ConfigureAwait(false);
        httpResponse.EnsureSuccessStatusCode();
        return (await httpResponse.Content.ReadFromJsonAsync<RpcResponse<Models.Task>>(cancellationToken).ConfigureAwait(false))!;
    }

    /// <inheritdoc/>
    public virtual async Task<RpcResponse<TaskPushNotificationConfiguration>> GetTaskPushNotificationsAsync(GetTaskPushNotificationsRequest request, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);
        ArgumentNullException.ThrowIfNull(request);
        using var httpResponse = await HttpClient.PostAsJsonAsync(Options.Endpoint, request, cancellationToken).ConfigureAwait(false);
        httpResponse.EnsureSuccessStatusCode();
        return (await httpResponse.Content.ReadFromJsonAsync<RpcResponse<TaskPushNotificationConfiguration>>(cancellationToken).ConfigureAwait(false))!;
    }

    /// <inheritdoc/>
    public virtual async Task<RpcResponse<TaskPushNotificationConfiguration>> SetTaskPushNotificationsAsync(SetTaskPushNotificationsRequest request, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);
        ArgumentNullException.ThrowIfNull(request);
        using var httpResponse = await HttpClient.PostAsJsonAsync(Options.Endpoint, request, cancellationToken).ConfigureAwait(false);
        httpResponse.EnsureSuccessStatusCode();
        return (await httpResponse.Content.ReadFromJsonAsync<RpcResponse<TaskPushNotificationConfiguration>>(cancellationToken).ConfigureAwait(false))!;
    }

    /// <summary>
    /// Disposes of the <see cref="A2AProtocolHttpClient"/>
    /// </summary>
    /// <param name="disposing">A boolean indicating whether or not the <see cref="A2AProtocolHttpClient"/> is being disposed of</param>
    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                
            }
            _disposed = true;
        }
    }

    /// <inheritdoc/>
    public void Dispose()
    {
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }

}
