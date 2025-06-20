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
/// Represents the default implementation of the <see cref="IOpenIdConnectSecuritySchemeBuilder"/> interface.
/// </summary>
public class OpenIdConnectSecuritySchemeBuilder
    : IOpenIdConnectSecuritySchemeBuilder
{

    /// <summary>
    /// Gets the <see cref="OpenIdConnectSecurityScheme"/> to configure.
    /// </summary>
    protected OpenIdConnectSecurityScheme SecurityScheme { get; } = new();

    /// <inheritdoc/>
    public virtual IOpenIdConnectSecuritySchemeBuilder WithUrl(Uri url)
    {
        ArgumentNullException.ThrowIfNull(url);
        SecurityScheme.OpenIdConnectUrl = url;
        return this;
    }

    /// <inheritdoc/>
    public virtual OpenIdConnectSecurityScheme Build() => SecurityScheme;

    SecurityScheme ISecuritySchemeBuilder.Build() => Build();

}
