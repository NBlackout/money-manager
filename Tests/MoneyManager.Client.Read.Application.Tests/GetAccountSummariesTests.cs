using MoneyManager.Client.Read.Application.UseCases.AccountSummaries;
using MoneyManager.Client.Read.Infrastructure.AccountSummariesGateway;

namespace MoneyManager.Client.Read.Application.Tests;

public class GetAccountSummariesTests
{
    [Fact]
    public async Task Should_retrieve_account_summaries()
    {
        AccountSummary[] expected =
        {
            new(Guid.NewGuid(), "Checking account", 10000.00m, false),
            new(Guid.NewGuid(), "Saving account", 5500.12m, true)
        };
        StubbedAccountSummariesGateway gateway = new(expected);
        GetAccountSummaries sut = new(gateway);

        IReadOnlyCollection<AccountSummary> actual = await sut.Execute();

        actual.Should().Equal(expected);
    }
}