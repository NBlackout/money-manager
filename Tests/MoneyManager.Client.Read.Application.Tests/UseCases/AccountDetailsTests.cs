using MoneyManager.Client.Read.Infrastructure.Gateways;

namespace MoneyManager.Client.Read.Application.Tests.UseCases;

public class AccountDetailsTests
{
    [Fact]
    public async Task Should_retrieve_account_details()
    {
        StubbedAccountDetailsGateway gateway = new();
        AccountDetails sut = new(gateway);

        AccountDetailsPresentation expected = new(Guid.NewGuid(), "Big bucks?", 1.84m,
            new TransactionSummary(Guid.NewGuid(), 111.42m, "My payment"));
        gateway.Feed(expected.Id, expected);

        AccountDetailsPresentation actual = await sut.Execute(expected.Id);
        actual.Should().Be(expected);
    }
}