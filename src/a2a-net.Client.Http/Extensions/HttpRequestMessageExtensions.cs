﻿// Copyright © 2025-Present the a2a-net Authors
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
/// Defines extensions for <see cref="HttpRequestMessage"/>s
/// </summary>
public static class HttpRequestMessageExtensions
{

    /// <summary>
    /// Enables Web Assembly streaming response for the specified <see cref="HttpRequestMessage"/>
    /// </summary>
    /// <param name="request">The <see cref="HttpRequestMessage"/> to enable Web Assembly streaming response for</param>
    public static void EnableWebAssemblyStreamingResponse(this HttpRequestMessage request) => request.Options.Set(new("WebAssemblyEnableStreamingResponse"), true);

}
