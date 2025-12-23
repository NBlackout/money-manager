using App.Read.Ports;
using App.Tests.Read.Tooling;
using App.Write.Ports;
using Infra.Read.DataSources;
using Infra.Tests.Tooling;
using Infra.Write.Repositories;

namespace Infra.Tests.Read.DataSources;

public class InMemoryAccountSummariesDataSourceTests : InfraTest<IAccountSummariesDataSource, InMemoryAccountSummariesDataSource>
{
    private readonly InMemoryAccountRepository accountRepository;

    public InMemoryAccountSummariesDataSourceTests()
    {
        this.accountRepository = this.Resolve<IAccountRepository, InMemoryAccountRepository>();
    }

    [Theory, RandomData]
    public async Task Gives_accounts(AccountBuilder[] accounts)
    {
        this.Feed(accounts);
        await this.Verify(accounts);
    }

    private async Task Verify(params AccountBuilder[] accounts)
    {
        AccountSummaryPresentation[] actual = await this.Sut.All();
        actual.Should().Equal(accounts.Select(a => a.ToSummary()).ToArray());
    }

    private void Feed(AccountBuilder[] accounts) =>
        this.accountRepository.Feed([..accounts.Select(a => a.ToSnapshot())]);
}