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

using A2A.Server.Infrastructure.Services;
using StreamJsonRpc;

namespace A2A.Server.AspNetCore;

/// <summary>
/// Defines extensions for <see cref="IApplicationBuilder"/>s
/// </summary>
public static class IApplicationBuilderExtensions
{

    /// <summary>
    /// Maps an endpoint to handle A2A protocol requests over WebSocket using JSON-RPC
    /// </summary>
    /// <param name="app">The <see cref="IApplicationBuilder"/> used to configure the application pipeline</param>
    /// <param name="path">The endpoint path to listen on for WebSocket connections</param>
    /// <returns>The configured <see cref="IApplicationBuilder"/> instance.</returns>
    public static IApplicationBuilder MapA2AEndpoint(this IApplicationBuilder app, string path = "/a2a")
    {
        app.UseWebSockets();
        app.Map(path, builder =>
        {
            builder.Use(async (HttpContext context, RequestDelegate next) =>
            {
                if (context.WebSockets.IsWebSocketRequest)
                {
                    var protocolHandler = context.RequestServices.GetRequiredService<IA2AProtocolServer>();
                    using var socket = await context.WebSockets.AcceptWebSocketAsync();
                    using var jsonRpc = new JsonRpc(new WebSocketMessageHandler(socket, new SystemTextJsonFormatter()), protocolHandler);
                    jsonRpc.CancelLocallyInvokedMethodsWhenConnectionIsClosed = true;
                    jsonRpc.StartListening();
                    await jsonRpc.Completion;
                }
                else
                {
                    context.Response.StatusCode = StatusCodes.Status405MethodNotAllowed;
                }
            });
        });
        return app;
    }

}
