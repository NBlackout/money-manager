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
    public async Task Retrieves_account_summaries(AccountSummaryPresentation[] expected)
    {
        this.Fed(expected);
        await this.Verify(expected);
    }

    private async Task Verify(AccountSummaryPresentation[] expected)
    {
        AccountSummaryPresentation[] actual = await this.sut.Execute();
        actual.Should().Equal(expected);
    }

    private void Fed(AccountSummaryPresentation[] expected) =>
        this.gateway.Feed(expected);
}