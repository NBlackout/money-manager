using MoneyManager.Read.Infrastructure.DataSources.TransactionsOfMonth;
using MoneyManager.Write.Infrastructure.Repositories;

namespace MoneyManager.Read.Infrastructure.Tests.DataSources;

public sealed class RepositoryTransactionsOfMonthDataSourceTests : IDisposable
{
    private readonly IHost host;
    private readonly RepositoryTransactionsOfMonthDataSource sut;
    private readonly InMemoryTransactionRepository repository;

    public RepositoryTransactionsOfMonthDataSourceTests()
    {
        this.host = Host.CreateDefaultBuilder()
            .ConfigureServices(services => services.AddWriteDependencies().AddReadDependencies())
            .Build();
        this.sut = this.host.Service<ITransactionsOfMonthDataSource, RepositoryTransactionsOfMonthDataSource>();
        this.repository = this.host.Service<ITransactionRepository, InMemoryTransactionRepository>();

        this.repository.Clear();
    }

    [Fact]
    public async Task Should_retrieve_transactions_of_month()
    {
        Guid accountId = Guid.NewGuid();
        Guid anotherAccountId = Guid.NewGuid();

        TransactionBuilder transactionBefore = SomeTransaction(Guid.NewGuid(), accountId, DateTime.Parse("2023-03-31"));
        TransactionBuilder aTransactionThisMonth =
            SomeTransaction(Guid.NewGuid(), accountId, DateTime.Parse("2023-04-03"));
        TransactionBuilder anotherTransactionThisMonth =
            SomeTransaction(Guid.NewGuid(), accountId, DateTime.Parse("2023-04-16"));
        TransactionBuilder transactionAfter = SomeTransaction(Guid.NewGuid(), accountId, DateTime.Parse("2023-05-01"));
        TransactionBuilder transactionOfAnotherAccount =
            SomeTransaction(Guid.NewGuid(), anotherAccountId, DateTime.Parse("2023-04-21"));
        this.repository.Feed(transactionBefore.Build(), aTransactionThisMonth.Build(),
            anotherTransactionThisMonth.Build(), transactionAfter.Build(), transactionOfAnotherAccount.Build());

        IReadOnlyCollection<TransactionSummaryPresentation> actual = await this.sut.Get(accountId, 2023, 04);
        actual.Should().Equal(aTransactionThisMonth.ToSummary(), anotherTransactionThisMonth.ToSummary());
    }

    public void Dispose() =>
        this.host.Dispose();

    private static TransactionBuilder SomeTransaction(Guid id, Guid accountId, DateTime date) =>
        TransactionBuilder.For(id) with { AccountId = accountId, Date = date };
}