using Microsoft.Extensions.DependencyInjection;
using Read.Infra.DataSources.AccountSummaries;
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

    protected override void Configure(IServiceCollection services) =>
        services.AddWriteDependencies().AddReadDependencies();

    [Theory, RandomData]
    public async Task Should_retrieve_summaries(AccountBuilder[] accounts)
    {
        this.accountRepository.Feed(accounts.Select(a => a.Build()).ToArray());

        AccountSummaryPresentation[] actual = await this.sut.All();
        actual.Should().Equal(accounts.Select(a => a.ToSummary()));
    }
}
