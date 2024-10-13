using Read.Infra.DataSources.AccountDetails;
using Shared.Infra.TestTooling;
using Write.Infra.Repositories;

namespace Read.Infra.Tests.DataSources;

public sealed class RepositoryAccountDetailsDataSourceTests : HostFixture
{
    private readonly RepositoryAccountDetailsDataSource sut;
    private readonly InMemoryAccountRepository accountRepository;

    public RepositoryAccountDetailsDataSourceTests()
    {
        this.sut = this.Resolve<IAccountDetailsDataSource, RepositoryAccountDetailsDataSource>();
        this.accountRepository = this.Resolve<IAccountRepository, InMemoryAccountRepository>();
    }

    [Theory, RandomData]
    public async Task Retrieves_account_details(AccountBuilder account)
    {
        this.accountRepository.Feed(account.ToSnapshot());
        AccountDetailsPresentation actual = await this.sut.By(account.Id);
        actual.Should().Be(account.ToDetails());
    }
}
