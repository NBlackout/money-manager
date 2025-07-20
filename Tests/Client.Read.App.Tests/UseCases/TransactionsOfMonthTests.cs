namespace Client.Read.App.Tests.UseCases;

public class TransactionsOfMonthTests
{
    private readonly StubbedAccountGateway gateway = new();
    private readonly TransactionsOfMonth sut;

    public TransactionsOfMonthTests()
    {
        this.sut = new TransactionsOfMonth(this.gateway);
    }

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
        this.gateway.Feed(accountId, year, month, expected);
}