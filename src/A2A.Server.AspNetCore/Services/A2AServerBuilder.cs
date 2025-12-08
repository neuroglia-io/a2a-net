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

#pragma warning disable CS0618 // Type or member is obsolete

using A2A.Models;
using System.Diagnostics.CodeAnalysis;

namespace A2A.Server.Services;

/// <summary>
/// Represents the default implementation of the <see cref="IA2AServerBuilder"/>
/// </summary>
/// <param name="services">The <see cref="IServiceCollection"/> to configure</param>
public sealed class A2AServerBuilder(IServiceCollection services)
    : IA2AServerBuilder
{

    AgentCapabilities capabilities = new();
    Type taskEventStreamType = typeof(A2ATaskEventStream);
    Type? storeType;
    Type? taskQueueType;
    Type pushNotificationSenderType = typeof(A2APushNotificationSender);
    Type serverType = typeof(A2AServer);
    A2AHostedAgentDefinition? agentDefinition;
    readonly List<TransportDefinition> transports = [];

    /// <inheritdoc/>
    public IServiceCollection Services { get; } = services;

    /// <inheritdoc/>
    public IA2AServerBuilder SupportsStreaming()
    {
        capabilities = capabilities with
        {
            Streaming = true
        };
        return this;
    }

    /// <inheritdoc/>
    public IA2AServerBuilder SupportsPushNotifications()
    {
        capabilities = capabilities with
        {
            PushNotifications = true
        };
        return this;
    }

    /// <inheritdoc/>
    public IA2AServerBuilder SupportsStateTransitionHistory()
    {
        capabilities = capabilities with
        {
            StateTransitionHistory = true
        };
        return this;
    }

    /// <inheritdoc/>
    public IA2AServerBuilder UseTaskEventStream<TStream>()
        where TStream : class, IA2ATaskEventStream
    {
        taskEventStreamType = typeof(TStream);
        return this;
    }

    /// <inheritdoc/>
    public IA2AServerBuilder UseStore<TStore>()
        where TStore : class, IA2AStore
    {
        storeType = typeof(TStore);
        return this;
    }

    /// <inheritdoc/>
    public IA2AServerBuilder UseTaskQueue<TQueue>()
        where TQueue : class, IA2ATaskQueue
    {
        taskQueueType = typeof(TQueue);
        return this;
    }

    /// <inheritdoc/>
    public IA2AServerBuilder UseTransport<TTransport>(string protocolBinding, [StringSyntax("Route")] string path)
        where TTransport : class, IA2AServerTransport
    {
        ArgumentNullException.ThrowIfNull(protocolBinding);
        ArgumentNullException.ThrowIfNull(path);
        transports.Add(new(typeof(TTransport), protocolBinding, new(path, UriKind.Relative)));
        return this;
    }

    /// <inheritdoc/>
    public IA2AServerBuilder UsePushNotificationSender<TSender>()
        where TSender : class, IA2APushNotificationSender
    {
        pushNotificationSenderType = typeof(TSender);
        return this;
    }

    /// <inheritdoc/>
    public IA2AServerBuilder Host(Action<IA2AHostedAgentDefinitionBuilder> setup)
    {
        var hostedAgentDefinitionBuilder = new A2AHostedAgentDefinitionBuilder();
        setup(hostedAgentDefinitionBuilder);
        agentDefinition = hostedAgentDefinitionBuilder.Build();
        return this;
    }

    /// <inheritdoc/>
    public IA2AServerBuilder OfType<TServer>()
        where TServer : class, IA2AServer
    {
        serverType = typeof(TServer);
        return this;
    }

    /// <inheritdoc/>
    public IServiceCollection Build()
    {
        var env = Environment.GetEnvironmentVariable(A2AServerDefaults.EnvironmentVariables.ServerAddress);
        if (string.IsNullOrWhiteSpace(env)) throw new NullReferenceException("The server address must be configured using environment variables");
        if(!Uri.TryCreate(env, UriKind.Absolute, out var serverAddress)) throw new FormatException($"The specified server address '{env}' is not a valid absolute URI");
        if (storeType == null) throw new NullReferenceException("The state store type must be configured");
        if (taskQueueType == null) throw new NullReferenceException("The task queue type must be configured");
        if (agentDefinition == null) throw new NullReferenceException("The hosted agent must be configured");
        if (transports.Count < 1) throw new NullReferenceException("At least one transport must be configured");
        var interfaces = transports.Select(transport => new AgentInterface()
        {
            ProtocolBinding = transport.ProtocolBinding,
            Url = new(serverAddress, transport.Uri)
        });
        Services.AddSingleton(typeof(IA2ATaskEventStream), taskEventStreamType);
        Services.AddSingleton(typeof(IA2AStore), storeType);
        Services.AddSingleton(typeof(IA2ATaskQueue), taskQueueType);
        Services.AddSingleton(typeof(IA2APushNotificationSender), pushNotificationSenderType);
        Services.AddSingleton(typeof(IA2AServer), serverType);
        Services.AddKeyedSingleton<AgentCard>(null, agentDefinition.Card with
        {
            SupportedInterfaces = [.. interfaces]
        });
        if (agentDefinition.ExtendedCard is not null) Services.AddKeyedSingleton(A2AServerDefaults.ExtendedAgentCardServiceKey, agentDefinition.ExtendedCard with
        {
            SupportedInterfaces = [.. interfaces]
        });
        Services.AddSingleton(typeof(IA2AAgentRuntime), agentDefinition.RuntimeType);
        transports.ToList().ForEach(transport => Services.AddKeyedScoped(typeof(IA2AServerTransport), transport.ProtocolBinding, transport.Type));
        return Services;
    }

    sealed record TransportDefinition(Type Type, string ProtocolBinding, Uri Uri);

}
