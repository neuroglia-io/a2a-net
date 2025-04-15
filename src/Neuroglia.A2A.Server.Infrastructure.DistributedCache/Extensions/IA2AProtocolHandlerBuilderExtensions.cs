using Neuroglia.A2A.Server.Infrastructure.Services;

namespace Neuroglia.A2A.Server.Infrastructure;

/// <summary>
/// Defines extensions for <see cref="IA2AProtocolHandlerBuilder"/>s
/// </summary>
public static class IA2AProtocolHandlerBuilderExtensions
{

    /// <summary>
    /// Configures the <see cref="IA2AProtocolHandlerBuilder"/> to use the <see cref="DistributedCacheTaskRepository"/>
    /// </summary>
    /// <param name="builder">The <see cref="IA2AProtocolHandlerBuilder"/> to configure</param>
    /// <returns>The configured <see cref="IA2AProtocolHandlerBuilder"/></returns>
    public static IA2AProtocolHandlerBuilder UseDistributedCacheTaskRepository(this IA2AProtocolHandlerBuilder builder)
    {
        builder.UseTaskRepository<DistributedCacheTaskRepository>();
        return builder;
    }

}
