namespace A2A.UnitTests.Services;

internal static class AuthenticationInfoFactory
{

    internal static AuthenticationInfo Create() => new()
    {
        Schemes = [ "Scheme1", "Scheme2" ],
        Credentials = "SampleCredentials"
    };

}