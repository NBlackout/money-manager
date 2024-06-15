using Client.Read.Infra.Gateways.Account;

namespace Client.Read.App.Tests.UseCases;

public class TransactionsOfMonthTests
{
    private readonly StubbedAccountGateway gateway = new();
    private readonly TransactionsOfMonth sut;

    public TransactionsOfMonthTests()
    {
        this.sut = new TransactionsOfMonth(this.gateway);
    }

    [Theory, RandomData]
    public async Task Should_retrieve_transactions_of_month(
        Guid accountId,
        int year,
        int month,
        TransactionSummaryPresentation[] expected)
    {
        this.gateway.Feed(accountId, year, month, expected);
        TransactionSummaryPresentation[] actual = await this.sut.Execute(accountId, year, month);
        actual.Should().Equal(expected);
    }
}