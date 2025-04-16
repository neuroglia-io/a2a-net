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

namespace A2A.Server.AspNetCore;

/// <summary>
/// Defines extensions for <see cref="IApplicationBuilder"/>s
/// </summary>
public static class IApplicationBuilderExtensions
{

    /// <summary>
    /// Maps a well-known HTTP endpoint for A2A agent discovery, exposing the agent manifest(s) as JSON<para></para>
    /// If a single <see cref="AgentCard"/> is registered, it will be served at <c>/.well-known/agent.json</c><para></para>
    /// If multiple agents are registered, they will be served as a list at <c>/.well-known/agents.json</c>
    /// </summary>
    /// <param name="app">The <see cref="IApplicationBuilder"/> to configure</param>
    public static void MapA2AWellKnownAgentEndpoint(this IApplicationBuilder app)
    {
        var agents = app.ApplicationServices.GetServices<AgentCard>().ToList();
        if (agents.Count < 1) return;
        else if(agents.Count == 1)
        {
            app.Map("/.well-known/agent.json", app =>
            {
                app.Use(async (HttpContext context, RequestDelegate next) =>
                {
                    context.Response.ContentType = MediaTypeNames.Application.Json;
                    await context.Response.WriteAsJsonAsync(agents[0]);
                });
            });
        }
        else
        {
            app.Map("/.well-known/agents.json", app =>
            {
                app.Use(async (HttpContext context, RequestDelegate next) =>
                {
                    context.Response.ContentType = MediaTypeNames.Application.Json;
                    await context.Response.WriteAsJsonAsync(agents);
                });
            });
        }
    }

}
