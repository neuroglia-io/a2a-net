namespace Neuroglia.A2A.Server;

/// <summary>
/// Defines extensions for <see cref="IServiceCollection"/>s
/// </summary>
public static class IServiceCollectionExtensions
{

    /// <summary>
    /// Adds and configures a new <see cref="IA2AProtocolHandler"/>
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> to configure</param>
    /// <param name="setup">An <see cref="Action{T}"/> used to setup the <see cref="IA2AProtocolHandler"/> to use</param>
    /// <returns>The configured <see cref="IServiceCollection"/></returns>
    public static IServiceCollection AddA2AProtocolHandler(this IServiceCollection services, Action<IA2AProtocolHandlerBuilder> setup)
    {
        var builder = new A2AProtocolHandlerBuilder(services);
        setup(builder);
        return builder.Build();
    }

}
