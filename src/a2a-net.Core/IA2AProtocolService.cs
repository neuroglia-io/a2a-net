// Copyright � 2025-Present the a2a-net Authors
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

namespace A2A;

/// <summary>
/// Defines the shared contract of the Agent-to-Agent (A2A) protocol, used by both clients and agents to exchange tasks, messages, and events
/// </summary>
public interface IA2AProtocolService
{

    /// <summary>
    /// Sends content to a remote agent to start a new Task, resumes an interrupted Task or reopens a completed Task<para></para>
    /// A Task interrupt may be caused due to an agent requiring additional user input or a runtime error
    /// </summary>
    /// <param name="request">The request to perform</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/></param>
    /// <returns>A new <see cref="RpcResponse"/> that describes the result of the operation</returns>
    [JsonRpcMethod(A2AProtocol.Methods.Tasks.Send)]
    Task<RpcResponse<Models.Task>> SendTaskAsync(SendTaskRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves the currently configured push notification configuration for a Task
    /// </summary>
    /// <param name="request">The request to perform</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/></param>
    /// <returns>A new <see cref="IAsyncEnumerable{T}"/> used to stream the task's events</returns>
    [JsonRpcMethod(A2AProtocol.Methods.Tasks.SendSubscribe)]
    IAsyncEnumerable<RpcResponse<TaskEvent>> SendTaskStreamingAsync(SendTaskStreamingRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Resubscribes to a remote agent
    /// </summary>
    /// <param name="request">The request to perform</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/></param>
    /// <returns>A new <see cref="IAsyncEnumerable{T}"/> used to stream the task's events</returns>
    [JsonRpcMethod(A2AProtocol.Methods.Tasks.Resubscribe)]
    IAsyncEnumerable<RpcResponse<TaskEvent>> ResubscribeToTaskAsync(TaskResubscriptionRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves the generated Artifacts for a Task
    /// </summary>
    /// <param name="request">The request to perform</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/></param>
    /// <returns>A new <see cref="RpcResponse"/> that describes the result of the operation</returns>
    [JsonRpcMethod(A2AProtocol.Methods.Tasks.Get)]
    Task<RpcResponse<Models.Task>> GetTaskAsync(GetTaskRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Cancels a previously submitted task
    /// </summary>
    /// <param name="request">The request to perform</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/></param>
    /// <returns>A new <see cref="RpcResponse"/> that describes the result of the operation</returns>
    [JsonRpcMethod(A2AProtocol.Methods.Tasks.Cancel)]
    Task<RpcResponse<Models.Task>> CancelTaskAsync(CancelTaskRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Configures a push notification URL for receiving an update on Task status change
    /// </summary>
    /// <param name="request">The request to perform</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/></param>
    /// <returns>A new <see cref="RpcResponse"/> that describes the result of the operation</returns>
    [JsonRpcMethod(A2AProtocol.Methods.Tasks.PushNotifications.Set)]
    Task<RpcResponse<TaskPushNotificationConfiguration>> SetTaskPushNotificationsAsync(SetTaskPushNotificationsRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves the currently configured push notification configuration for a Task
    /// </summary>
    /// <param name="request">The request to perform</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/></param>
    /// <returns>A new <see cref="RpcResponse"/> that describes the result of the operation</returns>
    [JsonRpcMethod(A2AProtocol.Methods.Tasks.PushNotifications.Get)]
    Task<RpcResponse<TaskPushNotificationConfiguration>> GetTaskPushNotificationsAsync(GetTaskPushNotificationsRequest request, CancellationToken cancellationToken = default);

}
