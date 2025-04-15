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
/// Represents the default implementation of the <see cref="IA2AProtocolClient"/> interface
/// </summary>
/// <param name="logger">The service used to perform logging </param>
/// <param name="jsonRpcTransportFactory">The service used to create <see cref="JsonRpc"/> instances</param>
public class A2AProtocolClient(ILogger<A2AProtocolClient> logger, IJsonRpcTransportFactory jsonRpcTransportFactory)
    : IA2AProtocolClient
{

    JsonRpc? _jsonRpc;
    bool _disposed;

    /// <summary>
    /// Gets the service used to perform logging 
    /// </summary>
    protected ILogger Logger { get; } = logger;

    /// <summary>
    /// Gets the service used to create <see cref="JsonRpc"/> instances
    /// </summary>
    protected IJsonRpcTransportFactory JsonRpcTransportFactory { get; } = jsonRpcTransportFactory;

    /// <inheritdoc/>
    public virtual async Task<RpcResponse<Models.Task>> SendTaskAsync(SendTaskRequest request, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);
        var transport = await GetOrCreateTransportAsync(cancellationToken).ConfigureAwait(false);
        return await transport.InvokeWithCancellationAsync<RpcResponse<Models.Task>>("tasks/send", [request], cancellationToken).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public virtual async IAsyncEnumerable<RpcResponse<TaskEvent>> SendTaskStreamingAsync(SendTaskStreamingRequest request, [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);
        var transport = await GetOrCreateTransportAsync(cancellationToken).ConfigureAwait(false);
        await foreach(var e in await transport.InvokeWithCancellationAsync<IAsyncEnumerable<RpcResponse<TaskEvent>>>("tasks/sendSubscribe", [request], cancellationToken).ConfigureAwait(false)) yield return e;
    }

    /// <inheritdoc/>
    public virtual async IAsyncEnumerable<RpcResponse<TaskEvent>> ResubscribeToTaskAsync(TaskResubscriptionRequest request, [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);
        var transport = await GetOrCreateTransportAsync(cancellationToken).ConfigureAwait(false);
        await foreach (var e in await transport.InvokeWithCancellationAsync<IAsyncEnumerable<RpcResponse<TaskEvent>>>("tasks/resubscribe", [request], cancellationToken).ConfigureAwait(false)) yield return e;
    }

    /// <inheritdoc/>
    public virtual async Task<RpcResponse<Models.Task>> GetTaskAsync(GetTaskRequest request, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);
        var transport = await GetOrCreateTransportAsync(cancellationToken).ConfigureAwait(false);
        return await transport.InvokeWithCancellationAsync<RpcResponse<Models.Task>>("tasks/get", [request], cancellationToken).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public virtual async Task<RpcResponse<Models.Task>> CancelTaskAsync(CancelTaskRequest request, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);
        var transport = await GetOrCreateTransportAsync(cancellationToken).ConfigureAwait(false);
        return await transport.InvokeWithCancellationAsync<RpcResponse<Models.Task>>("tasks/cancel", [request], cancellationToken).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public virtual async Task<RpcResponse<TaskPushNotificationConfiguration>> SetTaskPushNotificationsAsync(SetTaskPushNotificationsRequest request, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);
        var transport = await GetOrCreateTransportAsync(cancellationToken).ConfigureAwait(false);
        return await transport.InvokeWithCancellationAsync<RpcResponse<TaskPushNotificationConfiguration>>("tasks/pushNotification/set", [request], cancellationToken).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public virtual async Task<RpcResponse<TaskPushNotificationConfiguration>> GetTaskPushNotificationsAsync(GetTaskPushNotificationsRequest request, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);
        var transport = await GetOrCreateTransportAsync(cancellationToken).ConfigureAwait(false);
        return await transport.InvokeWithCancellationAsync<RpcResponse<TaskPushNotificationConfiguration>>("tasks/pushNotification/get", [request], cancellationToken).ConfigureAwait(false);
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
            _jsonRpc = await JsonRpcTransportFactory.CreateAsync(cancellationToken).ConfigureAwait(false);
            _jsonRpc.StartListening();
        }
        return _jsonRpc;
    }

    /// <summary>
    /// Disposes of the <see cref="A2AProtocolClient"/>
    /// </summary>
    /// <param name="disposing">A boolean indicating whether or not the <see cref="A2AProtocolClient"/> is being disposed of</param>
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