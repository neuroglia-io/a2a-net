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
namespace A2A.Server;

/// <summary>
/// Defines extensions for <see cref="IA2AServerBuilder"/>s.
/// </summary>
public static class RedisStateStoreA2AServerBuilderExtensions
{

    /// <summary>
    /// Configures the <see cref="IA2AServer"/> to use the <see cref="RedisStore"/> as its <see cref="IA2AStore"/>.
    /// </summary>
    /// <param name="builder">The <see cref="IA2AServerBuilder"/> to configure.</param>
    /// <param name="configure">An <see cref="Action{T}"/>, if any, used to configure the <see cref="RedisStateStoreOptions"/> to use.</param>
    /// <returns>The configured <see cref="IA2AServerBuilder"/>.</returns>
    public static IA2AServerBuilder UseRedisStore(this IA2AServerBuilder builder, Action<RedisStateStoreOptions>? configure = null)
    {
        ArgumentNullException.ThrowIfNull(builder);
        if (configure is not null) builder.Services.Configure(configure);
        builder.UseStore<RedisStore>();
        return builder;
    }

}
