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

namespace A2A.Server.Infrastructure.Services;

/// <summary>
/// Represents the default implementation of the <see cref="IOAuthFlowBuilder"/> interface.
/// </summary>
public class OAuthFlowBuilder
    : IOAuthFlowBuilder
{

    /// <summary>
    /// Gets the <see cref="OAuthFlow"/> to configure.
    /// </summary>
    protected OAuthFlow Flow { get; } = new();

    /// <inheritdoc/>
    public virtual IOAuthFlowBuilder WithAuthorizationUrl(Uri url)
    {
        ArgumentNullException.ThrowIfNull(url);
        Flow.AuthorizationUrl = url;
        return this;
    }

    /// <inheritdoc/>
    public virtual IOAuthFlowBuilder WithTokenUrl(Uri url)
    {
        ArgumentNullException.ThrowIfNull(url);
        Flow.TokenUrl = url;
        return this;
    }

    /// <inheritdoc/>
    public virtual IOAuthFlowBuilder WithRefreshUrl(Uri url)
    {
        ArgumentNullException.ThrowIfNull(url);
        Flow.RefreshUrl = url;
        return this;
    }

    /// <inheritdoc/>
    public virtual IOAuthFlowBuilder AddScope(string name, string? description = null)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        Flow.Scopes ??= new();
        Flow.Scopes[name] = description ?? string.Empty;
        return this;
    }

    /// <inheritdoc/>
    public virtual OAuthFlow Build() => Flow;

}