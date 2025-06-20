﻿// Copyright © 2025-Present the a2a-net Authors
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

namespace A2A.Server;

/// <summary>
/// Defines extensions for <see cref="IServiceCollection"/>s
/// </summary>
public static class IServiceCollectionExtensions
{

    /// <summary>
    /// Adds and configures a new well known A2A agent
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> to configure</param>
    /// <param name="agent">The <see cref="AgentCard"/> that describes the well known A2A agent to add</param>
    /// <returns>The configured <see cref="IServiceCollection"/></returns>
    public static IServiceCollection AddA2AWellKnownAgent(this IServiceCollection services, AgentCard agent)
    {
        ArgumentNullException.ThrowIfNull(agent);
        services.AddSingleton(agent);
        return services;
    }

    /// <summary>
    /// Adds and configures a new well known A2A agent
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> to configure</param>
    /// <param name="setup">An <see cref="Action{T}"/> used to setup the <see cref="AgentCard"/> that describes the well known A2A agent to add</param>
    /// <returns>The configured <see cref="IServiceCollection"/></returns>
    public static IServiceCollection AddA2AWellKnownAgent(this IServiceCollection services, Action<IServiceProvider, IAgentCardBuilder> setup)
    {
        ArgumentNullException.ThrowIfNull(setup);
        return services.AddSingleton(provider =>
        {
            var builder = new AgentCardBuilder();
            setup(provider, builder);
            return builder.Build();
        });
    }

    /// <summary>
    /// Adds and configures a new <see cref="IA2AProtocolServer"/>
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> to configure</param>
    /// <param name="name">The name of the <see cref="IA2AProtocolServer"/> to add and configure</param>
    /// <param name="setup">An <see cref="Action{T}"/> used to setup the <see cref="IA2AProtocolServer"/> to use</param>
    /// <returns>The configured <see cref="IServiceCollection"/></returns>
    public static IServiceCollection AddA2AProtocolServer(this IServiceCollection services, string name, Action<IA2AProtocolServerBuilder> setup)
    {
        ArgumentNullException.ThrowIfNull(setup);
        services.AddHttpClient();
        services.TryAddSingleton<IJsonWebKeySet, JsonWebKeySet>();
        services.TryAddTransient<IA2AProtocolServerProvider, A2AProtocolServerProvider>();
        var builder = new A2AProtocolServerBuilder(name, services);
        setup(builder);
        return builder.Build();
    }

    /// <summary>
    /// Adds and configures a new <see cref="IA2AProtocolServer"/>
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> to configure</param>
    /// <param name="setup">An <see cref="Action{T}"/> used to setup the <see cref="IA2AProtocolServer"/> to use</param>
    /// <returns>The configured <see cref="IServiceCollection"/></returns>
    public static IServiceCollection AddA2AProtocolServer(this IServiceCollection services, Action<IA2AProtocolServerBuilder> setup) => services.AddA2AProtocolServer(A2AProtocolServer.DefaultName, setup);

}
