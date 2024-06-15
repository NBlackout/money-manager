using Microsoft.Extensions.DependencyInjection;
using Read.Infra.DataSources.AccountSummaries;
using Write.Infra.Repositories;

namespace Read.Infra.Tests.DataSources;

public sealed class RepositoryAccountSummariesDataSourceTests : HostFixture
{
    private readonly RepositoryAccountSummariesDataSource sut;
    private readonly InMemoryBankRepository bankRepository;
    private readonly InMemoryAccountRepository accountRepository;

    public RepositoryAccountSummariesDataSourceTests()
    {
        this.sut = this.Resolve<IAccountSummariesDataSource, RepositoryAccountSummariesDataSource>();
        this.bankRepository = this.Resolve<IBankRepository, InMemoryBankRepository>();
        this.accountRepository = this.Resolve<IAccountRepository, InMemoryAccountRepository>();

        this.bankRepository.Clear();
        this.accountRepository.Clear();
    }

    protected override void Configure(IServiceCollection services) =>
        services.AddWriteDependencies().AddReadDependencies();

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
}