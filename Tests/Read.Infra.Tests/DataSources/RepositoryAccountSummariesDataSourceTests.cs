using Microsoft.Extensions.DependencyInjection;
using Read.Infra.DataSources.AccountSummaries;
using Write.App.Model.Banks;
using Write.Infra.Repositories;
using static Shared.TestTooling.Randomizer;

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
    public async Task Should_retrieve_summaries(Bank aBank, Bank anotherBank)
    {
        this.bankRepository.Feed(aBank, anotherBank);

        AccountBuilder anAccount = Any<AccountBuilder>() with { BankId = aBank.Id };
        AccountBuilder anotherAccount = Any<AccountBuilder>() with { BankId = anotherBank.Id };
        this.accountRepository.Feed(anAccount.Build(), anotherAccount.Build());

        AccountSummaryPresentation[] actual = await this.sut.Get();
        actual.Should().Equal(anAccount.ToSummary(), anotherAccount.ToSummary());
    }
}
