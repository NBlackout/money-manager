using Read.App.UseCases;
using Read.Infra.DataSources.AccountSummaries;
using Read.TestTooling;
using Shared.Presentation;

namespace Read.App.Tests.UseCases;

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