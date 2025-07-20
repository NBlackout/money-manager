using Write.Infra.Repositories;

namespace Read.Infra.Tests.DataSources;

public class InMemoryAccountSummariesDataSourceTests : InfraTest<IAccountSummariesDataSource, InMemoryAccountSummariesDataSource>
{
    private readonly InMemoryAccountRepository accountRepository;

    public InMemoryAccountSummariesDataSourceTests()
    {
        this.accountRepository = this.Resolve<IAccountRepository, InMemoryAccountRepository>();
    }

    [Theory]
    [RandomData]
    public async Task Retrieves_summaries(AccountBuilder[] accounts)
    {
        this.Feed(accounts);
        await this.Verify(accounts);
    }

    private async Task Verify(AccountBuilder[] accounts)
    {
        AccountSummaryPresentation[] actual = await this.Sut.All();
        actual.Should().Equal(accounts.Select(a => a.ToSummary()).ToArray());
    }

    private void Feed(AccountBuilder[] accounts) =>
        this.accountRepository.Feed([..accounts.Select(a => a.ToSnapshot())]);
}