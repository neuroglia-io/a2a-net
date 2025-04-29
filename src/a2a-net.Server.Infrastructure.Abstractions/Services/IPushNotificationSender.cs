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

namespace A2A.Server.Infrastructure.Services;

/// <summary>
/// Defines the fundamentals of a service used to send push notifications
/// </summary>
public interface IPushNotificationSender
{

    /// <summary>
    /// Verifies the specified push notifications url
    /// </summary>
    /// <param name="url">The url to verify</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/></param>
    /// <returns>A boolean indicating whether or not the push notification url could be verified</returns>
    Task<bool> VerifyPushNotificationUrlAsync(Uri url, CancellationToken cancellationToken = default);

    /// <summary>
    /// Sends a push notification to the specified url
    /// </summary>
    /// <param name="url">The url to send the push notification to</param>
    /// <param name="payload">The push notification's payload</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/></param>
    /// <returns>A new awaitable <see cref="System.Threading.Tasks.Task"/></returns>
    System.Threading.Tasks.Task SendPushNotificationAsync(Uri url, object payload, CancellationToken cancellationToken = default);

}