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

    internal const string ServerVariableName = "server";

    /// <summary>
    /// Maps the A2A agent middleware to a specified route pattern.
    /// If the pattern includes a <c>{server}</c> route variable, the middleware will resolve the server by key.
    /// </summary>
    /// <param name="endpoints">The endpoint route builder</param>
    /// <param name="pattern">The route pattern to map (e.g., <c>/a2a</c> or '<c>/a2a/{server}</c>')</param>
    public static void MapA2AAgentHttpEndpoint(this IEndpointRouteBuilder endpoints, string pattern = $"/a2a/{{{ServerVariableName}}}")
    {
        endpoints.Map(pattern, async context =>
        {
            var middleware = ActivatorUtilities.CreateInstance<A2AAgentHttpMiddleware>(context.RequestServices);
            await middleware.InvokeAsync(context);
        });
    }

    /// <summary>
    /// Maps the A2A agent middleware to a specified route pattern.
    /// If the pattern includes a <c>{server}</c> route variable, the middleware will resolve the server by key.
    /// </summary>
    /// <param name="endpoints">The endpoint route builder</param>
    /// <param name="pattern">The route pattern to map (e.g., <c>/a2a</c> or '<c>/a2a/{server}</c>')</param>
    public static void MapA2AAgentWebSocketEndpoint(this IEndpointRouteBuilder endpoints, string pattern = $"/a2a/{{{ServerVariableName}}}")
    {
        endpoints.Map(pattern, async context =>
        {
            var middleware = ActivatorUtilities.CreateInstance<A2AAgentWebSocketMiddleware>(context.RequestServices);
            await middleware.InvokeAsync(context);
        });
    }

}
