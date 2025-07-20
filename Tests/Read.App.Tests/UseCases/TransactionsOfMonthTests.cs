namespace Read.App.Tests.UseCases;

public class TransactionsOfMonthTests
{
    private readonly StubbedTransactionsOfMonthDataSource dataSource = new();
    private readonly TransactionsOfMonth sut;

    public TransactionsOfMonthTests()
    {
        this.sut = new TransactionsOfMonth(this.dataSource);
    }

    [Theory]
    [RandomData]
    public async Task Gives_transactions_of_month(Guid accountId, int year, int month, TransactionSummaryPresentation[] expected)
    {
        this.dataSource.Feed(accountId, year, month, expected);
        TransactionSummaryPresentation[] actual = await this.sut.Execute(accountId, year, month);
        actual.Should().Equal(expected);
    }
}