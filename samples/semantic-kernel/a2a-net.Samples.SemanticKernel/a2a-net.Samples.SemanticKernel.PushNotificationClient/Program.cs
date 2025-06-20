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

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddOptions<ApplicationOptions>().Bind(builder.Configuration).ValidateDataAnnotations().ValidateOnStart();
builder.Services.AddHttpClient();
var app = builder.Build();

app.MapGet("/", (HttpRequest request) =>
{
    if (request.Query.TryGetValue("validationToken", out var token)) return Results.Text(token);
    else return Results.BadRequest("Missing validationToken query param");
});
app.MapPost("/", async (HttpRequest request, HttpClient httpClient, IOptions<ApplicationOptions> options) =>
{
    var json = await httpClient.GetStringAsync(new Uri(options.Value.Server, "/.well-known/jwks.json"), request.HttpContext.RequestAborted);
    var jwks = new JsonWebKeySet(json);
    using var reader = new StreamReader(request.Body);
    var payload = await reader.ReadToEndAsync(request.HttpContext.RequestAborted);
    var token = request.Headers.Authorization.ToString()["Bearer ".Length..].Trim();
    var tokenHandler = new JsonWebTokenHandler();
    var result = await tokenHandler.ValidateTokenAsync(token, new TokenValidationParameters
    {
        ValidateIssuer = false,
        ValidateAudience = false,
        RequireSignedTokens = true,
        ValidateLifetime = true,
        IssuerSigningKeys = jwks.GetSigningKeys(),
        ValidAlgorithms = [SecurityAlgorithms.RsaSha256],
        RequireExpirationTime = false
    });
    if (result.IsValid) return Results.Ok();
    else return Results.Forbid();
});

await app.RunAsync();
