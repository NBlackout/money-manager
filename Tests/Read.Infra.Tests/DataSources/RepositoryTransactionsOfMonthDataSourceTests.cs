using Microsoft.Extensions.DependencyInjection;
using Read.Infra.DataSources.TransactionsOfMonth;
using Write.Infra.Repositories;
using static Shared.TestTooling.Randomizer;

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
    }

    protected override void Configure(IServiceCollection services) =>
        services.AddWriteDependencies().AddReadDependencies();

    [Theory, RandomData]
    public async Task Retrieves_transactions_of_month(Guid accountId)
    {
        TransactionBuilder aTransaction = ATransactionOn(accountId, DateTime.Parse("2023-04-03"));
        TransactionBuilder anotherTransaction = ATransactionOn(accountId, DateTime.Parse("2023-04-16"));
        this.Feed(aTransaction, anotherTransaction);

        await this.Verify(accountId, 2023, 04, aTransaction.ToSummary(), anotherTransaction.ToSummary());
    }

    [Theory, RandomData]
    public async Task Does_not_give_transactions_of_another_account(TransactionBuilder transaction)
    {
        this.Feed(transaction);
        await this.Verify(Another(transaction.AccountId), transaction.Date.Year, transaction.Date.Month, []);
    }

    [Theory, RandomData]
    public async Task Excludes_transactions_of_another_period(TransactionBuilder transaction)
    {
        this.Feed(transaction);
        await this.Verify(transaction.AccountId, Another(transaction.Date.Year), transaction.Date.Month, []);
        await this.Verify(transaction.AccountId, transaction.Date.Year, Another(transaction.Date.Month), []);
    }

    private async Task Verify(Guid accountId, int year, int month, params TransactionSummaryPresentation[] expected)
    {
        TransactionSummaryPresentation[] actual = await this.sut.By(accountId, year, month);
        actual.Should().Equal(expected);
    }

    private void Feed(params TransactionBuilder[] transactions)
    {
        foreach (TransactionBuilder transaction in transactions)
        {
            if (transaction.Category != null)
                this.categoryRepository.Feed(transaction.Category.ToSnapshot());

            this.transactionRepository.Feed(transaction.ToSnapshot());
        }
    }

    private static TransactionBuilder ATransactionOn(Guid accountId, DateTime date) =>
        Any<TransactionBuilder>() with { AccountId = accountId, Date = date };
}
