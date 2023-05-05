using MoneyManager.Read.Application.UseCases;
using MoneyManager.Read.Infrastructure.DataSources.AccountSummaries;
using MoneyManager.Shared.Presentation;

namespace MoneyManager.Read.Application.Tests.UseCases;

public class GetAccountSummariesTests
{
    [Fact]
    public async Task Should_retrieve_account_summaries()
    {
        AccountSummaryPresentation[] expected =
        {
            new(Guid.NewGuid(), Guid.NewGuid(), "It's a bank", "A label", 12.34m, true),
            new(Guid.NewGuid(), Guid.NewGuid(), "Big bank", "Another label", 56.78m, false)
        };
        GetAccountSummaries sut = new(new StubbedAccountSummariesDataSource(expected));

        IReadOnlyCollection<AccountSummaryPresentation> actual = await sut.Execute();

        actual.Should().Equal(expected);
    }
}