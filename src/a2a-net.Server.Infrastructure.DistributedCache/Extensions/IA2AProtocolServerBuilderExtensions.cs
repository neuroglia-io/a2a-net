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

using A2A.Server.Infrastructure.Services;

namespace A2A.Server.Infrastructure;

/// <summary>
/// Defines extensions for <see cref="IA2AProtocolServerBuilder"/>s
/// </summary>
public static class IA2AProtocolServerBuilderExtensions
{

    /// <summary>
    /// Configures the <see cref="IA2AProtocolServerBuilder"/> to use the <see cref="DistributedCacheTaskRepository"/>
    /// </summary>
    /// <param name="builder">The <see cref="IA2AProtocolServerBuilder"/> to configure</param>
    /// <returns>The configured <see cref="IA2AProtocolServerBuilder"/></returns>
    public static IA2AProtocolServerBuilder UseDistributedCacheTaskRepository(this IA2AProtocolServerBuilder builder)
    {
        builder.UseTaskRepository<DistributedCacheTaskRepository>();
        return builder;
    }

}
