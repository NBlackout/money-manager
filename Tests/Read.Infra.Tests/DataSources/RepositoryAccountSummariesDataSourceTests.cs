using Read.Infra.DataSources.AccountSummaries;
using Shared.Infra.TestTooling;
using Write.Infra.Repositories;

namespace Read.Infra.Tests.DataSources;

public sealed class RepositoryAccountSummariesDataSourceTests : HostFixture
{
    private readonly RepositoryAccountSummariesDataSource sut;
    private readonly InMemoryAccountRepository accountRepository;

    public RepositoryAccountSummariesDataSourceTests()
    {
        this.sut = this.Resolve<IAccountSummariesDataSource, RepositoryAccountSummariesDataSource>();
        this.accountRepository = this.Resolve<IAccountRepository, InMemoryAccountRepository>();
    }

    [Theory, RandomData]
    public async Task Retrieves_summaries(AccountBuilder[] accounts)
    {
        this.accountRepository.Feed(accounts.Select(a => a.ToSnapshot()).ToArray());

        AccountSummaryPresentation[] actual = await this.sut.All();
        actual.Should().Equal(accounts.Select(a => a.ToSummary()));
    }
}
