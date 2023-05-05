using MoneyManager.Write.Application.Ports;
using MoneyManager.Read.Infrastructure.DataSources.AccountSummaries;
using MoneyManager.Shared.Presentation;
using MoneyManager.Write.Infrastructure.Repositories;

namespace MoneyManager.Read.Infrastructure.Tests;

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
        this.sut = this.host.GetRequiredService<IAccountSummariesDataSource, RepositoryAccountSummariesDataSource>();
        this.bankRepository = this.host.GetRequiredService<IBankRepository, InMemoryBankRepository>();
        this.accountRepository = this.host.GetRequiredService<IAccountRepository, InMemoryAccountRepository>();

        this.bankRepository.Clear();
        this.accountRepository.Clear();
    }

    [Fact]
    public async Task Should_retrieve_tracked_account_summaries()
    {
        BankBuilder aBank = BankBuilder.For(Guid.NewGuid()) with { Name = "This is my bank" };
        BankBuilder anotherBank = BankBuilder.For(Guid.NewGuid()) with { Name = "Not my bank" };
        AccountBuilder checking = AccountBuilder.For(Guid.NewGuid()) with
        {
            BankId = aBank.Id, Label = "Checking account", Balance = 10.44m, Tracked = true
        };
        AccountBuilder saving = AccountBuilder.For(Guid.NewGuid()) with
        {
            BankId = aBank.Id, Label = "My savings", Balance = 656.98m, Tracked = true
        };
        AccountBuilder unknown = AccountBuilder.For(Guid.NewGuid()) with
        {
            BankId = anotherBank.Id, Label = "This one is not tracked", Balance = 1301.51m, Tracked = false
        };
        this.bankRepository.Feed(aBank.Build(), anotherBank.Build());
        this.accountRepository.Feed(checking.Build(), saving.Build(), unknown.Build());

        IReadOnlyCollection<AccountSummary> actual = await this.sut.Get();
        actual.Should().Equal(new AccountSummary[]
        {
            new(checking.Id, aBank.Id, aBank.Name, checking.Label, checking.Balance, checking.Tracked),
            new(saving.Id, aBank.Id, aBank.Name, saving.Label, saving.Balance, saving.Tracked),
            new(unknown.Id, anotherBank.Id, anotherBank.Name, unknown.Label, unknown.Balance, unknown.Tracked)
        });
    }

    public void Dispose() =>
        this.host.Dispose();
}