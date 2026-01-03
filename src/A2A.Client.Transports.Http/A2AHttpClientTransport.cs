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
/// Represents the HTTP implementation of the <see cref="IA2AClientTransport"/> interface.
/// </summary>
/// <param name="httpClient">The service used to perform HTTP requests.</param>
public sealed class A2AHttpClientTransport(HttpClient httpClient)
    : IA2AClientTransport
{

    const string ExtensionHeaderName = "A2A-Extensions";

    /// <inheritdoc/>
    public async Task<Response> SendMessageAsync(SendMessageRequest request, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);
        using var content = new StringContent(JsonSerializer.Serialize(request, JsonSerializationContext.Default.SendMessageRequest), Encoding.UTF8, MediaTypeNames.Application.Json);
        using var httpRequest = new HttpRequestMessage(HttpMethod.Post, "/v1/message:send")
        {
            Content = content
        };
        using var httpResponse = await httpClient.SendAsync(httpRequest, cancellationToken).ConfigureAwait(false);
        httpResponse.EnsureSuccessStatusCode();
        return await httpResponse.Content.ReadFromJsonAsync(JsonSerializationContext.Default.Response, cancellationToken).ConfigureAwait(false) ?? throw new InvalidOperationException("An error occurred while deserializing the response.");
    }

    /// <inheritdoc/>
    public async IAsyncEnumerable<StreamResponse> SendStreamingMessageAsync(SendMessageRequest request, [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);
        using var content = new StringContent(JsonSerializer.Serialize(request, JsonSerializationContext.Default.SendMessageRequest), Encoding.UTF8, MediaTypeNames.Application.Json);
        using var httpRequest = new HttpRequestMessage(HttpMethod.Post, "/v1/message:stream")
        {
            Content = content
        };
        using var httpResponse = await httpClient.SendAsync(httpRequest, HttpCompletionOption.ResponseHeadersRead, cancellationToken).ConfigureAwait(false);
        httpResponse.EnsureSuccessStatusCode();
        await using var stream = await httpResponse.Content.ReadAsStreamAsync(cancellationToken).ConfigureAwait(false);
        var sseParser = SseParser.Create(stream, (type, bytes) => JsonSerializer.Deserialize(bytes, JsonSerializationContext.Default.StreamResponse)!);
        await foreach (var sse in sseParser.EnumerateAsync(cancellationToken)) yield return sse.Data;
    }

    /// <inheritdoc/>
    public async Task<Models.Task> GetTaskAsync(string id, uint? historyLength = null, string? tenant = null, CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(id);
        var url = $"/v1/tasks/{id}";
        var queryParameters = new List<string>();
        if (historyLength.HasValue) queryParameters.Add($"historyLength={historyLength.Value}");
        if (!string.IsNullOrWhiteSpace(tenant)) queryParameters.Add($"tenant={Uri.EscapeDataString(tenant)}");
        if (queryParameters.Count > 0) url += "?" + string.Join("&", queryParameters);
        using var httpResponse = await httpClient.GetAsync(url, cancellationToken).ConfigureAwait(false);
        httpResponse.EnsureSuccessStatusCode();
        return await httpResponse.Content.ReadFromJsonAsync(JsonSerializationContext.Default.Task, cancellationToken).ConfigureAwait(false) ?? throw new InvalidOperationException("An error occurred while deserializing the response.");
    }

    /// <inheritdoc/>
    public async Task<TaskQueryResult> ListTasksAsync(TaskQueryOptions? queryOptions = null, CancellationToken cancellationToken = default)
    {
        var url = "/v1/tasks";
        var queryParameters = new List<string>();
        if (queryOptions is not null)
        {
            if (!string.IsNullOrWhiteSpace(queryOptions.ContextId)) queryParameters.Add($"contextId={Uri.EscapeDataString(queryOptions.ContextId)}");
            if (!string.IsNullOrWhiteSpace(queryOptions.Status)) queryParameters.Add($"status={Uri.EscapeDataString(queryOptions.Status)}");
            if (queryOptions.PageSize.HasValue) queryParameters.Add($"pageSize={queryOptions.PageSize.Value}");
            if (!string.IsNullOrWhiteSpace(queryOptions.PageToken)) queryParameters.Add($"pageToken={Uri.EscapeDataString(queryOptions.PageToken)}");
            if (queryOptions.HistoryLength.HasValue) queryParameters.Add($"historyLength={queryOptions.HistoryLength.Value}");
            if (queryOptions.LastUpdateAfter.HasValue) queryParameters.Add($"lastUpdateAfter={queryOptions.LastUpdateAfter.Value}");
            if (queryOptions.IncludeArtifacts.HasValue) queryParameters.Add($"includeArtifacts={queryOptions.IncludeArtifacts.Value.ToString().ToLowerInvariant()}");
        }
        if (queryParameters.Count > 0) url += "?" + string.Join("&", queryParameters);
        using var httpResponse = await httpClient.GetAsync(url, cancellationToken).ConfigureAwait(false);
        httpResponse.EnsureSuccessStatusCode();
        return await httpResponse.Content.ReadFromJsonAsync(JsonSerializationContext.Default.TaskQueryResult, cancellationToken).ConfigureAwait(false) ?? throw new InvalidOperationException("An error occurred while deserializing the response.");
    }

    /// <inheritdoc/>
    public async Task<Models.Task> CancelTaskAsync(string id, string? tenant = null, CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(id);
        var uri = $"/v1/tasks/{id}:cancel";
        if(!string.IsNullOrWhiteSpace(tenant)) uri += $"?tenant={Uri.EscapeDataString(tenant)}";
        using var httpResponse = await httpClient.PostAsync(uri, null, cancellationToken).ConfigureAwait(false);
        httpResponse.EnsureSuccessStatusCode();
        return await httpResponse.Content.ReadFromJsonAsync(JsonSerializationContext.Default.Task, cancellationToken).ConfigureAwait(false) ?? throw new InvalidOperationException("An error occurred while deserializing the response.");
    }

    /// <inheritdoc/>
    public async IAsyncEnumerable<StreamResponse> SubscribeToTaskAsync(string id, string? tenant = null, [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(id);
        var uri = $"/v1/tasks/{id}:subscribe";
        if (!string.IsNullOrWhiteSpace(tenant)) uri += $"?tenant={Uri.EscapeDataString(tenant)}";
        using var httpRequest = new HttpRequestMessage(HttpMethod.Post, uri);
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
        var taskId = request.Parent.Split('/')[1];
        using var content = new StringContent(JsonSerializer.Serialize(request, JsonSerializationContext.Default.SetTaskPushNotificationConfigRequest), Encoding.UTF8, MediaTypeNames.Application.Json);
        using var httpRequest = new HttpRequestMessage(HttpMethod.Post, $"/v1/tasks/{taskId}/pushNotificationConfigs")
        {
            Content = content
        };
        using var httpResponse = await httpClient.SendAsync(httpRequest, cancellationToken).ConfigureAwait(false);
        httpResponse.EnsureSuccessStatusCode();
        return await httpResponse.Content.ReadFromJsonAsync(JsonSerializationContext.Default.TaskPushNotificationConfig, cancellationToken).ConfigureAwait(false) ?? throw new InvalidOperationException("An error occurred while deserializing the response.");
    }

    /// <inheritdoc/>
    public async Task<TaskPushNotificationConfig> GetTaskPushNotificationConfigAsync(string taskId, string configId, string? tenant = null, CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(taskId);
        ArgumentException.ThrowIfNullOrWhiteSpace(configId);
        var uri = $"/v1/tasks/{taskId}/pushNotificationConfigs/{configId}";
        if (!string.IsNullOrWhiteSpace(tenant)) uri += $"?tenant={Uri.EscapeDataString(tenant)}";
        using var httpResponse = await httpClient.GetAsync(uri, cancellationToken).ConfigureAwait(false);
        httpResponse.EnsureSuccessStatusCode();
        return await httpResponse.Content.ReadFromJsonAsync(JsonSerializationContext.Default.TaskPushNotificationConfig, cancellationToken).ConfigureAwait(false) ?? throw new InvalidOperationException("An error occurred while deserializing the response.");
    }

    /// <inheritdoc/>
    public async Task<TaskPushNotificationConfigQueryResult> ListTaskPushNotificationConfigAsync(TaskPushNotificationConfigQueryOptions queryOptions, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(queryOptions);
        var url = $"/v1/tasks/{queryOptions.TaskId}/pushNotificationConfigs";
        var queryParameters = new List<string>();
        if (queryOptions is not null)
        {
            if (queryOptions.PageSize.HasValue) queryParameters.Add($"pageSize={queryOptions.PageSize.Value}");
            if (!string.IsNullOrWhiteSpace(queryOptions.PageToken)) queryParameters.Add($"pageToken={Uri.EscapeDataString(queryOptions.PageToken)}");
            if (!string.IsNullOrWhiteSpace(queryOptions.Tenant)) queryParameters.Add($"tenant={Uri.EscapeDataString(queryOptions.Tenant)}");
        }
        if (queryParameters.Count > 0) url += "?" + string.Join("&", queryParameters);
        using var httpResponse = await httpClient.GetAsync(url, cancellationToken).ConfigureAwait(false);
        httpResponse.EnsureSuccessStatusCode();
        return await httpResponse.Content.ReadFromJsonAsync(JsonSerializationContext.Default.TaskPushNotificationConfigQueryResult, cancellationToken).ConfigureAwait(false) ?? throw new InvalidOperationException("An error occurred while deserializing the response.");
    }

    /// <inheritdoc/>
    public async Task DeletePushNotificationConfigAsync(string taskId, string configId, string? tenant = null, CancellationToken cancellationToken = default)
    {
        var uri = $"/v1/tasks/{taskId}/pushNotificationConfigs/{configId}";
        if (!string.IsNullOrWhiteSpace(tenant)) uri += $"?tenant={Uri.EscapeDataString(tenant)}";
        using var httpResponse = await httpClient.DeleteAsync(uri, cancellationToken).ConfigureAwait(false);
        httpResponse.EnsureSuccessStatusCode();

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
