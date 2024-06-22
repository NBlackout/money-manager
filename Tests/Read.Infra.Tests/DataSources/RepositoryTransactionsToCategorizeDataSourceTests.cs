using Microsoft.Extensions.DependencyInjection;
using Read.Infra.DataSources.TransactionsToCategorize;
using Write.Infra.Repositories;
using static Shared.TestTooling.Randomizer;

namespace Read.Infra.Tests.DataSources;

public sealed class RepositoryTransactionsToCategorizeDataSourceTests : HostFixture
{
    private readonly RepositoryTransactionsToCategorizeDataSource sut;
    private readonly InMemoryTransactionRepository categoryRepository;

    public RepositoryTransactionsToCategorizeDataSourceTests()
    {
        this.sut = this.Resolve<ITransactionsToCategorizeDataSource, RepositoryTransactionsToCategorizeDataSource>();
        this.categoryRepository = this.Resolve<ITransactionRepository, InMemoryTransactionRepository>();

        this.categoryRepository.Clear();
    }

    protected override void Configure(IServiceCollection services) =>
        services.AddWriteDependencies().AddReadDependencies();

    [Fact]
    public async Task Should_retrieve_transactions_not_already_categorized()
    {
        TransactionBuilder[] expected = [ATransactionWithoutCategory(), ATransactionWithoutCategory()];
        this.Feed(expected);
        await this.Verify(expected);
    }

    [Fact]
    public async Task Should_exclude_ones_already_categorized()
    {
        this.Feed(ATransactionWithCategory());
        await this.Verify(Array.Empty<TransactionBuilder>());
    }

    private async Task Verify(params TransactionBuilder[] expected)
    {
        TransactionToCategorize[] actual = await this.sut.Get();
        actual.Should().Equal(expected.Select(t => new TransactionToCategorize(t.Id, t.Label, t.Amount)));
    }

    private void Feed(params TransactionBuilder[] categories) =>
        this.categoryRepository.Feed(categories.Select(c => c.Build()).ToArray());

    private static TransactionBuilder ATransactionWithoutCategory() => 
        Any<TransactionBuilder>() with { Category = null };

    private static TransactionBuilder ATransactionWithCategory() => 
        Any<TransactionBuilder>() with { Category = Any<CategoryBuilder>() };
}