using MoneyManager.Application.Read.UseCases.AccountSummaries;
using MoneyManager.Infrastructure.Read.DataSources.AccountSummaries;

namespace MoneyManager.Application.Read.Tests;

public class GetAccountSummariesTests
{
    [Fact]
    public async Task Should_retrieve_account_summaries()
    {
        AccountSummary[] expected =
        {
            new(Guid.NewGuid(), "A label", 12.34m),
            new(Guid.NewGuid(), "Another label", 56.78m)
        };
        GetAccountSummaries sut = new(new StubbedAccountSummariesDataSource(expected));

        IReadOnlyCollection<AccountSummary> actual = await sut.Execute();

        actual.Should().Equal(expected);
    }
}