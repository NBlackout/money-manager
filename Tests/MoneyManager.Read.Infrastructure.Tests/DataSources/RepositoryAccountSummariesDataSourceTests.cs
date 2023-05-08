using MoneyManager.Read.Infrastructure.DataSources.AccountSummaries;
using MoneyManager.Write.Infrastructure.Repositories;

namespace MoneyManager.Read.Infrastructure.Tests.DataSources;

public sealed class RepositoryAccountSummariesDataSourceTests : IDisposable
{
    private readonly IHost host;
    private readonly RepositoryAccountSummariesDataSource sut;
    private readonly InMemoryBankRepository bankRepository;
    private readonly InMemoryAccountRepository accountRepository;

    public RepositoryAccountSummariesDataSourceTests()
    {
        this.host = Host.CreateDefaultBuilder()
            .ConfigureServices(services => services.AddWriteDependencies().AddReadDependencies())
            .Build();
        this.sut = this.host.Service<IAccountSummariesDataSource, RepositoryAccountSummariesDataSource>();
        this.bankRepository = this.host.Service<IBankRepository, InMemoryBankRepository>();
        this.accountRepository = this.host.Service<IAccountRepository, InMemoryAccountRepository>();

        this.bankRepository.Clear();
        this.accountRepository.Clear();
    }

    [Fact]
    public async Task Should_retrieve_tracked_account_summaries()
    {
        Guid aBankId = Guid.NewGuid();
        Guid anotherBankId = Guid.NewGuid();
        AccountBuilder checking = AccountBuilder.For(Guid.NewGuid()) with { BankId = aBankId, Tracked = true };
        AccountBuilder saving = AccountBuilder.For(Guid.NewGuid()) with { BankId = aBankId, Tracked = true };
        AccountBuilder notTracked = AccountBuilder.For(Guid.NewGuid()) with { BankId = anotherBankId, Tracked = false };
        this.bankRepository.Feed(checking.BuildBank(), notTracked.BuildBank());
        this.accountRepository.Feed(checking.Build(), saving.Build(), notTracked.Build());

        IReadOnlyCollection<AccountSummaryPresentation> actual = await this.sut.Get();
        actual.Should().Equal(checking.ToSummary(), saving.ToSummary(), notTracked.ToSummary());
    }

    public void Dispose() =>
        this.host.Dispose();
}