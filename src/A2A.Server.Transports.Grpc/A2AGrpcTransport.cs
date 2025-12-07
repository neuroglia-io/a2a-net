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

namespace A2A.Server.Transports;

/// <summary>
/// Represents the gRPC implementation of the <see cref="IA2ATransport"/> interface.
/// </summary>
public class A2AGrpcTransport
    : IA2ATransport
{

    /// <inheritdoc/>
    public Task<IResult> HandleAsync(HttpContext httpContext)
    {
        return Task.FromResult(Results.Problem(new()
        {
            Type = "https://a2a-net.github.io/docs/errors/not-implemented",
            Title = "Not Implemented",
            Detail = "gRPC transport is not implemented via IA2ATransport.HandleAsync. Register the gRPC service with app.MapGrpcService<A2AGrpcService>().",
            Status = StatusCodes.Status501NotImplemented,
        }));
    }

}
