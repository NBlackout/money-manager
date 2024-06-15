using Microsoft.Extensions.DependencyInjection;
using Read.Infra.DataSources.AccountSummaries;
using Write.App.Model.Banks;
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

    [Theory, RandomData]
    public async Task Should_retrieve_tracked_account_summaries(Bank aBank, Bank anotherBank)
    {
        this.bankRepository.Feed(aBank, anotherBank);

        AccountBuilder checking = AccountBuilder.Create() with { BankId = aBank.Id, Tracked = true };
        AccountBuilder saving = AccountBuilder.Create() with { BankId = aBank.Id, Tracked = true };
        AccountBuilder notTracked = AccountBuilder.Create() with { BankId = anotherBank.Id, Tracked = false };
        this.accountRepository.Feed(checking.Build(), saving.Build(), notTracked.Build());

        AccountSummaryPresentation[] actual = await this.sut.Get();
        actual.Should().Equal(checking.ToSummary(), saving.ToSummary(), notTracked.ToSummary());
    }
}