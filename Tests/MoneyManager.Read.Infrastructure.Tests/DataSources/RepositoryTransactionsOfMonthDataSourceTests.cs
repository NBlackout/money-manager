using MoneyManager.Read.Infrastructure.DataSources.TransactionsOfMonth;
using MoneyManager.Write.Infrastructure.Repositories;

namespace MoneyManager.Read.Infrastructure.Tests.DataSources;

public sealed class RepositoryTransactionsOfMonthDataSourceTests : IDisposable
{
    private readonly IHost host;
    private readonly RepositoryTransactionsOfMonthDataSource sut;
    private readonly InMemoryTransactionRepository transactionRepository;
    private readonly InMemoryCategoryRepository categoryRepository;

    public RepositoryTransactionsOfMonthDataSourceTests()
    {
        this.host = Host.CreateDefaultBuilder()
            .ConfigureServices(services => services.AddWriteDependencies().AddReadDependencies())
            .Build();
        this.sut = this.host.Service<ITransactionsOfMonthDataSource, RepositoryTransactionsOfMonthDataSource>();
        this.transactionRepository = this.host.Service<ITransactionRepository, InMemoryTransactionRepository>();
        this.categoryRepository = this.host.Service<ICategoryRepository, InMemoryCategoryRepository>();

        this.transactionRepository.Clear();
    }

    [Fact]
    public async Task Should_retrieve_transactions_of_month()
    {
        Guid accountId = Guid.NewGuid();
        Guid anotherAccountId = Guid.NewGuid();

        TransactionBuilder transactionBefore = SomeTransaction(Guid.NewGuid(), accountId, DateTime.Parse("2023-03-31"));
        TransactionBuilder aTransactionThisMonth = SomeTransaction(Guid.NewGuid(), accountId,
            DateTime.Parse("2023-04-03"), CategoryBuilder.For(Guid.Parse("2EE59E6A-C71C-44A2-8A9C-10587BF97FBB"))
        );
        TransactionBuilder anotherTransactionThisMonth =
            SomeTransaction(Guid.NewGuid(), accountId, DateTime.Parse("2023-04-16"), null);
        TransactionBuilder transactionAfter = SomeTransaction(Guid.NewGuid(), accountId, DateTime.Parse("2023-05-01"));
        TransactionBuilder transactionOfAnotherAccount =
            SomeTransaction(Guid.NewGuid(), anotherAccountId, DateTime.Parse("2023-04-21"));
        this.Feed(transactionBefore, aTransactionThisMonth, anotherTransactionThisMonth, transactionAfter,
            transactionOfAnotherAccount);

        IReadOnlyCollection<TransactionSummaryPresentation> actual = await this.sut.Get(accountId, 2023, 04);
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

    public void Dispose() =>
        this.host.Dispose();

    private static TransactionBuilder SomeTransaction(Guid id, Guid accountId, DateTime date) =>
        SomeTransaction(id, accountId, date, null);

    private static TransactionBuilder SomeTransaction(Guid id, Guid accountId, DateTime date,
        CategoryBuilder? category) =>
        TransactionBuilder.For(id) with { AccountId = accountId, Date = date, Category = category };
}