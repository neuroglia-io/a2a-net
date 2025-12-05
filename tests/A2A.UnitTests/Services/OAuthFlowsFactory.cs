namespace A2A.UnitTests.Services;

internal static class OAuthFlowsFactory
{

    internal static OAuthFlows Create() => new()
    {
        AuthorizationCode = OAuthFlowFactory.CreateAuthorizationCodeFlow(),
        ClientCredentials = OAuthFlowFactory.CreateClientCredentialsFlow(),
        Implicit = OAuthFlowFactory.CreateImplicitFlow(),
        Password = OAuthFlowFactory.CreatePasswordFlow()
    };

}
