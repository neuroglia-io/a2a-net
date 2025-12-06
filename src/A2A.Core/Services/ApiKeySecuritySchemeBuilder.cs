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

namespace A2A.Services;

/// <summary>
/// Represents the default implementation of the <see cref="IApiKeySecuritySchemeBuilder"/> interface.
/// </summary>
public sealed class ApiKeySecuritySchemeBuilder
    : IApiKeySecuritySchemeBuilder
{

    readonly ApiKeySecurityScheme securityScheme = new();

    /// <inheritdoc/>
    public IApiKeySecuritySchemeBuilder WithName(string name)
    {
        securityScheme.Name = name;
        return this;
    }

    /// <inheritdoc/>
    public IApiKeySecuritySchemeBuilder WithLocation(string location)
    {
        securityScheme.In = location;
        return this;
    }

    /// <inheritdoc/>
    public ApiKeySecurityScheme Build() => securityScheme;

    SecurityScheme ISecuritySchemeBuilder.Build() => Build();

}