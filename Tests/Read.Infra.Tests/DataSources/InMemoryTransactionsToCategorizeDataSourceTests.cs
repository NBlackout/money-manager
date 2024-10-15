﻿using Write.Infra.Repositories;

namespace Read.Infra.Tests.DataSources;

public sealed class InMemoryTransactionsToCategorizeDataSourceTests : HostFixture
{
    private readonly InMemoryTransactionsToCategorizeDataSource sut;
    private readonly InMemoryTransactionRepository categoryRepository;

    public InMemoryTransactionsToCategorizeDataSourceTests()
    {
        this.sut = this.Resolve<ITransactionsToCategorizeDataSource, InMemoryTransactionsToCategorizeDataSource>();
        this.categoryRepository = this.Resolve<ITransactionRepository, InMemoryTransactionRepository>();
    }

    [Fact]
    public async Task Retrieves_transactions_not_already_categorized()
    {
        TransactionBuilder[] expected = [ATransactionWithoutCategory(), ATransactionWithoutCategory()];
        this.Feed(expected);
        await this.Verify(expected);
    }

    [Fact]
    public async Task Excludes_ones_already_categorized()
    {
        this.Feed(ATransactionWithCategory());
        await this.Verify([]);
    }

    private async Task Verify(params TransactionBuilder[] expected)
    {
        TransactionToCategorize[] actual = await this.sut.All();
        actual.Should().Equal(expected.Select(t => new TransactionToCategorize(t.Id, t.Label, t.Amount)));
    }

    private void Feed(params TransactionBuilder[] categories) =>
        this.categoryRepository.Feed(categories.Select(c => c.ToSnapshot()).ToArray());

    private static TransactionBuilder ATransactionWithoutCategory() =>
        Any<TransactionBuilder>() with { Category = null };

    private static TransactionBuilder ATransactionWithCategory() =>
        Any<TransactionBuilder>() with { Category = Any<CategoryBuilder>() };
}