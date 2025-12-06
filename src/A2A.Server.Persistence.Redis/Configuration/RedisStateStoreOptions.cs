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

namespace A2A.Server.Configuration;

/// <summary>
/// Represents the options used to configure a <see cref="RedisStateStore"/>.
/// </summary>
public sealed class RedisStateStoreOptions
{

    /// <summary>
    /// Gets or sets the connection string to use.
    /// </summary>
    [Required, MinLength(1)]
    public string ConnectionString { get; set; } = null!;

    /// <summary>
    /// Gets or sets the prefix to use for all keys stored in the Redis state store.
    /// </summary>
    public string? KeyPrefix { get; set; } = "a2a:";

    /// <summary>
    /// Gets or sets the default page size for paginated queries.
    /// </summary>
    public uint DefaultPageSize { get; init; } = 25;

    /// <summary>
    /// Gets or sets the maximum page size for paginated queries.
    /// </summary>
    public uint MaxPageSize { get; init; } = 100;

}