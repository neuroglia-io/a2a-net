using AwesomeAssertions.Primitives;

#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace A2A.UnitTests;

public static class ObjectAssertionsExtensions
{

    public static void BeJsonEquivalentTo<T>(this ObjectAssertions should, T expected)
    {
        should.BeEquivalentTo(expected, opts => opts
            .Using<JsonNode>(ctx =>
                ctx.Subject?.ToJsonString().Should().Be(ctx.Expectation?.ToJsonString()))
                .WhenTypeIs<JsonNode>()
            .Using<ReadOnlyMemory<byte>>(ctx =>
                ctx.Subject.ToArray().Should().BeEquivalentTo(ctx.Expectation.ToArray()))
                .WhenTypeIs<ReadOnlyMemory<byte>>());
    }

}
