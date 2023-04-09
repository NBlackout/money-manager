using Xunit;
using FluentAssertions;
using MoneyManager.Application.Read.AccountSummaries;
using MoneyManager.Infrastructure.Read.AccountSummaries;

namespace MoneyManager.Application.Read.Tests;

public class GetAccountSummariesTests
{
    [Fact]
    public async Task Should_retrieve_account_summaries()
    {
        var expected = new[]
        {
            new AccountSummary(Guid.NewGuid(), "A label", 12.34m),
            new AccountSummary(Guid.NewGuid(), "Another label", 56.78m)
        };
        var sut = new GetAccountSummaries(new StubbedAccountSummariesDataSource(expected));

        IReadOnlyCollection<AccountSummary> actual = await sut.Handle();

        actual.Should().Equal(expected);
    }
}