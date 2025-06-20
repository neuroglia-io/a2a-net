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

using Microsoft.IdentityModel.Tokens;

namespace A2A.Server.Infrastructure.Services;

/// <summary>
/// Represents the default implementation of the <see cref="IJsonWebKeySet"/> interface
/// </summary>
public class JsonWebKeySet
    : IJsonWebKeySet
{

    /// <summary>
    /// Gets a list containing all registered public keys
    /// </summary>
    protected List<JsonWebKey> PublicKeys { get; } = [];

    /// <inheritdoc/>
    public virtual void AddPublicKey(JsonWebKey key)
    {
        ArgumentNullException.ThrowIfNull(key);
        PublicKeys.Add(key);
    }

    /// <inheritdoc/>
    public virtual string Export()
    {
        var jwks = new 
        { 
            keys = PublicKeys 
        };
        return JsonSerializer.Serialize(jwks);
    }

}
