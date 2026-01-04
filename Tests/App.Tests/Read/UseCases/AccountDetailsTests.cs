using App.Read.Ports;
using App.Read.UseCases.Accounts;
using App.Tests.Read.TestDoubles;

namespace App.Tests.Read.UseCases;

public class AccountDetailsTests
{
    private readonly StubbedAccountDetailsDataSource dataSource = new();
    private readonly AccountDetails sut;

    public AccountDetailsTests()
    {
        this.sut = new AccountDetails(this.dataSource);
    }

    [Theory, RandomData]
    public async Task Gives_account_details(AccountDetailsPresentation expected)
    {
        this.Feed(expected);
        await this.Verify(expected);
    }

    private async Task Verify(AccountDetailsPresentation expected)
    {
        AccountDetailsPresentation actual = await this.sut.Execute(expected.Id);
        actual.Should().Be(expected);
    }

    private void Feed(AccountDetailsPresentation expected) =>
        this.dataSource.Feed(expected.Id, expected);
}