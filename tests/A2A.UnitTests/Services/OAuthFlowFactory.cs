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

namespace A2A.UnitTests.Services;

internal static class OAuthFlowFactory
{

    internal static OAuthFlow CreateAuthorizationCodeFlow() => new()
    {
        AuthorizationUrl = new Uri("https://example.com/oauth2/authorize"),
        TokenUrl = new Uri("https://example.com/oauth2/token"),
        Scopes = new Dictionary<string, string>
        {
            ["read"] = "Read access",
            ["write"] = "Write access"
        }
    };

    internal static OAuthFlow CreateClientCredentialsFlow() => new()
    {
        TokenUrl = new Uri("https://example.com/oauth2/token"),
        Scopes = new Dictionary<string, string>
        {
            ["admin"] = "Admin access"
        }
    };

    internal static OAuthFlow CreateImplicitFlow() => new()
    {
        AuthorizationUrl = new Uri("https://example.com/oauth2/authorize"),
        Scopes = new Dictionary<string, string>
        {
            ["user"] = "User access"
        }
    };

    internal static OAuthFlow CreatePasswordFlow() => new()
    {
        TokenUrl = new Uri("https://example.com/oauth2/token"),
        Scopes = new Dictionary<string, string>
        {
            ["basic"] = "Basic access"
        }
    };

}