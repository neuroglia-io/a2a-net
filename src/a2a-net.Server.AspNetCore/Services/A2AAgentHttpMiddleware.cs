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

using Microsoft.VisualStudio.Threading;

namespace A2A.Server.AspNetCore.Services;

/// <summary>
/// Represents the middleware that handles HTTP-based JSON-RPC requests for an A2A agent
/// </summary>
public class A2AAgentHttpMiddleware
{

    readonly IA2AProtocolServerProvider _serverProvider;

    /// <summary>
    /// Initializes a new <see cref="A2AAgentHttpMiddleware"/>
    /// </summary>
    /// <param name="serverProvider">The service used to provide <see cref="IA2AProtocolServer"/>s</param>
    public A2AAgentHttpMiddleware(IA2AProtocolServerProvider serverProvider)
    {
        _serverProvider = serverProvider;
    }

    /// <summary>
    /// Invokes the <see cref="A2AAgentHttpMiddleware"/>
    /// </summary>
    /// <param name="context">The current <see cref="HttpContext"/></param>
    /// <returns>A new awaitable <see cref="System.Threading.Tasks.Task"/></returns>
    public async System.Threading.Tasks.Task InvokeAsync(HttpContext context)
    {
        if (!HttpMethods.IsPost(context.Request.Method)) return;
        var request = await JsonSerializer.DeserializeAsync<RpcRequest>(context.Request.Body, cancellationToken: context.RequestAborted);
        if (request == null)
        {
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            await WriteJsonResponseAsync(new InvalidRequestError()).ConfigureAwait(false);
            return;
        }
        var serverName = context.Request.RouteValues.TryGetValue(A2AEndpointRouteBuilderExtensions.ServerVariableName, out var value) && value is string name && !string.IsNullOrWhiteSpace(name) ? name : A2AProtocolServer.DefaultName;
        var server = _serverProvider.Get(serverName);
        switch (request.Method)
        {
            case A2AProtocol.Methods.Tasks.Send:
                if (!TryConvertTo(request, out SendTaskRequest sendTaskRequest))
                {
                    await InvalidParams().ConfigureAwait(false);
                    return;
                }
                await WriteJsonResponseAsync(await server.SendTaskAsync(sendTaskRequest, context.RequestAborted).ConfigureAwait(false)).ConfigureAwait(false);
                break;

            case A2AProtocol.Methods.Tasks.SendSubscribe:
                if (!server.Capabilities.Streaming)
                {
                    await UnsupportedOperation();
                    return;
                }
                if (!TryConvertTo(request, out SendTaskStreamingRequest sendTaskStreamingRequest))
                {
                    await InvalidParams().ConfigureAwait(false);
                    return;
                }
                await StreamTaskEventsAsync(context, server.SendTaskStreamingAsync(sendTaskStreamingRequest, context.RequestAborted));
                break;

            case A2AProtocol.Methods.Tasks.Resubscribe:
                if (!server.Capabilities.Streaming)
                {
                    await UnsupportedOperation();
                    return;
                }
                if (!TryConvertTo(request, out TaskResubscriptionRequest resubscribeTaskRequest))
                {
                    await InvalidParams().ConfigureAwait(false);
                    return;
                }
                await StreamTaskEventsAsync(context, server.ResubscribeToTaskAsync(resubscribeTaskRequest, context.RequestAborted));
                break;

            case A2AProtocol.Methods.Tasks.Get:
                if (!TryConvertTo(request, out GetTaskRequest getTaskRequest))
                {
                    await InvalidParams().ConfigureAwait(false);
                    return;
                }
                await WriteJsonResponseAsync(await server.GetTaskAsync(getTaskRequest, context.RequestAborted).ConfigureAwait(false)).ConfigureAwait(false);
                break;

            case A2AProtocol.Methods.Tasks.Cancel:
                if (!TryConvertTo(request, out CancelTaskRequest cancelTaskRequest))
                {
                    await InvalidParams().ConfigureAwait(false);
                    return;
                }
                await WriteJsonResponseAsync(await server.CancelTaskAsync(cancelTaskRequest, context.RequestAborted).ConfigureAwait(false)).ConfigureAwait(false);
                break;

            case A2AProtocol.Methods.Tasks.PushNotifications.Set:
                if (!server.Capabilities.PushNotifications)
                {
                    await PushNotificationNotSupported().ConfigureAwait(false);
                    return;
                }
                if (!TryConvertTo(request, out SetTaskPushNotificationsRequest setTaskPushNotificationsRequest))
                {
                    await InvalidParams().ConfigureAwait(false);
                    return;
                }
                await WriteJsonResponseAsync(await server.SetTaskPushNotificationsAsync(setTaskPushNotificationsRequest, context.RequestAborted).ConfigureAwait(false)).ConfigureAwait(false);
                break;

            case A2AProtocol.Methods.Tasks.PushNotifications.Get:
                if (!server.Capabilities.PushNotifications)
                {
                    await PushNotificationNotSupported().ConfigureAwait(false);
                    return;
                }
                if (!TryConvertTo(request, out GetTaskPushNotificationsRequest getTaskPushNotificationsRequest))
                {
                    await InvalidParams().ConfigureAwait(false);
                    return;
                }
                await WriteJsonResponseAsync(await server.GetTaskPushNotificationsAsync(getTaskPushNotificationsRequest, context.RequestAborted).ConfigureAwait(false)).ConfigureAwait(false);
                break;

            default:
                await WriteJsonResponseAsync(new UnsupportedOperationError()).ConfigureAwait(false);
                break;
        }

        System.Threading.Tasks.Task InvalidParams()
        {
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            return WriteJsonResponseAsync(new InvalidParamsError());
        }

        System.Threading.Tasks.Task UnsupportedOperation()
        {
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            return WriteJsonResponseAsync(new UnsupportedOperationError());
        }

        System.Threading.Tasks.Task PushNotificationNotSupported()
        {
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            return WriteJsonResponseAsync(new PushNotificationNotSupportedError());
        }

        async System.Threading.Tasks.Task WriteJsonResponseAsync(object result)
        {
            context.Response.ContentType = MediaTypeNames.Application.Json;
            await JsonSerializer.SerializeAsync(context.Response.Body, result, result.GetType(), cancellationToken: context.RequestAborted);
        }

    }

    static async System.Threading.Tasks.Task StreamTaskEventsAsync(HttpContext context, IAsyncEnumerable<RpcResponse<TaskEvent>> stream)
    {
        context.Response.ContentType = "text/event-stream";
        context.Response.Headers.CacheControl = "no-cache";
        context.Response.Headers.Connection = "keep-alive";
        await context.Response.Body.FlushAsync(context.RequestAborted);
        await foreach (var e in stream.WithCancellation(context.RequestAborted))
        {
            var sseMessage = $"data: {JsonSerializer.Serialize(e)}\n\n";
            await context.Response.Body.WriteAsync(Encoding.UTF8.GetBytes(sseMessage), context.RequestAborted);
            await context.Response.Body.FlushAsync(context.RequestAborted);
        }
    }

    static bool TryConvertTo<TRequest>(RpcRequest request, out TRequest result) 
        where TRequest : RpcRequest
    {
        try
        {
            var json = JsonSerializer.SerializeToElement(request);
            result = json.Deserialize<TRequest>()!;
            return result != null;
        }
        catch
        {
            result = null!;
            return false;
        }
    }

}
