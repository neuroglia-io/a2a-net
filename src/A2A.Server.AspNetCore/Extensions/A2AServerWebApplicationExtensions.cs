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

using A2A.Models;
using A2A.Server.Transports;

#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace A2A.Server;

/// <summary>
/// Defines extensions for <see cref="WebApplication"/>s.
/// </summary>
public static class A2AServerWebApplicationExtensions
{

    const string JwksEndpointRegistrationFlag = "jwks-endpoint:registered";

    /// <summary>
    /// Configures the application to use the registered A2A server.
    /// </summary>
    /// <param name="application">The <see cref="WebApplication"/> to configure.</param>
    public static void UseA2AServer(this WebApplication application)
    {
        ArgumentNullException.ThrowIfNull(application);
        application.MapWellKnownA2AAgentCard();
        var agentCard = application.Services.GetRequiredService<AgentCard>();
        if (agentCard.Interfaces is null || agentCard.Interfaces.Count < 1) return;
        if (agentCard.Capabilities?.PushNotifications is true) application.MapWellKnownJwksEndpoint();
        foreach (var agentInterface in agentCard.Interfaces)
        {
            switch (agentInterface.ProtocolBinding)
            {
                case ProtocolBinding.Http:
                    application.MapA2AHttpEndpoints(agentInterface);
                    break;
                case ProtocolBinding.Grpc:
                    application.MapA2AGrpcEndpoints(agentInterface);
                    break;
                case ProtocolBinding.JsonRpc:
                    application.MapA2AJsonRpcEndpoints(agentInterface);
                    break;
                default:
                    throw new NotSupportedException($"The specified protocol binding '{agentInterface.ProtocolBinding}' is not supported.");
            }
        }
    }

    static void MapA2AHttpEndpoints(this IEndpointRouteBuilder endpoints, AgentInterface agentInterface)
    {
        var group = endpoints.MapGroup(agentInterface.Url.AbsolutePath);
        group.Map("/{*path}", async (HttpContext httpContext) =>
        {
            var transport = httpContext.RequestServices.GetRequiredKeyedService<IA2ATransport>(ProtocolBinding.Http);
            return await transport.HandleAsync(httpContext).ConfigureAwait(false);
        });
    }

    static void MapA2AGrpcEndpoints(this IEndpointRouteBuilder endpoints, AgentInterface agentInterface)
    {
        endpoints.Map(agentInterface.Url.AbsolutePath, async (HttpContext httpContext) =>
        {
            var transport = httpContext.RequestServices.GetRequiredKeyedService<IA2ATransport>(ProtocolBinding.Grpc);
            return await transport.HandleAsync(httpContext).ConfigureAwait(false);
        });
    }

    static void MapA2AJsonRpcEndpoints(this IEndpointRouteBuilder endpoints, AgentInterface agentInterface)
    {
        endpoints.MapGrpcService<A2AGrpcServerService>();
    }

    static void MapWellKnownA2AAgentCard(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet("/.well-known/agent-card.json", async httpContext =>
        {
            var agentCard = httpContext.RequestServices.GetRequiredKeyedService<AgentCard>(null);
            httpContext.Response.ContentType = MediaTypeNames.Application.Json;
            await httpContext.Response.WriteAsJsonAsync(agentCard, JsonSerializationContext.Default.AgentCard, MediaTypeNames.Application.Json, httpContext.RequestAborted);
        });
    }

    static void MapWellKnownJwksEndpoint(this IEndpointRouteBuilder endpoints)
    {
        var data = endpoints.DataSources;
        if (endpoints is not IApplicationBuilder appBuilder || appBuilder.Properties.ContainsKey(JwksEndpointRegistrationFlag)) return;
        endpoints.MapGet("/.well-known/jwks.json", async httpContext =>
        {
            var json = httpContext.RequestServices.GetRequiredService<IJsonWebKeySet>().Export();
            httpContext.Response.StatusCode = StatusCodes.Status200OK;
            httpContext.Response.ContentType = MediaTypeNames.Application.Json;
            await httpContext.Response.WriteAsync(json, Encoding.UTF8, httpContext.RequestAborted).ConfigureAwait(false);
        });
        appBuilder.Properties[JwksEndpointRegistrationFlag] = true;
    }

}