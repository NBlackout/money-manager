using MoneyManager.Client.Read.Infrastructure.Gateways.Account;

namespace MoneyManager.Client.Read.Application.Tests.UseCases;

public class AccountSummariesTests
{
    [Fact]
    public async Task Should_retrieve_account_summaries()
    {
        AccountSummaryPresentation[] expected =
        {
            new(Guid.NewGuid(), Guid.NewGuid(), "Checking account", "014FZ3", 10000.00m, DateTime.Now,
                false),
            new(Guid.NewGuid(), Guid.NewGuid(), "Saving account", "DSFP348324V94", 5500.12m,
                DateTime.Now.AddDays(3), true)
        };
        StubbedAccountGateway gateway = new(expected);
        AccountSummaries sut = new(gateway);

        IReadOnlyCollection<AccountSummaryPresentation> actual = await sut.Execute();

        actual.Should().Equal(expected);
    }
}