using App.Read.Ports;
using App.Write.Model.Accounts;
using Infra.Write.Repositories;

namespace Infra.Read.DataSources;

public class InMemoryAccountSummariesDataSource(InMemoryAccountRepository accountRepository) : IAccountSummariesDataSource
{
    public Task<AccountSummaryPresentation[]> All()
    {
        AccountSummaryPresentation[] summaries = [..accountRepository.Data.Select(ToSummary)];

        return Task.FromResult(summaries);
    }

    private static AccountSummaryPresentation ToSummary(AccountSnapshot account) =>
        new(account.Id.Value, account.Label, account.Number, account.Balance);
}