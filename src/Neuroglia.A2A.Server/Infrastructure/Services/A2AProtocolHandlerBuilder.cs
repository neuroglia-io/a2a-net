namespace Neuroglia.A2A.Server.Infrastructure.Services;

/// <summary>
/// Represents the default implementation of the <see cref="IA2AProtocolHandlerBuilder"/>
/// </summary>
/// <param name="services">The <see cref="IServiceCollection"/> to configure</param>
public class A2AProtocolHandlerBuilder(IServiceCollection services)
    : IA2AProtocolHandlerBuilder
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
    /// Gets the type of the <see cref="IA2AProtocolHandler"/> to build
    /// </summary>
    protected Type ProtocolHandlerType { get; set; } = typeof(A2AProtocolHandler);

    /// <inheritdoc/>
    public virtual IA2AProtocolHandlerBuilder UseAgentRuntime<TRuntime>(ServiceLifetime serviceLifetime = ServiceLifetime.Singleton) 
        where TRuntime : IAgentRuntime
    {
        AgentRuntime = new(typeof(IAgentRuntime), typeof(TRuntime), serviceLifetime);
        return this;
    }

    /// <inheritdoc/>
    public virtual IA2AProtocolHandlerBuilder UseTaskEventStream<TStream>(ServiceLifetime serviceLifetime = ServiceLifetime.Singleton) 
        where TStream : ITaskEventStream
    {
        TaskEventStream = new(typeof(ITaskEventStream), typeof(TStream), serviceLifetime);
        return this;
    }

    /// <inheritdoc/>
    public virtual IA2AProtocolHandlerBuilder UseTaskHandler<THandler>(ServiceLifetime serviceLifetime = ServiceLifetime.Singleton) 
        where THandler : ITaskHandler
    {
        TaskHandler = new(typeof(ITaskHandler), typeof(THandler), serviceLifetime);
        return this;
    }

    /// <inheritdoc/>
    public virtual IA2AProtocolHandlerBuilder UseTaskRepository<TRepository>(ServiceLifetime serviceLifetime = ServiceLifetime.Singleton) 
        where TRepository : ITaskRepository
    {
        TaskRepository = new(typeof(ITaskRepository), typeof(TRepository), serviceLifetime);
        return this;
    }

    /// <inheritdoc/>
    public virtual IA2AProtocolHandlerBuilder OfType<THandler>() 
        where THandler : IA2AProtocolHandler
    {
        ProtocolHandlerType = typeof(THandler);
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
        Services.Add(new(typeof(IA2AProtocolHandler), ProtocolHandlerType, ServiceLifetime.Singleton));
        return Services;
    }

}