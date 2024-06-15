using Write.Infra.Repositories;

namespace Read.Infra.DataSources.AccountSummaries;

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

    public Task<AccountSummaryPresentation[]> Get()
    {
        AccountSummaryPresentation[] summaries = this.accountRepository.Data
            .Select(account => ToSummary(this.bankRepository.Data.Single(b => b.Id == account.BankId), account))
            .ToArray();

        return Task.FromResult(summaries);
    }

    private static AccountSummaryPresentation ToSummary(BankSnapshot bank, AccountSnapshot account)
    {
        return new AccountSummaryPresentation(account.Id, bank.Id, account.Label, account.Number,
            account.Balance, account.BalanceDate, account.Tracked);
    }
}