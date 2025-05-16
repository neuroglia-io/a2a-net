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

namespace A2A.Samples.SemanticKernel.Client.Configuration
{
    /// <summary>
    /// Represents the options used to configure the application
    /// </summary>
    public class ApplicationOptions
    {
        /// <summary>
        /// Gets/sets the URI of the A2A server to interact with
        /// </summary>
        [Required]
        public Uri? Server { get; set; } = null;

        /// <summary>
        /// Gets/sets the URI, if any, of the endpoint to send push notifications to
        /// </summary>
        public Uri? PushNotificationClient { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether streaming is enabled
        /// </summary>
        public bool Streaming { get; set; }

        /// <summary>
        /// Gets or sets the authentication token or mechanism
        /// </summary>
        public string? Auth { get; set; }
    }
}
