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

namespace A2A.Client.Configuration;

/// <summary>
/// Represents the options used to configure an A2A client
/// </summary>
public class A2AProtocolClientOptions
{

    /// <summary>
    /// Gets/sets the <see cref="Uri"/> that references the endpoint of the A2A server to use
    /// </summary>
    public virtual Uri Endpoint { get; set; } = null!;

    public virtual Func<(string Scheme, string Token)>? Authorization { get; set; } = null;
}
