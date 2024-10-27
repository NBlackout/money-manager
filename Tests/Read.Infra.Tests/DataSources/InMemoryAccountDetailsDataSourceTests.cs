using Write.Infra.Repositories;

namespace Read.Infra.Tests.DataSources;

public sealed class InMemoryAccountDetailsDataSourceTests : HostFixture
{
    private readonly InMemoryAccountDetailsDataSource sut;
    private readonly InMemoryAccountRepository accountRepository;

    public InMemoryAccountDetailsDataSourceTests()
    {
        this.sut = this.Resolve<IAccountDetailsDataSource, InMemoryAccountDetailsDataSource>();
        this.accountRepository = this.Resolve<IAccountRepository, InMemoryAccountRepository>();
    }

    [Theory, RandomData]
    public async Task Gives_account_details(AccountBuilder account)
    {
        this.Feed(account);
        await this.Verify(account);
    }

    private async Task Verify(AccountBuilder account)
    {
        AccountDetailsPresentation actual = await this.sut.By(account.Id);
        actual.Should().Be(account.ToDetails());
    }

    private void Feed(AccountBuilder account)
    {
        this.accountRepository.Feed(account.ToSnapshot());
    }
}