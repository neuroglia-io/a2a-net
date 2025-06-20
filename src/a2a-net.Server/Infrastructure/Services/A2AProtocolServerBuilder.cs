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

namespace A2A.Server.Infrastructure.Services;

/// <summary>
/// Represents the default implementation of the <see cref="IA2AProtocolServerBuilder"/>
/// </summary>
/// <param name="name">The name of the <see cref="IA2AProtocolServer"/> to build</param>
/// <param name="services">The <see cref="IServiceCollection"/> to configure</param>
public class A2AProtocolServerBuilder(string name, IServiceCollection services)
    : IA2AProtocolServerBuilder
{

    /// <summary>
    /// Gets the name of the <see cref="IA2AProtocolServer"/> to build
    /// </summary>
    protected string Name { get; } = name;

    /// <summary>
    /// Gets the <see cref="IServiceCollection"/> to configure
    /// </summary>
    protected IServiceCollection Services { get; } = services;

    /// <summary>
    /// Gets the capabilities of the <see cref="IA2AProtocolServer"/> to build
    /// </summary>
    protected AgentCapabilities Capabilities { get; } = new();

    /// <summary>
    /// Gets or sets the lifetime of the <see cref="IA2AProtocolServer"/> to build
    /// </summary>
    protected ServiceLifetime Lifetime { get; set; } = ServiceLifetime.Singleton;

    /// <summary>
    /// Gets the type of the <see cref="IAgentRuntime"/> to use
    /// </summary>
    protected Type? AgentRuntimeType { get; set; }

    /// <summary>
    /// Gets a <see cref="Func{T, TResult}"/> used to create the <see cref="IAgentRuntime"/> to use
    /// </summary>
    protected Func<IServiceProvider, IAgentRuntime>? AgentRuntimeFactory { get; set; }

    /// <summary>
    /// Gets the type of the <see cref="ITaskEventStream"/> to use
    /// </summary>
    protected Type TaskEventStreamType { get; set; } = typeof(TaskEventStream);

    /// <summary>
    /// Gets the type of the <see cref="ITaskHandler"/> to use
    /// </summary>
    protected Type TaskHandlerType { get; set; } = typeof(TaskHandler);

    /// <summary>
    /// Gets the type of <see cref="ITaskRepository"/> to use
    /// </summary>
    protected Type? TaskRepositoryType { get; set; }

    /// <summary>
    /// Gets the type of <see cref="IPushNotificationSender"/> to use
    /// </summary>
    protected Type PushNotificationSenderType { get; set; } = typeof(PushNotificationSender);

    /// <summary>
    /// Gets the type of the <see cref="IA2AProtocolServer"/> to build
    /// </summary>
    protected Type ServerType { get; set; } = typeof(A2AProtocolServer);

    /// <inheritdoc/>
    public virtual IA2AProtocolServerBuilder WithLifetime(ServiceLifetime lifetime)
    {
        Lifetime = lifetime;
        return this;
    }

    /// <inheritdoc/>
    public virtual IA2AProtocolServerBuilder SupportsStreaming()
    {
        Capabilities.Streaming = true;
        return this;
    }

    /// <inheritdoc/>
    public virtual IA2AProtocolServerBuilder SupportsPushNotifications()
    {
        Capabilities.PushNotifications = true;
        return this;
    }

    /// <inheritdoc/>
    public virtual IA2AProtocolServerBuilder SupportsStateTransitionHistory()
    {
        Capabilities.StateTransitionHistory = true;
        return this;
    }

    /// <inheritdoc/>
    public virtual IA2AProtocolServerBuilder UseAgentRuntime<TRuntime>() 
        where TRuntime : class, IAgentRuntime
    {
        AgentRuntimeType = typeof(TRuntime);
        return this;
    }

    /// <inheritdoc/>
    public virtual IA2AProtocolServerBuilder UseAgentRuntime(Func<IServiceProvider, IAgentRuntime> factory)
    {
        AgentRuntimeFactory = factory;
        return this;
    }

    /// <inheritdoc/>
    public virtual IA2AProtocolServerBuilder UseTaskEventStream<TStream>() 
        where TStream : class, ITaskEventStream
    {
        TaskEventStreamType = typeof(TStream);
        return this;
    }

    /// <inheritdoc/>
    public virtual IA2AProtocolServerBuilder UseTaskHandler<THandler>() 
        where THandler : class, ITaskHandler
    {
        TaskHandlerType = typeof(THandler);
        return this;
    }

    /// <inheritdoc/>
    public virtual IA2AProtocolServerBuilder UseTaskRepository<TRepository>() 
        where TRepository : class, ITaskRepository
    {
        TaskRepositoryType = typeof(TRepository);
        return this;
    }

    /// <inheritdoc/>
    public virtual IA2AProtocolServerBuilder UsePushNotificationSender<TSender>()
        where TSender : class, IPushNotificationSender
    {
        PushNotificationSenderType = typeof(TSender);
        return this;
    }

    /// <inheritdoc/>
    public virtual IA2AProtocolServerBuilder OfType<TServer>() 
        where TServer : class, IA2AProtocolServer
    {
        ServerType = typeof(TServer);
        return this;
    }

    /// <inheritdoc/>
    public virtual IServiceCollection Build()
    {
        if (AgentRuntimeType == null && AgentRuntimeFactory == null) throw new NullReferenceException("The agent runtime must be configured");
        if (TaskRepositoryType == null) throw new NullReferenceException("The task repository type must be configured");
        Func<IServiceProvider, object?, object> factory = (provider, key) =>
        {
            var agentRuntime = AgentRuntimeType == null ? AgentRuntimeFactory!.Invoke(provider) : ActivatorUtilities.CreateInstance(provider, AgentRuntimeType);
            var taskEventStream = ActivatorUtilities.CreateInstance(provider, TaskEventStreamType);
            var taskRepository = ActivatorUtilities.CreateInstance(provider, TaskRepositoryType);
            var pushNotificationSender = ActivatorUtilities.CreateInstance(provider, PushNotificationSenderType);
            var taskHandler = ActivatorUtilities.CreateInstance(provider, TaskHandlerType, agentRuntime, taskEventStream, taskRepository, pushNotificationSender);
            return ActivatorUtilities.CreateInstance(provider, ServerType, Name, Capabilities, taskEventStream, taskHandler, taskRepository, pushNotificationSender);
        };
        Services.Add(new(typeof(IA2AProtocolServer), Name, factory, Lifetime));
        return Services;
    }

}