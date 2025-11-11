using App.Read.Ports;
using App.Tests.Read.Tooling;
using App.Write.Ports;
using Infra.Read.DataSources;
using Infra.Tests.Tooling;
using Infra.Write.Repositories;

namespace Infra.Tests.Read.DataSources;

public class InMemoryExpectedTransactionSummariesDataSourceTests : InfraTest<IExpectedTransactionSummariesDataSource, InMemoryExpectedTransactionSummariesDataSource>
{
    private readonly InMemoryTransactionRepository transactionRepository;

    public InMemoryExpectedTransactionSummariesDataSourceTests()
    {
        this.transactionRepository = this.Resolve<ITransactionRepository, InMemoryTransactionRepository>();
    }

    [Fact]
    public async Task Gives_recurring_transactions()
    {
        TransactionBuilder aNotRecurringTransaction = ATransaction() with { IsRecurring = false };
        TransactionBuilder aRecurringTransaction = ATransaction() with { IsRecurring = true };
        this.Feed(aNotRecurringTransaction, aRecurringTransaction);

        await this.Verify([ExpectedFrom(aRecurringTransaction)]);
    }

    private async Task Verify(ExpectedTransactionSummaryPresentation[] expected)
    {
        ExpectedTransactionSummaryPresentation[] actual = await this.Sut.All();
        actual.Should().Equal(expected);
    }

    private void Feed(params TransactionBuilder[] expected) =>
        this.transactionRepository.Feed([..expected.Select(c => c.ToSnapshot())]);

    private static TransactionBuilder ATransaction() =>
        Any<TransactionBuilder>();

    private static ExpectedTransactionSummaryPresentation ExpectedFrom(TransactionBuilder transaction) =>
        new(transaction.Amount, transaction.Label);
}