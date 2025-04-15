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
/// Defines extensions for <see cref="IA2AProtocolClient"/>s
/// </summary>
public static class IA2AProtocolClientBuilderExtensions
{

    /// <summary>
    /// Configures the <see cref="IA2AProtocolClientBuilder"/> to use the WebSocket transport for JSON-RPC
    /// </summary>
    /// <param name="builder">The <see cref="IA2AProtocolClientBuilder"/> to configure</param>
    /// <param name="setup">An <see cref="Action{T}"/> used to configure the <see cref="JsonRpcWebSocketTransportFactory"/> to use</param>
    /// <returns>The configured <see cref="IA2AProtocolClientBuilder"/></returns>
    public static IA2AProtocolClientBuilder UseWebSocketTransport(this IA2AProtocolClientBuilder builder, Action<JsonRpcWebSocketTransportFactoryOptions> setup)
    {
        ArgumentNullException.ThrowIfNull(builder);
        ArgumentNullException.ThrowIfNull(setup);
        builder.Services.Configure(setup);
        builder.UseTransportFactory<JsonRpcWebSocketTransportFactory>();
        return builder;
    }

}
