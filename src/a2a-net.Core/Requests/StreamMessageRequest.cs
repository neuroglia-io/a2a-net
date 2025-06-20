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

namespace A2A.Requests;

/// <summary>
/// Represents a request used to send a message to an agent to initiate/continue a task AND subscribes the client to real-time updates for that task via Server-Sent Events (SSE).<para></para>
/// Requires the server to have streaming capabilities.
/// </summary>
[Description("Represents a request used to send a message to an agent to initiate/continue a task AND subscribes the client to real-time updates for that task via Server-Sent Events (SSE). Requires the server to have streaming capabilities.")]
[DataContract]
public record StreamMessageRequest
    : RpcRequest<SendMessageRequestParameters>
{

    /// <summary>
    /// The name of the request's RPC method.
    /// </summary>
    public const string RpcMethodName = "message/stream";

    /// <inheritdoc/>
    public StreamMessageRequest() : base(A2AProtocol.Methods.Messages.Stream) { }

    /// <inheritdoc/>
    public StreamMessageRequest(SendMessageRequestParameters parameters) : base(A2AProtocol.Methods.Messages.Stream, parameters) { }

}