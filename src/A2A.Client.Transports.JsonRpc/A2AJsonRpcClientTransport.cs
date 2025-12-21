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

using System.Net.ServerSentEvents;
using Task = System.Threading.Tasks.Task;

namespace A2A.Client.Transports;

/// <summary>
/// Represents the JSON-RPC implementation of the <see cref="IA2AClientTransport"/> interface.
/// </summary>
/// <param name="httpClient">The service used to perform HTTP requests.</param>
public sealed class A2AJsonRpcClientTransport(HttpClient httpClient)
    : IA2AClientTransport
{

    const string ExtensionHeaderName = "A2A-Extensions";

    /// <inheritdoc/>
    public async Task<Response> SendMessageAsync(SendMessageRequest request, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);
        var rpcRequest = new JsonRpcRequest()
        {
            Method = A2AJsonRpcMethod.Message.Send,
            Params = JsonSerializer.SerializeToNode(request, JsonSerializationContext.Default.SendMessageRequest)!.AsObject()
        };
        using var content = new StringContent(JsonSerializer.Serialize(rpcRequest, JsonRpcSerializationContext.Default.JsonRpcRequest), System.Text.Encoding.UTF8, MediaTypeNames.Application.Json);
        using var httpRequest = new HttpRequestMessage(HttpMethod.Post, string.Empty)
        {
            Content = content
        };
        using var httpResponse = await httpClient.SendAsync(httpRequest, cancellationToken).ConfigureAwait(false);
        httpResponse.EnsureSuccessStatusCode();
        var rpcResponse = await httpResponse.Content.ReadFromJsonAsync(JsonRpcSerializationContext.Default.JsonRpcResponse, cancellationToken).ConfigureAwait(false) ?? throw new InvalidOperationException("An error occurred while deserializing the response.");
        if (rpcResponse.Error is not null) throw new Exception($"An error occurred while processing the JSON-RPC request (error code: {rpcResponse.Error.Code}): {rpcResponse.Error.Message}).");
        return JsonSerializer.Deserialize(rpcResponse.Result, JsonSerializationContext.Default.Response) ?? throw new InvalidOperationException("An error occurred while deserializing the response.");
    }

    /// <inheritdoc/>
    public async IAsyncEnumerable<StreamResponse> SendStreamingMessageAsync(SendMessageRequest request, [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);
        var rpcRequest = new JsonRpcRequest()
        {
            Method = A2AJsonRpcMethod.Message.SendStreaming,
            Params = JsonSerializer.SerializeToNode(request, JsonSerializationContext.Default.SendMessageRequest)!.AsObject()
        };
        using var content = new StringContent(JsonSerializer.Serialize(rpcRequest, JsonRpcSerializationContext.Default.JsonRpcRequest), System.Text.Encoding.UTF8, MediaTypeNames.Application.Json);
        using var httpRequest = new HttpRequestMessage(HttpMethod.Post, string.Empty)
        {
            Content = content
        };
        using var httpResponse = await httpClient.SendAsync(httpRequest, HttpCompletionOption.ResponseHeadersRead, cancellationToken).ConfigureAwait(false);
        httpResponse.EnsureSuccessStatusCode();
        await using var stream = await httpResponse.Content.ReadAsStreamAsync(cancellationToken).ConfigureAwait(false);
        var sseParser = SseParser.Create(stream, (type, bytes) => JsonSerializer.Deserialize(bytes, JsonSerializationContext.Default.StreamResponse)!);
        await foreach (var streamResponse in sseParser.EnumerateAsync(cancellationToken)) yield return streamResponse.Data;
    }

    /// <inheritdoc/>
    public async Task<Models.Task> GetTaskAsync(string id, uint? historyLength = null, string? tenant = null, CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(id);
        var rpcRequest = new JsonRpcRequest()
        {
            Method = A2AJsonRpcMethod.Task.Get,
            Params = JsonSerializer.SerializeToNode(new GetTaskMethodParameters()
            {
                TaskId = id,
                HistoryLength = historyLength,
                Tenant = tenant
            }, JsonRpcSerializationContext.Default.GetTaskMethodParameters)!.AsObject()
        };
        using var content = new StringContent(JsonSerializer.Serialize(rpcRequest, JsonRpcSerializationContext.Default.JsonRpcRequest), System.Text.Encoding.UTF8, MediaTypeNames.Application.Json);
        using var httpRequest = new HttpRequestMessage(HttpMethod.Post, string.Empty)
        {
            Content = content
        };
        using var httpResponse = await httpClient.SendAsync(httpRequest, cancellationToken).ConfigureAwait(false);
        httpResponse.EnsureSuccessStatusCode();
        var rpcResponse = await httpResponse.Content.ReadFromJsonAsync(JsonRpcSerializationContext.Default.JsonRpcResponse, cancellationToken).ConfigureAwait(false) ?? throw new InvalidOperationException("An error occurred while deserializing the response.");
        if (rpcResponse.Error is not null) throw new Exception($"An error occurred while processing the JSON-RPC request (error code: {rpcResponse.Error.Code}): {rpcResponse.Error.Message}).");
        return JsonSerializer.Deserialize(rpcResponse.Result, JsonSerializationContext.Default.Task) ?? throw new InvalidOperationException("An error occurred while deserializing the response.");
    }

    /// <inheritdoc/>
    public async Task<TaskQueryResult> ListTasksAsync(TaskQueryOptions? queryOptions = null, CancellationToken cancellationToken = default)
    {
        var rpcRequest = new JsonRpcRequest()
        {
            Method = A2AJsonRpcMethod.Task.List,
            Params = JsonSerializer.SerializeToNode(queryOptions ?? new(), JsonSerializationContext.Default.TaskQueryOptions)!.AsObject()
        };
        using var content = new StringContent(JsonSerializer.Serialize(rpcRequest, JsonRpcSerializationContext.Default.JsonRpcRequest), System.Text.Encoding.UTF8, MediaTypeNames.Application.Json);
        using var httpRequest = new HttpRequestMessage(HttpMethod.Post, string.Empty)
        {
            Content = content
        };
        using var httpResponse = await httpClient.SendAsync(httpRequest, cancellationToken).ConfigureAwait(false);
        httpResponse.EnsureSuccessStatusCode();
        var rpcResponse = await httpResponse.Content.ReadFromJsonAsync(JsonRpcSerializationContext.Default.JsonRpcResponse, cancellationToken).ConfigureAwait(false) ?? throw new InvalidOperationException("An error occurred while deserializing the response.");
        if (rpcResponse.Error is not null) throw new Exception($"An error occurred while processing the JSON-RPC request (error code: {rpcResponse.Error.Code}): {rpcResponse.Error.Message}).");
        return JsonSerializer.Deserialize(rpcResponse.Result, JsonSerializationContext.Default.TaskQueryResult) ?? throw new InvalidOperationException("An error occurred while deserializing the response.");
    }

    /// <inheritdoc/>
    public async Task<Models.Task> CancelTaskAsync(string id, string? tenant = null, CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(id);
        var rpcRequest = new JsonRpcRequest()
        {
            Method = A2AJsonRpcMethod.Task.Cancel,
            Params = JsonSerializer.SerializeToNode(new CancelTaskMethodParameters()
            {
                Id = id,
                Tenant = tenant
            }, JsonRpcSerializationContext.Default.CancelTaskMethodParameters)!.AsObject()
        };
        using var content = new StringContent(JsonSerializer.Serialize(rpcRequest, JsonRpcSerializationContext.Default.JsonRpcRequest), System.Text.Encoding.UTF8, MediaTypeNames.Application.Json);
        using var httpRequest = new HttpRequestMessage(HttpMethod.Post, string.Empty)
        {
            Content = content
        };
        using var httpResponse = await httpClient.SendAsync(httpRequest, cancellationToken).ConfigureAwait(false);
        httpResponse.EnsureSuccessStatusCode();
        var rpcResponse = await httpResponse.Content.ReadFromJsonAsync(JsonRpcSerializationContext.Default.JsonRpcResponse, cancellationToken).ConfigureAwait(false) ?? throw new InvalidOperationException("An error occurred while deserializing the response.");
        if (rpcResponse.Error is not null) throw new Exception($"An error occurred while processing the JSON-RPC request (error code: {rpcResponse.Error.Code}): {rpcResponse.Error.Message}).");
        return JsonSerializer.Deserialize(rpcResponse.Result, JsonSerializationContext.Default.Task) ?? throw new InvalidOperationException("An error occurred while deserializing the response.");
    }

    /// <inheritdoc/>
    public async IAsyncEnumerable<StreamResponse> SubscribeToTaskAsync(string id, string? tenant = null, [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(id);
        var rpcRequest = new JsonRpcRequest()
        {
            Method = A2AJsonRpcMethod.Task.Subscribe,
            Params = JsonSerializer.SerializeToNode(new SubscribeToTaskMethodParameters()
            {
                Id = id,
                Tenant = tenant
            }, JsonRpcSerializationContext.Default.SubscribeToTaskMethodParameters)!.AsObject()
        };
        using var content = new StringContent(JsonSerializer.Serialize(rpcRequest, JsonRpcSerializationContext.Default.JsonRpcRequest), System.Text.Encoding.UTF8, MediaTypeNames.Application.Json);
        using var httpRequest = new HttpRequestMessage(HttpMethod.Post, string.Empty)
        {
            Content = content
        };
        using var httpResponse = await httpClient.SendAsync(httpRequest, HttpCompletionOption.ResponseHeadersRead, cancellationToken).ConfigureAwait(false);
        httpResponse.EnsureSuccessStatusCode();
        await using var stream = await httpResponse.Content.ReadAsStreamAsync(cancellationToken).ConfigureAwait(false);
        var sseParser = SseParser.Create(stream, (type, bytes) => JsonSerializer.Deserialize(bytes, JsonSerializationContext.Default.StreamResponse)!);
        await foreach (var streamResponse in sseParser.EnumerateAsync(cancellationToken)) yield return streamResponse.Data;
    }

    /// <inheritdoc/>
    public async Task<TaskPushNotificationConfig> SetTaskPushNotificationConfigAsync(SetTaskPushNotificationConfigRequest request, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);
        var rpcRequest = new JsonRpcRequest()
        {
            Method = A2AJsonRpcMethod.Task.PushNotificationConfig.Set,
            Params = JsonSerializer.SerializeToNode(request, JsonSerializationContext.Default.SetTaskPushNotificationConfigRequest)!.AsObject()
        };
        using var content = new StringContent(JsonSerializer.Serialize(rpcRequest, JsonRpcSerializationContext.Default.JsonRpcRequest), System.Text.Encoding.UTF8, MediaTypeNames.Application.Json);
        using var httpRequest = new HttpRequestMessage(HttpMethod.Post, string.Empty)
        {
            Content = content
        };
        using var httpResponse = await httpClient.SendAsync(httpRequest, cancellationToken).ConfigureAwait(false);
        httpResponse.EnsureSuccessStatusCode();
        var rpcResponse = await httpResponse.Content.ReadFromJsonAsync(JsonRpcSerializationContext.Default.JsonRpcResponse, cancellationToken).ConfigureAwait(false) ?? throw new InvalidOperationException("An error occurred while deserializing the response.");
        if (rpcResponse.Error is not null) throw new Exception($"An error occurred while processing the JSON-RPC request (error code: {rpcResponse.Error.Code}): {rpcResponse.Error.Message}).");
        return JsonSerializer.Deserialize(rpcResponse.Result, JsonSerializationContext.Default.TaskPushNotificationConfig) ?? throw new InvalidOperationException("An error occurred while deserializing the response.");
    }

    /// <inheritdoc/>
    public async Task<TaskPushNotificationConfig> GetTaskPushNotificationConfigAsync(string taskId, string configId, string? tenant = null, CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(taskId);
        ArgumentException.ThrowIfNullOrWhiteSpace(configId);
        var rpcRequest = new JsonRpcRequest()
        {
            Method = A2AJsonRpcMethod.Task.PushNotificationConfig.Get,
            Params = JsonSerializer.SerializeToNode(new GetPushNotificationConfigMethodParameters()
            {
                TaskId = taskId,
                ConfigId = configId,
                Tenant = tenant
            }, JsonRpcSerializationContext.Default.GetPushNotificationConfigMethodParameters)!.AsObject()
        };
        using var content = new StringContent(JsonSerializer.Serialize(rpcRequest, JsonRpcSerializationContext.Default.JsonRpcRequest), System.Text.Encoding.UTF8, MediaTypeNames.Application.Json);
        using var httpRequest = new HttpRequestMessage(HttpMethod.Post, string.Empty)
        {
            Content = content
        };
        using var httpResponse = await httpClient.SendAsync(httpRequest, cancellationToken).ConfigureAwait(false);
        httpResponse.EnsureSuccessStatusCode();
        var rpcResponse = await httpResponse.Content.ReadFromJsonAsync(JsonRpcSerializationContext.Default.JsonRpcResponse, cancellationToken).ConfigureAwait(false) ?? throw new InvalidOperationException("An error occurred while deserializing the response.");
        if (rpcResponse.Error is not null) throw new Exception($"An error occurred while processing the JSON-RPC request (error code: {rpcResponse.Error.Code}): {rpcResponse.Error.Message}).");
        return JsonSerializer.Deserialize(rpcResponse.Result, JsonSerializationContext.Default.TaskPushNotificationConfig) ?? throw new InvalidOperationException("An error occurred while deserializing the response.");
    }

    /// <inheritdoc/>
    public async Task<TaskPushNotificationConfigQueryResult> ListTaskPushNotificationConfigAsync(TaskPushNotificationConfigQueryOptions queryOptions, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(queryOptions);
        var rpcRequest = new JsonRpcRequest()
        {
            Method = A2AJsonRpcMethod.Task.PushNotificationConfig.List,
            Params = JsonSerializer.SerializeToNode(queryOptions, JsonSerializationContext.Default.TaskPushNotificationConfigQueryOptions)!.AsObject()
        };
        using var content = new StringContent(JsonSerializer.Serialize(rpcRequest, JsonRpcSerializationContext.Default.JsonRpcRequest), System.Text.Encoding.UTF8, MediaTypeNames.Application.Json);
        using var httpRequest = new HttpRequestMessage(HttpMethod.Post, string.Empty)
        {
            Content = content
        };
        using var httpResponse = await httpClient.SendAsync(httpRequest, cancellationToken).ConfigureAwait(false);
        httpResponse.EnsureSuccessStatusCode();
        var rpcResponse = await httpResponse.Content.ReadFromJsonAsync(JsonRpcSerializationContext.Default.JsonRpcResponse, cancellationToken).ConfigureAwait(false) ?? throw new InvalidOperationException("An error occurred while deserializing the response.");
        if (rpcResponse.Error is not null) throw new Exception($"An error occurred while processing the JSON-RPC request (error code: {rpcResponse.Error.Code}): {rpcResponse.Error.Message}).");
        return JsonSerializer.Deserialize(rpcResponse.Result, JsonSerializationContext.Default.TaskPushNotificationConfigQueryResult) ?? throw new InvalidOperationException("An error occurred while deserializing the response.");
    }

    /// <inheritdoc/>
    public async Task DeletePushNotificationConfigAsync(string taskId, string configId, string? tenant = null, CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(taskId);
        ArgumentException.ThrowIfNullOrWhiteSpace(configId);
        var rpcRequest = new JsonRpcRequest()
        {
            Method = A2AJsonRpcMethod.Task.PushNotificationConfig.Delete,
            Params = JsonSerializer.SerializeToNode(new DeletePushNotificationConfigMethodParameters()
            {
                TaskId = taskId,
                ConfigId = configId,
                Tenant = tenant
            }, JsonRpcSerializationContext.Default.DeletePushNotificationConfigMethodParameters)!.AsObject()
        };
        using var content = new StringContent(JsonSerializer.Serialize(rpcRequest, JsonRpcSerializationContext.Default.JsonRpcRequest), System.Text.Encoding.UTF8, MediaTypeNames.Application.Json);
        using var httpRequest = new HttpRequestMessage(HttpMethod.Post, string.Empty)
        {
            Content = content
        };
        using var httpResponse = await httpClient.SendAsync(httpRequest, cancellationToken).ConfigureAwait(false);
        httpResponse.EnsureSuccessStatusCode();
        var rpcResponse = await httpResponse.Content.ReadFromJsonAsync(JsonRpcSerializationContext.Default.JsonRpcResponse, cancellationToken).ConfigureAwait(false) ?? throw new InvalidOperationException("An error occurred while deserializing the response.");
        if (rpcResponse.Error is not null) throw new Exception($"An error occurred while processing the JSON-RPC request (error code: {rpcResponse.Error.Code}): {rpcResponse.Error.Message}).");
    }

    /// <inheritdoc/>
    public void ActivateExtension(Uri uri)
    {
        ArgumentNullException.ThrowIfNull(uri);
        var value = uri.OriginalString;
        if (httpClient.DefaultRequestHeaders.TryGetValues(ExtensionHeaderName, out var existing) && existing.Contains(value, StringComparer.OrdinalIgnoreCase)) return;
        httpClient.DefaultRequestHeaders.TryAddWithoutValidation(ExtensionHeaderName, value);
    }

    /// <inheritdoc/>
    public void DeactivateExtension(Uri uri)
    {
        ArgumentNullException.ThrowIfNull(uri);
        var value = uri.OriginalString;
        if (!httpClient.DefaultRequestHeaders.TryGetValues(ExtensionHeaderName, out var existing)) return;
        var extensions = existing.Where(v => !string.Equals(v, value, StringComparison.OrdinalIgnoreCase)).ToArray();
        httpClient.DefaultRequestHeaders.Remove(ExtensionHeaderName);
        if (extensions.Length == 0) return;
        foreach (var extension in extensions) httpClient.DefaultRequestHeaders.TryAddWithoutValidation(ExtensionHeaderName, extension);
    }

}
