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

namespace A2A.Server;

/// <summary>
/// Defines extensions for <see cref="IServiceCollection"/>s.
/// </summary>
public static class IServiceCollectionExtensions
{

    /// <summary>
    /// Adds and configures a new Microsoft Semantic Kernel based <see cref="IAgentRuntime"/>.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> to configure.</param>
    /// <param name="configureRuntime">An <see cref="Action{T}"/>, if any, used to configure the <see cref="SemanticKernelAgentRuntimeOptions"/> to use.</param>
    /// <param name="configureKernel">An <see cref="Action{T}"/>, if any, used to configure the <see cref="IKernelBuilder"/> to use.</param>
    /// <param name="serviceKey">The key of the <see cref="SemanticKernelAgentRuntime"/> to add.</param>
    /// <param name="serviceLifetime">The <see cref="SemanticKernelAgentRuntime"/>'s <see cref="ServiceLifetime"/>.</param>
    /// <returns>The configured <see cref="IServiceCollection"/>.</returns>
    public static IServiceCollection AddSemanticKernelAgentRuntime(this IServiceCollection services, Action<SemanticKernelAgentRuntimeOptions>? configureRuntime = null, Action<IKernelBuilder>? configureKernel = null, string? serviceKey = null, ServiceLifetime serviceLifetime = ServiceLifetime.Transient)
    {
        if (configureRuntime != null) services.Configure(serviceKey, configureRuntime);
        if (configureKernel != null)
        {
            var kernelBuilder = services.AddKernel();
            configureKernel(kernelBuilder);
        }
        services.TryAdd(new ServiceDescriptor(typeof(IAgentRuntime), serviceKey, typeof(SemanticKernelAgentRuntime), serviceLifetime));
        return services;
    }

}
