namespace Neuroglia.A2A.Server.Infrastructure.Services;

/// <summary>
/// Defines the fundamentals of a service used to build and configure an A2A protocol server
/// </summary>
public interface IA2AProtocolHandlerBuilder
{

    /// <summary>
    /// Configures the server to use the specified <see cref="IAgentRuntime"/>
    /// </summary>
    /// <typeparam name="TRuntime">The type of <see cref="IAgentRuntime"/> to use</typeparam>
    /// <param name="serviceLifetime">The <see cref="IAgentRuntime"/>'s service lifetime</param>
    /// <returns>The configured <see cref="IA2AProtocolHandlerBuilder"/></returns>
    IA2AProtocolHandlerBuilder UseAgentRuntime<TRuntime>(ServiceLifetime serviceLifetime = ServiceLifetime.Singleton)
        where TRuntime : IAgentRuntime;

    /// <summary>
    /// Configures the server to use the specified <see cref="ITaskEventStream"/>
    /// </summary>
    /// <typeparam name="TStream">The type of <see cref="ITaskEventStream"/> to use</typeparam>
    /// <param name="serviceLifetime">The <see cref="ITaskEventStream"/>'s service lifetime</param>
    /// <returns>The configured <see cref="IA2AProtocolHandlerBuilder"/></returns>
    IA2AProtocolHandlerBuilder UseTaskEventStream<TStream>(ServiceLifetime serviceLifetime = ServiceLifetime.Singleton)
        where TStream : ITaskEventStream;

    /// <summary>
    /// Configures the server to use the specified <see cref="ITaskRepository"/>
    /// </summary>
    /// <typeparam name="TRepository">The type of <see cref="ITaskRepository"/> to use</typeparam>
    /// <param name="serviceLifetime">The <see cref="ITaskRepository"/>'s service lifetime</param>
    /// <returns>The configured <see cref="IA2AProtocolHandlerBuilder"/></returns>
    IA2AProtocolHandlerBuilder UseTaskRepository<TRepository>(ServiceLifetime serviceLifetime = ServiceLifetime.Singleton)
        where TRepository : ITaskRepository;

    /// <summary>
    /// Configures the server to use the specified <see cref="ITaskHandler"/>
    /// </summary>
    /// <typeparam name="THandler">The type of <see cref="ITaskHandler"/> to use</typeparam>
    /// <param name="serviceLifetime">The <see cref="ITaskHandler"/>'s service lifetime</param>
    /// <returns>The configured <see cref="IA2AProtocolHandlerBuilder"/></returns>
    IA2AProtocolHandlerBuilder UseTaskHandler<THandler>(ServiceLifetime serviceLifetime = ServiceLifetime.Singleton)
        where THandler : ITaskHandler;

    /// <summary>
    /// Configures the type of the <see cref="IA2AProtocolHandler"/> to build
    /// </summary>
    /// <typeparam name="THandler">The type of <see cref="IA2AProtocolHandler"/> to build</typeparam>
    /// <returns>The configured <see cref="IA2AProtocolHandlerBuilder"/></returns>
    IA2AProtocolHandlerBuilder OfType<THandler>()
        where THandler : IA2AProtocolHandler;

    /// <summary>
    /// Registers the configured services and builds the <see cref="IA2AProtocolHandler"/>
    /// </summary>
    /// <returns>The configured <see cref="IServiceCollection"/></returns>
    IServiceCollection Build();

}
