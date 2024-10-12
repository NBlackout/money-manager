using Client.Read.Infra.Gateways.Account;

namespace Client.Read.App.Tests.UseCases;

public class AccountDetailsTests
{
    private readonly StubbedAccountGateway gateway = new();
    private readonly AccountDetails sut;

    public AccountDetailsTests()
    {
        this.sut = new AccountDetails(this.gateway);
    }

    [Theory, RandomData]
    public async Task Retrieves_account_details(AccountDetailsPresentation expected)
    {
        this.gateway.Feed(expected.Id, expected);
        AccountDetailsPresentation actual = await this.sut.Execute(expected.Id);
        actual.Should().Be(expected);
    }
}