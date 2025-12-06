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

namespace A2A.Server.Services;

/// <summary>
/// Represents the default implementation of the <see cref="IPushNotificationSender"/> interface.
/// </summary>
public sealed class PushNotificationSender
    : IPushNotificationSender
{

    readonly ILogger<PushNotificationSender> logger;
    readonly IJsonWebKeySet jwks;
    readonly HttpClient httpClient;
    readonly RsaSecurityKey privateKey;

    /// <summary>
    /// Initializes a new <see cref="PushNotificationSender"/>.
    /// </summary>
    /// <param name="logger">The service used to perform logging</param>
    /// <param name="jwks">The service used to manage the application's JSON Web Key Set</param>
    /// <param name="httpClient">The service used to perform HTTP requests</param>
    public PushNotificationSender(ILogger<PushNotificationSender> logger, IJsonWebKeySet jwks, HttpClient httpClient)
    {
        this.logger = logger;
        this.jwks = jwks;
        this.httpClient = httpClient;
        privateKey = GeneratePrivateKey();
    }

    /// <inheritdoc/>
    public async Task<bool> VerifyPushNotificationUrlAsync(Uri url, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(url);
        var validationToken = Guid.NewGuid().ToString("N");
        var uri = new UriBuilder(url)
        {
            Query = $"validationToken={validationToken}"
        }.Uri;
        try
        {
            var token = await httpClient.GetStringAsync(uri, cancellationToken).ConfigureAwait(false);
            return token == validationToken;
        }
        catch (Exception ex)
        {
            logger.LogWarning("An error occurred while verifying the specified push-notification URL {uri}: {ex}", url, ex);
            return false;
        }
    }

    /// <inheritdoc/>
    public async Task SendPushNotificationAsync(Uri url, object task, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(url);
        ArgumentNullException.ThrowIfNull(task);
        var token = GenerateJwt(task);
        var json = JsonSerializer.Serialize(task);
        using var content = new StringContent(json, Encoding.UTF8, MediaTypeNames.Application.Json);
        using var request = new HttpRequestMessage(HttpMethod.Post, url)
        {
            Content = content
        };
        request.Headers.Authorization = new("Bearer", token);
        using var response = await httpClient.SendAsync(request, cancellationToken).ConfigureAwait(false);
        response.EnsureSuccessStatusCode();
    }

    RsaSecurityKey GeneratePrivateKey()
    {
        var rsa = RSA.Create(2048);
        var privateKey = new RsaSecurityKey(rsa)
        {
            KeyId = Guid.NewGuid().ToString("N")
        };
        var publicKey = JsonWebKeyConverter.ConvertFromRSASecurityKey(privateKey);
        publicKey.Use = JsonWebKeyUseNames.Sig;
        jwks.AddPublicKey(publicKey);
        return privateKey;
    }

    string GenerateJwt(object payload)
    {
        ArgumentNullException.ThrowIfNull(payload);
        var securityKey = new RsaSecurityKey(privateKey.Rsa);
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.RsaSha256);
        var claims = new Dictionary<string, object>
        {
            {"iat", DateTimeOffset.UtcNow.ToUnixTimeSeconds()},
            {"request_body_sha256", Hash(payload)}
        };
        var token = new SecurityTokenDescriptor()
        {
            Claims = claims,
            IssuedAt = DateTime.UtcNow,
            NotBefore = DateTime.UtcNow,
            Expires = DateTime.UtcNow.AddMinutes(360),
            SigningCredentials = credentials,
            IncludeKeyIdInHeader = true
        };
        var tokenHandler = new JsonWebTokenHandler();
        return tokenHandler.CreateToken(token);
    }

    static string Hash(object payload)
    {
        ArgumentNullException.ThrowIfNull(payload);
        var json = JsonSerializer.Serialize(payload, new JsonSerializerOptions
        {
            Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
            WriteIndented = false
        });
        var hashBytes = SHA256.HashData(Encoding.UTF8.GetBytes(json));
        return Convert.ToHexStringLower(hashBytes).Replace("-", "");
    }

}
