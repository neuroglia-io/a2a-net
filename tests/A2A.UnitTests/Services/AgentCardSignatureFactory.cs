namespace A2A.UnitTests.Services;

internal static class AgentCardSignatureFactory
{

    internal static AgentCardSignature Create() => new()
    {
        Protected = "protectedHeaderSample",
        Signature = "signatureSample",
        Header = new Dictionary<string, JsonNode>
        {
            ["alg"] = "RS256",
            ["typ"] = "JWT"
        }
    };

}