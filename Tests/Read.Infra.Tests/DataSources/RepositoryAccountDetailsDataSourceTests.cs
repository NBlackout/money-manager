using Microsoft.Extensions.DependencyInjection;
using Read.Infra.DataSources.AccountDetails;
using Write.Infra.Repositories;

namespace Read.Infra.Tests.DataSources;

public sealed class RepositoryAccountDetailsDataSourceTests : HostFixture
{
    private readonly RepositoryAccountDetailsDataSource sut;
    private readonly InMemoryAccountRepository repository;

    public RepositoryAccountDetailsDataSourceTests()
    {
        this.sut = this.Resolve<IAccountDetailsDataSource, RepositoryAccountDetailsDataSource>();
        this.repository = this.Resolve<IAccountRepository, InMemoryAccountRepository>();

        this.repository.Clear();
    }

    protected override void Configure(IServiceCollection services) =>
        services.AddWriteDependencies().AddReadDependencies();

    [Theory, RandomData]
    public async Task Should_retrieve_account_details(AccountBuilder account)
    {
        this.repository.Feed(account.Build());
        AccountDetailsPresentation actual = await this.sut.Get(account.Id);
        actual.Should().Be(account.ToDetails());
    }
}