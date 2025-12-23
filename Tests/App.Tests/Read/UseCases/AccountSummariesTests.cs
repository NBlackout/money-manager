using App.Read.Ports;
using App.Read.UseCases;
using App.Tests.Read.TestDoubles;

namespace App.Tests.Read.UseCases;

public class AccountSummariesTests
{
    private readonly StubbedAccountSummariesDataSource dataSource = new();
    private readonly AccountSummaries sut;

    public AccountSummariesTests()
    {
        this.sut = new AccountSummaries(this.dataSource);
    }

    [Theory, RandomData]
    public async Task Gives_accounts(AccountSummaryPresentation[] expected)
    {
        this.Feed(expected);
        await this.Verify(expected);
    }

    private async Task Verify(AccountSummaryPresentation[] expected)
    {
        AccountSummaryPresentation[] actual = await this.sut.Execute();
        actual.Should().Equal(expected);
    }

    private void Feed(AccountSummaryPresentation[] expected) =>
        this.dataSource.Feed(expected);
}