using MoneyManager.Read.Application.UseCases;
using MoneyManager.Read.Infrastructure.DataSources.AccountSummaries;

namespace MoneyManager.Read.Application.Tests;

public class GetAccountSummariesTests
{
    [Fact]
    public async Task Should_retrieve_account_summaries()
    {
        AccountSummary[] expected =
        {
            new(Guid.NewGuid(), "It's a bank", "A label", 12.34m, true),
            new(Guid.NewGuid(), "Big bank", "Another label", 56.78m, false)
        };
        GetAccountSummaries sut = new(new StubbedAccountSummariesDataSource(expected));

        IReadOnlyCollection<AccountSummary> actual = await sut.Execute();

        actual.Should().Equal(expected);
    }
}