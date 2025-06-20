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

namespace A2A.Client.Transport.WebSocket.Services;

/// <summary>
/// Represents the WebSocket implementation of the <see cref="IA2AProtocolClient"/> interface
/// </summary>
/// <param name="logger">The service used to perform logging </param>
/// <param name="options">The service used to access the current <see cref="A2AProtocolClientOptions"/></param>
public class A2AProtocolWebSocketClient(ILogger<A2AProtocolWebSocketClient> logger, IOptions<A2AProtocolClientOptions> options)
    : IA2AProtocolClient
{

    ClientWebSocket? _clientWebSocket;
    JsonRpc? _jsonRpc;
    bool _disposed;

    /// <summary>
    /// Gets the service used to perform logging 
    /// </summary>
    protected ILogger Logger { get; } = logger;

    /// <summary>
    /// Gets the current <see cref="A2AProtocolClientOptions"/>
    /// </summary>
    protected A2AProtocolClientOptions Options { get; } = options.Value;

    /// <inheritdoc/>
    public virtual async Task<RpcResponse<Models.Task>> SendMessageAsync(SendMessageRequest request, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);
        var transport = await GetOrCreateTransportAsync(cancellationToken).ConfigureAwait(false);
        return await transport.InvokeWithCancellationAsync<RpcResponse<Models.Task>>(A2AProtocol.Methods.Messages.Send, [request], cancellationToken).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public virtual async IAsyncEnumerable<RpcResponse<TaskEvent>> StreamMessageAsync(StreamMessageRequest request, [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);
        var transport = await GetOrCreateTransportAsync(cancellationToken).ConfigureAwait(false);
        await foreach (var e in await transport.InvokeWithCancellationAsync<IAsyncEnumerable<RpcResponse<TaskEvent>>>(A2AProtocol.Methods.Messages.Stream, [request], cancellationToken).ConfigureAwait(false)) yield return e;
    }

    /// <inheritdoc/>
    public virtual async IAsyncEnumerable<RpcResponse<TaskEvent>> ResubscribeToTaskAsync(ResubscribeToTaskRequest request, [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);
        var transport = await GetOrCreateTransportAsync(cancellationToken).ConfigureAwait(false);
        await foreach (var e in await transport.InvokeWithCancellationAsync<IAsyncEnumerable<RpcResponse<TaskEvent>>>(A2AProtocol.Methods.Tasks.Resubscribe, [request], cancellationToken).ConfigureAwait(false)) yield return e;
    }

    /// <inheritdoc/>
    public virtual async Task<RpcResponse<Models.Task>> GetTaskAsync(GetTaskRequest request, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);
        var transport = await GetOrCreateTransportAsync(cancellationToken).ConfigureAwait(false);
        return await transport.InvokeWithCancellationAsync<RpcResponse<Models.Task>>(A2AProtocol.Methods.Tasks.Get, [request], cancellationToken).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public virtual async Task<RpcResponse<Models.Task>> CancelTaskAsync(CancelTaskRequest request, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);
        var transport = await GetOrCreateTransportAsync(cancellationToken).ConfigureAwait(false);
        return await transport.InvokeWithCancellationAsync<RpcResponse<Models.Task>>(A2AProtocol.Methods.Tasks.Cancel, [request], cancellationToken).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public virtual async Task<RpcResponse<TaskPushNotificationConfiguration>> SetTaskPushNotificationsAsync(SetTaskPushNotificationsRequest request, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);
        var transport = await GetOrCreateTransportAsync(cancellationToken).ConfigureAwait(false);
        return await transport.InvokeWithCancellationAsync<RpcResponse<TaskPushNotificationConfiguration>>(A2AProtocol.Methods.Tasks.PushNotifications.Set, [request], cancellationToken).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public virtual async Task<RpcResponse<TaskPushNotificationConfiguration>> GetTaskPushNotificationsAsync(GetTaskPushNotificationsRequest request, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);
        var transport = await GetOrCreateTransportAsync(cancellationToken).ConfigureAwait(false);
        return await transport.InvokeWithCancellationAsync<RpcResponse<TaskPushNotificationConfiguration>>(A2AProtocol.Methods.Tasks.PushNotifications.Get, [request], cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Gets or creates the <see cref="JsonRpc"/> instance to use
    /// </summary>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/></param>
    /// <returns>The <see cref="JsonRpc"/> instance to use</returns>
    protected virtual async Task<JsonRpc> GetOrCreateTransportAsync(CancellationToken cancellationToken)
    {
        if (_jsonRpc == null)
        {
            _clientWebSocket = new ClientWebSocket();
            await _clientWebSocket.ConnectAsync(Options.Endpoint, cancellationToken).ConfigureAwait(false);
            _jsonRpc = new(new WebSocketMessageHandler(_clientWebSocket, new SystemTextJsonFormatter()));
            _jsonRpc.StartListening();
        }
        return _jsonRpc;
    }

    /// <summary>
    /// Disposes of the <see cref="A2AProtocolWebSocketClient"/>
    /// </summary>
    /// <param name="disposing">A boolean indicating whether or not the <see cref="A2AProtocolWebSocketClient"/> is being disposed of</param>
    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing) _jsonRpc?.Dispose();
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
