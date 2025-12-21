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

namespace A2A.Extensions;

/// <summary>
/// Provides extension methods and related types for working with A2UI components and configuration.
/// </summary>
public static class A2UIExtension
{

    /// <summary>
    /// Gets the unique URI that identifies the A2UI extension.
    /// </summary>
    public static readonly Uri BaseUri = new("https://a2ui.org/a2a-extension/a2ui/");
    /// <summary>
    /// Gets the current version string of the A2UI extension.
    /// </summary>
    public const string CurrentVersion = "v0.9";
    /// <summary>
    /// Gets the unique URI that identifies the current version of the A2UI extension.
    /// </summary>
    public static readonly Uri Uri = GetUri(CurrentVersion);

    /// <summary>
    /// Gets the unique URI that identifies a specific version of the A2UI extension.
    /// </summary>
    /// <param name="version">The version string of the A2UI extension.</param>
    /// <returns>An URI representing the specific version of the A2UI extension.</returns>
    public static Uri GetUri(string version) => new(BaseUri, version);

    /// <summary>
    /// Represents the parameters for configuring an A2UI extension.
    /// </summary>
    [Description("Represents the parameters for configuring an A2UI extension.")]
    [DataContract]
    public sealed record Parameters
    {

        /// <summary>
        /// Gets a collection of URIs pointing to a component catalog definition schema that the agent can generate.
        /// </summary>
        [Description("A collection of URIs pointing to a component catalog definition schema that the agent can generate.")]
        [DataMember(Order = 1, Name = "supportedCatalogIds"), JsonPropertyOrder(1), JsonPropertyName("supportedCatalogIds")]
        public IReadOnlyCollection<string>? SupportedCatalogIds { get; init; }

        /// <summary>
        /// Gets a boolean indicating whether the agent can accept inline component catalogs provided directly within requests.
        /// </summary>
        [Description("A boolean indicating whether the agent can accept inline component catalogs provided directly within requests.")]
        [DataMember(Order = 2, Name = "acceptsInlineCatalogs"), JsonPropertyOrder(2), JsonPropertyName("acceptsInlineCatalogs")]
        public bool? AcceptsInlineCatalogs { get; init; }

    }

}