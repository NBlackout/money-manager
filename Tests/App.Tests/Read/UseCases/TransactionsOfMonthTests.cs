using App.Read.Ports;
using App.Read.UseCases;
using App.Tests.Read.TestDoubles;

namespace App.Tests.Read.UseCases;

public class TransactionsOfMonthTests
{
    private readonly StubbedTransactionsOfMonthDataSource dataSource = new();
    private readonly TransactionsOfMonth sut;

    public TransactionsOfMonthTests() =>
        this.sut = new TransactionsOfMonth(this.dataSource);

    [Theory]
    [RandomData]
    public async Task Gives_transactions_of_month(Guid accountId, int year, int month, TransactionSummaryPresentation[] expected)
    {
        this.Feed(accountId, year, month, expected);
        await this.Verify(accountId, year, month, expected);
    }

    private async Task Verify(Guid accountId, int year, int month, TransactionSummaryPresentation[] expected)
    {
        TransactionSummaryPresentation[] actual = await this.sut.Execute(accountId, year, month);
        actual.Should().Equal(expected);
    }

    private void Feed(Guid accountId, int year, int month, TransactionSummaryPresentation[] expected) =>
        this.dataSource.Feed(accountId, year, month, expected);
}