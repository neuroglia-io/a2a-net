namespace A2A.Server.Services;

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
    /// <returns>A new awaitable <see cref="Task"/></returns>
    Task SendPushNotificationAsync(Uri url, object payload, CancellationToken cancellationToken = default);

}