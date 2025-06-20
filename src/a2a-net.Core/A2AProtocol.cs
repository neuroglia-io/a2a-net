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

namespace A2A;

/// <summary>
/// Exposes constants and static about the A2A protocol.
/// </summary>
public static class A2AProtocol
{

    /// <summary>
    /// Exposes the JSON-RPC method names supported by the A2A protocol.
    /// </summary>
    public static class Methods
    {

        /// <summary>
        /// Exposes method names related to message management operations.
        /// </summary>
        public static class Messages
        {

            const string Prefix = "messages/";

            /// <summary>
            /// The name of the method for sending a message to an agent ("messages/send").
            /// </summary>
            public const string Send = Prefix + "send";
            /// <summary>
            /// The name of the method for streaming messages to an agent ("messages/stream").
            /// </summary>
            public const string Stream = Prefix + "stream";
        }

        /// <summary>
        /// Exposes method names related to task management operations.
        /// </summary>
        public static class Tasks
        {

            const string Prefix = "tasks/";

            /// <summary>
            /// The name of the method for resubscribing to task events from a remote agent ("tasks/resubscribe").
            /// </summary>
            public const string Resubscribe = Prefix + "resubscribe";
            /// <summary>
            /// The name of the method for retrieving a task's details and optional history ("tasks/get").
            /// </summary>
            public const string Get = Prefix + "get";
            /// <summary>
            /// The name of the method for cancelling a submitted task ("tasks/cancel").
            /// </summary>
            public const string Cancel = Prefix + "cancel";

            /// <summary>
            /// Exposes method names related to task push notifications.
            /// </summary>
            public static class PushNotifications
            {
      
                const string Prefix = Tasks.Prefix + "pushNotification/";

                /// <summary>
                /// The name of the method for retrieving the current push notification configuration for a task ("tasks/pushNotification/get").
                /// </summary>
                public const string Get = Prefix + "get";

                /// <summary>
                /// The name of the method for setting or updating the push notification configuration for a task ("tasks/pushNotification/set").
                /// </summary>
                public const string Set = Prefix + "set";
            }

        }

    }

}