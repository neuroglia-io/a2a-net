﻿// Copyright © 2025-Present the a2a-net Authors
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
/// Represents the request used to resubscribe to a remote agent.
/// </summary>
[Description("Represents the request used to resubscribe to a remote agent.")]
[DataContract]
public record ResubscribeToTaskRequest
    : RpcRequest<TaskQueryParameters>
{

    /// <inheritdoc/>
    public ResubscribeToTaskRequest() : base(A2AProtocol.Methods.Tasks.Resubscribe) { }

    /// <inheritdoc/>
    public ResubscribeToTaskRequest(TaskQueryParameters parameters) : base(A2AProtocol.Methods.Tasks.Resubscribe, parameters) { }

}