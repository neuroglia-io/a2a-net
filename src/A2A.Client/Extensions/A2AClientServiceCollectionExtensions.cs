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

#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace A2A.Client;

/// <summary>
/// Defines extensions for <see cref="IServiceCollection"/>s.
/// </summary>
public static class A2AClientServiceCollectionExtensions
{

    /// <summary>
    /// Adds and configures an <see cref="IA2AClient"/>.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> to configure.</param>
    /// <param name="setup">An <see cref="Action{T}"/> used to setup the <see cref="IA2AClient"/>.</param>
    /// <returns>The configured <see cref="IServiceCollection"/>.</returns>
    public static IServiceCollection AddA2AClient(this IServiceCollection services, Action<IA2AClientBuilder> setup)
    {
        var builder = new A2AClientBuilder(services);
        setup.Invoke(builder);
        services.AddSingleton<IA2AClient, A2AClient>();
        return services;
    }

}
