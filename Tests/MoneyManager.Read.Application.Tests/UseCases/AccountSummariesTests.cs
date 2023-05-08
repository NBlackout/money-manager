using MoneyManager.Read.Application.UseCases;
using MoneyManager.Read.Infrastructure.DataSources.AccountSummaries;
using MoneyManager.Read.TestTooling;
using MoneyManager.Shared.Presentation;

namespace MoneyManager.Read.Application.Tests.UseCases;

public class AccountSummariesTests
{
    [Fact]
    public async Task Should_retrieve_account_summaries()
    {
        AccountSummaryPresentation[] expected =
        {
            AccountBuilder.For(Guid.NewGuid()).ToSummary(), AccountBuilder.For(Guid.NewGuid()).ToSummary()
        };
        AccountSummaries sut = new(new StubbedAccountSummariesDataSource(expected));

        IReadOnlyCollection<AccountSummaryPresentation> actual = await sut.Execute();

        actual.Should().Equal(expected);
    }
}