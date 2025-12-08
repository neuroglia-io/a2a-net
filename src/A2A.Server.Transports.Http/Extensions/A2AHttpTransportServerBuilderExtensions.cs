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

using A2A.Server.Transports;
using System.Diagnostics.CodeAnalysis;

namespace A2A.Server;

/// <summary>
/// Defines extensions for <see cref="IA2AServerBuilder"/>s.
/// </summary>
public static class A2AHttpTransportServerBuilderExtensions
{

    /// <summary>
    /// Configures the server to use the HTTP transport.
    /// </summary>
    /// <param name="builder">The <see cref="IA2AServerBuilder"/> to configure.</param>
    /// <param name="path">The path, if any, at which the transport is made available.</param>
    /// <returns>The configured <see cref="IA2AServerBuilder"/>.</returns>
    public static IA2AServerBuilder UseHttpTransport(this IA2AServerBuilder builder, [StringSyntax("Route")] string path = "/")
    {
        builder.UseTransport<A2AHttpServerTransport>(ProtocolBinding.Http, path);
        return builder;
    }

}
