﻿namespace Read.Infra.DataSources;

public class InMemoryAccountSummariesDataSource(InMemoryAccountRepository accountRepository)
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