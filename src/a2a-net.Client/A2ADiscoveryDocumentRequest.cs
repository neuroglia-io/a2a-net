﻿// Copyright � 2025-Present the a2a-net Authors
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

namespace A2A.Client;

/// <summary>
/// Represents a request to retrieve an A2A discovery document from a remote server
/// </summary>
public class A2ADiscoveryDocumentRequest
{

    /// <summary>
    /// Gets/sets the base URI of the remote server to query for discovery metadata
    /// </summary>
    public virtual Uri? Address { get; init; }

}
