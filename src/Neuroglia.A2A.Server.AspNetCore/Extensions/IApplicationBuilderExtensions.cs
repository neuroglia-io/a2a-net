using Neuroglia.A2A.Server.Infrastructure.Services;
using StreamJsonRpc;

namespace Neuroglia.A2A.Server.AspNetCore;

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
                    var protocolHandler = context.RequestServices.GetRequiredService<IA2AProtocolHandler>();
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
