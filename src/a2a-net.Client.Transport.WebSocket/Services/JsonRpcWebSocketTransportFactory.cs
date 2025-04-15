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

namespace A2A.Client.Transport.WebSocket.Services;

/// <summary>
/// Represents the WebSocket implementation of the <see cref="IJsonRpcTransportFactory"/> interface
/// </summary>
/// <param name="options">The service used to access the current <see cref="JsonRpcWebSocketTransportFactoryOptions"/></param>
/// <param name="jsonRpcMessageFormatter">The service used to format JSON-RPC messages</param>
public class JsonRpcWebSocketTransportFactory(IOptions<JsonRpcWebSocketTransportFactoryOptions> options, IJsonRpcMessageFormatter jsonRpcMessageFormatter)
    : IJsonRpcTransportFactory, IDisposable
{

    bool _disposed;

    /// <summary>
    /// Gets the current <see cref="JsonRpcWebSocketTransportFactoryOptions"/>
    /// </summary>
    protected JsonRpcWebSocketTransportFactoryOptions Options { get; } = options.Value;

    /// <summary>
    /// Gets the service used to format JSON-RPC messages
    /// </summary>
    protected IJsonRpcMessageFormatter JsonRpcMessageFormatter { get; } = jsonRpcMessageFormatter;

    /// <summary>
    /// Gets the underlying <see cref="ClientWebSocket"/>
    /// </summary>
    protected ClientWebSocket? Client { get; set; }

    /// <inheritdoc/>
    public virtual async Task<JsonRpc> CreateAsync(CancellationToken cancellationToken = default)
    {
        Client = new ClientWebSocket();
        await Client.ConnectAsync(Options.Endpoint, cancellationToken).ConfigureAwait(false);
        return new(new WebSocketMessageHandler(Client, JsonRpcMessageFormatter));
    }

    /// <summary>
    /// Disposes of the <see cref="A2AProtocolClient"/>
    /// </summary>
    /// <param name="disposing">A boolean indicating whether or not the <see cref="A2AProtocolClient"/> is being disposed of</param>
    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing) Client?.Dispose();
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
