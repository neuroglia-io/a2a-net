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

#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace A2A.Server;

/// <summary>
/// Defines extensions for <see cref="IEndpointRouteBuilder"/>s.
/// </summary>
public static class A2AServerEndpointRouteBuilderExtensions
{

    const string JwksEndpointRegistrationFlag = "jwks-endpoint:registered";

    /// <summary>
    /// Maps A2A endpoints to the application's routing pipeline.
    /// </summary>
    /// <param name="endpoints">The endpoint route builder used to configure application endpoints.</param>
    public static void MapA2A(this IEndpointRouteBuilder endpoints)
    {
        ArgumentNullException.ThrowIfNull(endpoints);
        endpoints.MapWellKnownA2AAgentCard();
        var agentCard = endpoints.ServiceProvider.GetRequiredService<AgentCard>();
        if (agentCard.Interfaces is null || agentCard.Interfaces.Count < 1) return;
        if (agentCard.Capabilities?.PushNotifications is true) endpoints.MapWellKnownJwksEndpoint();
        foreach (var agentInterface in agentCard.Interfaces)
        {
            switch (agentInterface.ProtocolBinding)
            {
                case ProtocolBinding.Http:
                    endpoints.MapA2AHttpEndpoints(agentInterface);
                    break;
                case ProtocolBinding.Grpc:
                    endpoints.MapA2AGrpcEndpoints(agentInterface);
                    break;
                case ProtocolBinding.JsonRpc:
                    endpoints.MapA2AJsonRpcEndpoints(agentInterface);
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
        endpoints.Map(agentInterface.Url.AbsolutePath, async (HttpContext httpContext) =>
        {
            var transport = httpContext.RequestServices.GetRequiredKeyedService<IA2ATransport>(ProtocolBinding.JsonRpc);
            return await transport.HandleAsync(httpContext).ConfigureAwait(false);
        });
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