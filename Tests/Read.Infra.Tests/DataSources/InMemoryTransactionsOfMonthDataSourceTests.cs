using Write.Infra.Repositories;

namespace Read.Infra.Tests.DataSources;

public class
    InMemoryTransactionsOfMonthDataSourceTests : InfraTest<ITransactionsOfMonthDataSource,
    InMemoryTransactionsOfMonthDataSource>
{
    private readonly InMemoryTransactionRepository transactionRepository;
    private readonly InMemoryCategoryRepository categoryRepository;

    public InMemoryTransactionsOfMonthDataSourceTests()
    {
        this.transactionRepository = this.Resolve<ITransactionRepository, InMemoryTransactionRepository>();
        this.categoryRepository = this.Resolve<ICategoryRepository, InMemoryCategoryRepository>();
    }

    [Theory, RandomData]
    public async Task Gives_transactions_of_month(Guid accountId)
    {
        TransactionBuilder aTransaction = ATransactionOn(accountId, DateOnly.Parse("2023-04-03"));
        TransactionBuilder anotherTransaction = ATransactionOn(accountId, DateOnly.Parse("2023-04-16"));
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
        TransactionSummaryPresentation[] actual = await this.Sut.By(accountId, year, month);
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

    private static TransactionBuilder ATransactionOn(Guid accountId, DateOnly date) =>
        Any<TransactionBuilder>() with { AccountId = accountId, Date = date };
}