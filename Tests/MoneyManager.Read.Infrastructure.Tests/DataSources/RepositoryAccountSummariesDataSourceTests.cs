﻿using MoneyManager.Read.Infrastructure.DataSources.AccountSummaries;
using MoneyManager.Write.Api.Extensions;
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
        AccountBuilder notTracked = AccountBuilder.For(Guid.NewGuid()) with
        {
            BankId = anotherBank.Id, Label = "This one is not tracked", Balance = 1301.51m, Tracked = false
        };
        this.bankRepository.Feed(aBank.Build(), anotherBank.Build());
        this.accountRepository.Feed(checking.Build(), saving.Build(), notTracked.Build());

        IReadOnlyCollection<AccountSummaryPresentation> actual = await this.sut.Get();
        actual.Should().Equal(
            PresentationFrom(checking, aBank),
            PresentationFrom(saving, aBank),
            PresentationFrom(notTracked, anotherBank)
        );
    }

    public void Dispose() =>
        this.host.Dispose();

    private static AccountSummaryPresentation PresentationFrom(AccountBuilder account, BankBuilder bank) =>
        new(account.Id, bank.Id, bank.Name, account.Label, account.Balance, account.BalanceDate, account.Tracked);
}