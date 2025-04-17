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

using Microsoft.VisualStudio.Threading;

namespace A2A.Server.AspNetCore.Services;

/// <summary>
/// Represents the middleware that handles WebSocket-based JSON-RPC requests for an A2A agent
/// </summary>
/// <remarks>
/// Initializes a new <see cref="A2AAgentWebSocketMiddleware"/>
/// </remarks>
/// <param name="serverProvider">The service used to provide <see cref="IA2AProtocolServer"/>s</param>
public class A2AAgentWebSocketMiddleware(IA2AProtocolServerProvider serverProvider)
{

    readonly IA2AProtocolServerProvider _serverProvider = serverProvider;

    /// <summary>
    /// Invokes the <see cref="A2AAgentHttpMiddleware"/>
    /// </summary>
    /// <param name="context">The current <see cref="HttpContext"/></param>
    /// <returns>A new awaitable <see cref="System.Threading.Tasks.Task"/></returns>
    public async System.Threading.Tasks.Task InvokeAsync(HttpContext context)
    {
        if (context.WebSockets.IsWebSocketRequest)
        {
            context.Response.StatusCode = StatusCodes.Status405MethodNotAllowed;
            return;
        }
        var serverName = context.Request.RouteValues.TryGetValue(A2AEndpointRouteBuilderExtensions.AgentVariableName, out var value) && value is string name && !string.IsNullOrWhiteSpace(name) ? name : A2AProtocolServer.DefaultName;
        var server = _serverProvider.Get(serverName);
        using var socket = await context.WebSockets.AcceptWebSocketAsync().ConfigureAwait(false);
        using var jsonRpc = new JsonRpc(new WebSocketMessageHandler(socket, new SystemTextJsonFormatter()), server);
        jsonRpc.CancelLocallyInvokedMethodsWhenConnectionIsClosed = true;
        jsonRpc.StartListening();
        await jsonRpc.Completion.WithCancellation(context.RequestAborted).ConfigureAwait(false);
    }

}