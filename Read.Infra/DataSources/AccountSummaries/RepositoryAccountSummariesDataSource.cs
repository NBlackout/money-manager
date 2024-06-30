using Write.Infra.Repositories;

namespace Read.Infra.DataSources.AccountSummaries;

public class RepositoryAccountSummariesDataSource(InMemoryAccountRepository accountRepository)
    : IAccountSummariesDataSource
{
    public Task<AccountSummaryPresentation[]> All()
    {
        AccountSummaryPresentation[] summaries = accountRepository.Data.Select(ToSummary).ToArray();

        return Task.FromResult(summaries);
    }

    private static AccountSummaryPresentation ToSummary(AccountSnapshot account)
    {
        return new AccountSummaryPresentation(account.Id, account.Label, account.Number,
            account.Balance, account.BalanceDate);
    }
}
