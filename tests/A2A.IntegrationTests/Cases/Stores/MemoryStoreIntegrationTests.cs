namespace A2A.IntegrationTests.Cases.Stores;

public sealed class MemoryStoreIntegrationTests
    : A2AStoreIntegrationTestsBase<MemoryStore>
{

    MemoryStateStoreOptions options = null!;

    protected override Task<MemoryStore> CreateStoreAsync()
    {
        options = new MemoryStateStoreOptions
        {
            DefaultPageSize = 10,
            MaxPageSize = 50
        };
        return Task.FromResult(new MemoryStore(Options.Create(options)));
    }

}