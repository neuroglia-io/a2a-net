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
/// <param name="services">The <see cref="IServiceCollection"/> to configure</param>
public class A2AProtocolServerBuilder(IServiceCollection services)
    : IA2AProtocolServerBuilder
{

    /// <summary>
    /// Gets the <see cref="IServiceCollection"/> to configure
    /// </summary>
    protected IServiceCollection Services { get; } = services;

    /// <summary>
    /// Gets a <see cref="ServiceDescriptor"/> used to describe the <see cref="IAgentRuntime"/> to use
    /// </summary>
    protected ServiceDescriptor? AgentRuntime { get; set; }

    /// <summary>
    /// Gets a <see cref="ServiceDescriptor"/> used to describe the <see cref="ITaskEventStream"/> to use
    /// </summary>
    protected ServiceDescriptor TaskEventStream { get; set; } = new(typeof(ITaskEventStream), typeof(TaskEventStream), ServiceLifetime.Singleton);

    /// <summary>
    /// Gets a <see cref="ServiceDescriptor"/> used to describe the <see cref="ITaskHandler"/> to use
    /// </summary>
    protected ServiceDescriptor TaskHandler { get; set; } = new(typeof(ITaskHandler), typeof(TaskHandler), ServiceLifetime.Singleton);

    /// <summary>
    /// Gets a <see cref="ServiceDescriptor"/> used to describe the <see cref="ITaskRepository"/> to use
    /// </summary>
    protected ServiceDescriptor? TaskRepository { get; set; }

    /// <summary>
    /// Gets the type of the <see cref="IA2AProtocolServer"/> to build
    /// </summary>
    protected Type ServerType { get; set; } = typeof(A2AProtocolServer);

    /// <inheritdoc/>
    public virtual IA2AProtocolServerBuilder UseAgentRuntime<TRuntime>(ServiceLifetime serviceLifetime = ServiceLifetime.Singleton) 
        where TRuntime : class, IAgentRuntime
    {
        AgentRuntime = new(typeof(IAgentRuntime), typeof(TRuntime), serviceLifetime);
        return this;
    }

    /// <inheritdoc/>
    public virtual IA2AProtocolServerBuilder UseTaskEventStream<TStream>(ServiceLifetime serviceLifetime = ServiceLifetime.Singleton) 
        where TStream : class, ITaskEventStream
    {
        TaskEventStream = new(typeof(ITaskEventStream), typeof(TStream), serviceLifetime);
        return this;
    }

    /// <inheritdoc/>
    public virtual IA2AProtocolServerBuilder UseTaskHandler<THandler>(ServiceLifetime serviceLifetime = ServiceLifetime.Singleton) 
        where THandler : class, ITaskHandler
    {
        TaskHandler = new(typeof(ITaskHandler), typeof(THandler), serviceLifetime);
        return this;
    }

    /// <inheritdoc/>
    public virtual IA2AProtocolServerBuilder UseTaskRepository<TRepository>(ServiceLifetime serviceLifetime = ServiceLifetime.Singleton) 
        where TRepository : class, ITaskRepository
    {
        TaskRepository = new(typeof(ITaskRepository), typeof(TRepository), serviceLifetime);
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
        if (AgentRuntime == null) throw new NullReferenceException("The agent runtime must be configured");
        if (TaskRepository == null) throw new NullReferenceException("The task repository must be configured");
        Services.Add(AgentRuntime);
        Services.Add(TaskEventStream);
        Services.Add(TaskHandler);
        Services.Add(TaskRepository);
        Services.Add(new(typeof(IA2AProtocolServer), ServerType, ServiceLifetime.Singleton));
        return Services;
    }

}