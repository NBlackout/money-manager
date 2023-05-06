using MoneyManager.Client.Read.Application.UseCases.AccountSummaries;
using MoneyManager.Client.Read.Infrastructure.Gateways.AccountSummaries;
using MoneyManager.Shared.Presentation;

namespace MoneyManager.Client.Read.Application.Tests.UseCases;

public class AccountSummariesTests
{
    [Fact]
    public async Task Should_retrieve_account_summaries()
    {
        AccountSummaryPresentation[] expected =
        {
            new(Guid.NewGuid(), Guid.NewGuid(), "Another bank", "Checking account", 10000.00m, DateTime.Now, false),
            new(Guid.NewGuid(), Guid.NewGuid(), "Bank", "Saving account", 5500.12m, DateTime.Now.AddDays(3), true)
        };
        StubbedAccountSummariesGateway gateway = new(expected);
        AccountSummaries sut = new(gateway);

        IReadOnlyCollection<AccountSummaryPresentation> actual = await sut.Execute();

        actual.Should().Equal(expected);
    }
}