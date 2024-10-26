using Write.Infra.Repositories;

namespace Read.Infra.Tests.DataSources;

public sealed class InMemoryAccountSummariesDataSourceTests : HostFixture
{
    private readonly InMemoryAccountSummariesDataSource sut;
    private readonly InMemoryAccountRepository accountRepository;

    public InMemoryAccountSummariesDataSourceTests()
    {
        this.sut = this.Resolve<IAccountSummariesDataSource, InMemoryAccountSummariesDataSource>();
        this.accountRepository = this.Resolve<IAccountRepository, InMemoryAccountRepository>();
    }

    [Theory, RandomData]
    public async Task Retrieves_summaries(AccountBuilder[] accounts)
    {
        this.Feed(accounts);
        await this.Verify(accounts);
    }

    private async Task Verify(AccountBuilder[] accounts)
    {
        AccountSummaryPresentation[] actual = await this.sut.All();
        actual.Should().Equal(accounts.Select(a => a.ToSummary()));
    }

    private void Feed(AccountBuilder[] accounts) =>
        this.accountRepository.Feed([..accounts.Select(a => a.ToSnapshot())]);
}