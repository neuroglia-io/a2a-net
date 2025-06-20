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
/// Represents the default implementation of the <see cref="IHttpSecuritySchemeBuilder"/> interface.
/// </summary>
public class HttpSecuritySchemeBuilder
    : IHttpSecuritySchemeBuilder
{

    /// <summary>
    /// Gets the <see cref="HttpSecurityScheme"/> to configure.
    /// </summary>
    protected HttpSecurityScheme SecurityScheme { get; } = new();

    /// <inheritdoc/>
    public virtual IHttpSecuritySchemeBuilder WithScheme(string scheme)
    {
        SecurityScheme.Scheme = scheme;
        return this;
    }

    /// <inheritdoc/>
    public virtual IHttpSecuritySchemeBuilder WithBearerFormat(string format)
    {
        SecurityScheme.BearerFormat = format;
        return this;
    }

    /// <inheritdoc/>
    public virtual HttpSecurityScheme Build() => SecurityScheme;

    SecurityScheme ISecuritySchemeBuilder.Build() => Build();

}
