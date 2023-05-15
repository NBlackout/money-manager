using MoneyManager.Client.Read.Infrastructure.Gateways.Account;

namespace MoneyManager.Client.Read.Application.Tests.UseCases;

public class AccountDetailsTests
{
    private readonly StubbedAccountGateway gateway;
    private readonly AccountDetails sut;

    public AccountDetailsTests()
    {
        this.gateway = new StubbedAccountGateway();
        this.sut = new AccountDetails(this.gateway);
    }

    [Fact]
    public async Task Should_retrieve_account_details()
    {
        AccountDetailsPresentation expected = new(Guid.NewGuid(), "Big bucks?", "Number", 1.84m, DateTime.Parse("2023-11-28"));
        this.gateway.Feed(expected.Id, expected);

        AccountDetailsPresentation actual = await this.sut.Execute(expected.Id);
        actual.Should().Be(expected);
    }
}