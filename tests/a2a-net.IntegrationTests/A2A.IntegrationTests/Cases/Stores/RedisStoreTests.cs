namespace A2A.IntegrationTests.Cases.Stores;

public sealed class RedisStoreIntegrationTests
    : A2AStoreIntegrationTestsBase<RedisStore>
{

    readonly RedisContainer redis = new RedisBuilder()
        .WithImage("redis:7-alpine")
        .WithCleanUp(true)
        .Build();

    IConnectionMultiplexer connectionMultiplexer = null!;
    RedisStateStoreOptions options = null!;

    protected override async Task<RedisStore> CreateStoreAsync()
    {
        await redis.StartAsync();
        connectionMultiplexer = await ConnectionMultiplexer.ConnectAsync(redis.GetConnectionString());
        options = new RedisStateStoreOptions
        {
            KeyPrefix = "it:a2a:",
            DefaultPageSize = 10,
            MaxPageSize = 50
        };
        return new RedisStore(Options.Create(options), connectionMultiplexer);
    }

    public override async Task DisposeAsync()
    {
        if (connectionMultiplexer is not null) await connectionMultiplexer.DisposeAsync();
        await redis.DisposeAsync();
    }

}
