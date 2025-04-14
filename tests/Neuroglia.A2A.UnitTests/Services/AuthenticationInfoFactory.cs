namespace Neuroglia.A2A.UnitTests.Services;

internal static class AuthenticationInfoFactory
{

    internal static AuthenticationInfo Create() => new()
    {
        Schemes = ["OAuth2"],
        Credentials = "fake-credentials"
    };

}
