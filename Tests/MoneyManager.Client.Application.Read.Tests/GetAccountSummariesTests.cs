using MoneyManager.Client.Application.Read.UseCases.AccountSummaries;
using MoneyManager.Client.Infrastructure.Read.AccountSummariesGateway;

namespace MoneyManager.Client.Application.Read.Tests;

public class GetAccountSummariesTests
{
    [Fact]
    public async Task Should_retrieve_account_summaries()
    {
        AccountSummary[] expected = {
            new(Guid.NewGuid(), "Checking account", 10000.00m),
            new(Guid.NewGuid(), "Saving account", 5500.12m)
        };
        StubbedAccountSummariesGateway gateway = new(expected);
        GetAccountSummaries sut = new(gateway);

        IReadOnlyCollection<AccountSummary> actual = await sut.Execute();

        actual.Should().Equal(expected);
    }
}