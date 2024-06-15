using Client.Read.Infra.Gateways.Account;

namespace Client.Read.App.Tests.UseCases;

public class AccountSummariesTests
{
    private readonly StubbedAccountGateway gateway = new();
    private readonly AccountSummaries sut;

    public AccountSummariesTests()
    {
        this.sut = new AccountSummaries(this.gateway);
    }

    [Theory, RandomData]
    public async Task Should_retrieve_account_summaries(AccountSummaryPresentation[] expected)
    {
        this.gateway.Feed(expected);
        AccountSummaryPresentation[] actual = await this.sut.Execute();
        actual.Should().Equal(expected);
    }
}