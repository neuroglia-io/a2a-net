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

using A2A.Server.AspNetCore.Services;

namespace A2A.Server.AspNetCore;

/// <summary>
/// Defines extensions for <see cref="IEndpointRouteBuilder"/>s
/// </summary>
public static class A2AEndpointRouteBuilderExtensions
{

    internal const string AgentVariableName = "agent";
    const string JwksEndpointRegistrationFlag = "jwks-endpoint:registered";

    /// <summary>
    /// Maps the A2A agent middleware to a specified route pattern.
    /// If the pattern includes a <c>{agent}</c> route variable, the middleware will resolve the agent by key.
    /// </summary>
    /// <param name="endpoints">The endpoint route builder</param>
    /// <param name="pattern">The route pattern to map (e.g., <c>/a2a</c> or '<c>/a2a/{agent}</c>')</param>
    public static void MapA2AHttpEndpoint(this IEndpointRouteBuilder endpoints, string pattern = $"/a2a/{{{AgentVariableName}}}")
    {
        endpoints.MapWellKnownJwksEndpoint();
        endpoints.Map(pattern, async context =>
        {
            var middleware = ActivatorUtilities.CreateInstance<A2AHttpMiddleware>(context.RequestServices);
            await middleware.InvokeAsync(context);
        });
    }

    /// <summary>
    /// Maps the A2A agent middleware to a specified route pattern.
    /// If the pattern includes a <c>{agent}</c> route variable, the middleware will resolve the agent by key.
    /// </summary>
    /// <param name="endpoints">The endpoint route builder</param>
    /// <param name="pattern">The route pattern to map (e.g., <c>/a2a</c> or '<c>/a2a/{agent}</c>')</param>
    public static void MapA2AWebSocketEndpoint(this IEndpointRouteBuilder endpoints, string pattern = $"/a2a/{{{AgentVariableName}}}")
    {
        endpoints.MapWellKnownJwksEndpoint();
        endpoints.Map(pattern, async context =>
        {
            var middleware = ActivatorUtilities.CreateInstance<A2AWebSocketMiddleware>(context.RequestServices);
            await middleware.InvokeAsync(context);
        });
    }

    static void MapWellKnownJwksEndpoint(this IEndpointRouteBuilder endpoints)
    {
        var data = endpoints.DataSources;
        if (endpoints is not IApplicationBuilder appBuilder || appBuilder.Properties.ContainsKey(JwksEndpointRegistrationFlag)) return;
        endpoints.MapGet("/.well-known/jwks.json", async httpContext =>
        {
            var json = httpContext.RequestServices.GetRequiredService<IJsonWebKeySet>().Export();
            httpContext.Response.StatusCode = (int)HttpStatusCode.OK;
            httpContext.Response.ContentType = MediaTypeNames.Application.Json;
            await httpContext.Response.WriteAsync(json, Encoding.UTF8, httpContext.RequestAborted).ConfigureAwait(false);
        });
        appBuilder.Properties[JwksEndpointRegistrationFlag] = true;
    }

}
