using Read.App.UseCases;
using Read.Infra.DataSources.TransactionsOfMonth;
using Read.TestTooling;
using Shared.Presentation;

namespace Read.App.Tests.UseCases;

public class TransactionsOfMonthTests
{
    private readonly StubbedTransactionsOfMonthDataSource dataSource = new();
    private readonly TransactionsOfMonth sut;

    public TransactionsOfMonthTests()
    {
        this.sut = new TransactionsOfMonth(this.dataSource);
    }

    [Fact]
    public async Task Should_retrieve_transactions_of_month()
    {
        Guid accountId = Guid.NewGuid();
        const int year = 2424;
        const int month = 5;
        TransactionSummaryPresentation[] expected =
        {
            TransactionBuilder.For(Guid.NewGuid()).ToSummary(),
            TransactionBuilder.For(Guid.NewGuid()).ToSummary()
        };
        this.dataSource.Feed(accountId, year, month, expected);

        IReadOnlyCollection<TransactionSummaryPresentation> actual = await this.sut.Execute(accountId, year, month);
        actual.Should().Equal(expected);
    }
}