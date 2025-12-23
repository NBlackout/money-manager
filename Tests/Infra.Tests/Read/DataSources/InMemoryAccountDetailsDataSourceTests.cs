using App.Read.Ports;
using App.Tests.Read.Tooling;
using App.Write.Ports;
using Infra.Read.DataSources;
using Infra.Tests.Tooling;
using Infra.Write.Repositories;

namespace Infra.Tests.Read.DataSources;

public class InMemoryAccountDetailsDataSourceTests : InfraTest<IAccountDetailsDataSource, InMemoryAccountDetailsDataSource>
{
    private readonly InMemoryAccountRepository accountRepository;

    public InMemoryAccountDetailsDataSourceTests()
    {
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
        AccountDetailsPresentation actual = await this.Sut.By(account.Id);
        actual.Should().Be(account.ToDetails());
    }

    private void Feed(AccountBuilder account) =>
        this.accountRepository.Feed(account.ToSnapshot());
}