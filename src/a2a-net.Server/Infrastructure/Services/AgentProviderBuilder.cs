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

namespace A2A.Server.Infrastructure.Services;

/// <summary>
/// Represents the default implementation of the <see cref="IAgentProviderBuilder"/> interface
/// </summary>
public class AgentProviderBuilder
    : IAgentProviderBuilder
{

    /// <summary>
    /// Gets the <see cref="AgentProvider"/> to build and configure
    /// </summary>
    protected AgentProvider Provider { get; } = new();

    /// <inheritdoc/>
    public virtual IAgentProviderBuilder WithOrganization(string organization)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(organization);
        Provider.Organization = organization;
        return this;
    }

    /// <inheritdoc/>
    public virtual IAgentProviderBuilder WithUrl(Uri url)
    {
        Provider.Url = url;
        return this;
    }

    /// <inheritdoc/>
    public virtual AgentProvider Build() => Provider;

}