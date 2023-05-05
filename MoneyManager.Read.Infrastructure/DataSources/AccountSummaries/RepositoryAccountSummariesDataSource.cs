using MoneyManager.Write.Application.Model;
using MoneyManager.Write.Infrastructure.Repositories;

namespace MoneyManager.Read.Infrastructure.DataSources.AccountSummaries;

public class RepositoryAccountSummariesDataSource : IAccountSummariesDataSource
{
    private readonly InMemoryBankRepository bankRepository;
    private readonly InMemoryAccountRepository accountRepository;

    public RepositoryAccountSummariesDataSource(InMemoryBankRepository bankRepository,
        InMemoryAccountRepository accountRepository)
    {
        this.bankRepository = bankRepository;
        this.accountRepository = accountRepository;
    }

    public Task<IReadOnlyCollection<AccountSummaryPresentation>> Get()
    {
        IReadOnlyCollection<AccountSummaryPresentation> summaries = this.accountRepository.Data
            .Select(account => ToSummary(this.bankRepository.Data[account.Snapshot.BankId], account))
            .ToList();

        return Task.FromResult(summaries);
    }

    private static AccountSummaryPresentation ToSummary(Bank bank, Account account)
    {
        return new AccountSummaryPresentation(account.Id, bank.Id, bank.Snapshot.Name, account.Snapshot.Label,
            account.Snapshot.Balance, account.Snapshot.Tracked);
    }
}