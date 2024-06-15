using Microsoft.Extensions.DependencyInjection;
using Read.Infra.DataSources.TransactionsOfMonth;
using Write.Infra.Repositories;

namespace Read.Infra.Tests.DataSources;

public sealed class RepositoryTransactionsOfMonthDataSourceTests : HostFixture
{
    private readonly RepositoryTransactionsOfMonthDataSource sut;
    private readonly InMemoryTransactionRepository transactionRepository;
    private readonly InMemoryCategoryRepository categoryRepository;

    public RepositoryTransactionsOfMonthDataSourceTests()
    {
        this.sut = this.Resolve<ITransactionsOfMonthDataSource, RepositoryTransactionsOfMonthDataSource>();
        this.transactionRepository = this.Resolve<ITransactionRepository, InMemoryTransactionRepository>();
        this.categoryRepository = this.Resolve<ICategoryRepository, InMemoryCategoryRepository>();

        this.transactionRepository.Clear();
        this.categoryRepository.Clear();
    }

    protected override void Configure(IServiceCollection services) =>
        services.AddWriteDependencies().AddReadDependencies();

    [Theory, RandomData]
    public async Task Should_retrieve_transactions_of_month(Guid accountId, Guid anotherAccountId)
    {
        TransactionBuilder transactionBefore = SomeTransaction(accountId, DateTime.Parse("2023-03-31"));
        TransactionBuilder aTransactionThisMonth =
            SomeTransaction(accountId, DateTime.Parse("2023-04-03"), Randomizer.Any<CategoryBuilder>());
        TransactionBuilder anotherTransactionThisMonth = SomeTransaction(accountId, DateTime.Parse("2023-04-16"));
        TransactionBuilder transactionAfter = SomeTransaction(accountId, DateTime.Parse("2023-05-01"));
        TransactionBuilder transactionOfAnotherAccount =
            SomeTransaction(anotherAccountId, DateTime.Parse("2023-04-21"));
        this.Feed(transactionBefore, aTransactionThisMonth, anotherTransactionThisMonth, transactionAfter,
            transactionOfAnotherAccount);

        TransactionSummaryPresentation[] actual = await this.sut.Get(accountId, 2023, 04);
        actual.Should().Equal(aTransactionThisMonth.ToSummary(), anotherTransactionThisMonth.ToSummary());
    }

    private void Feed(params TransactionBuilder[] transactions)
    {
        foreach (TransactionBuilder transaction in transactions)
        {
            if (transaction.Category != null)
                this.categoryRepository.Feed(transaction.Category.Build());

            this.transactionRepository.Feed(transaction.Build());
        }
    }

    private static TransactionBuilder
        SomeTransaction(Guid accountId, DateTime date, CategoryBuilder? category = null) =>
        TransactionBuilder.Create() with { AccountId = accountId, Date = date, Category = category };
}