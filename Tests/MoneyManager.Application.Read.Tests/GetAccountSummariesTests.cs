using MoneyManager.Application.Read.UseCases;
using MoneyManager.Infrastructure.Read.DataSources.AccountSummaries;

namespace MoneyManager.Application.Read.Tests;

public class GetAccountSummariesTests
{
    [Fact]
    public async Task Should_retrieve_account_summaries()
    {
        AccountSummary[] expected =
        {
            new(Guid.NewGuid(), "A label", 12.34m, true),
            new(Guid.NewGuid(), "Another label", 56.78m, false)
        };
        GetAccountSummaries sut = new(new StubbedAccountSummariesDataSource(expected));

        IReadOnlyCollection<AccountSummary> actual = await sut.Execute();

        actual.Should().Equal(expected);
    }
}