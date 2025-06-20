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
/// Defines the fundamentals of a service used to manage the application's JSON Web Key Set (JWKS)
/// </summary>
public interface IJsonWebKeySet
{

    /// <summary>
    /// Adds the specified public key to the application's JSON Web Key Set 
    /// </summary>
    /// <param name="key">The public key to add</param>
    void AddPublicKey(JsonWebKey key);

    /// <summary>
    /// Exports the application's JSON Web Key Set (JWKS)
    /// </summary>
    /// <returns>The JSON representation of the application's JWKS</returns>
    string Export();

}
