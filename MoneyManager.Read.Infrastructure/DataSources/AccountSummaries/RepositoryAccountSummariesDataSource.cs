﻿using MoneyManager.Write.Application.Model;
using MoneyManager.Write.Infrastructure.Repositories;

namespace MoneyManager.Read.Infrastructure.DataSources.AccountSummaries;

public class RepositoryAccountSummariesDataSource : IAccountSummariesDataSource
{
    private readonly InMemoryAccountRepository repository;

    public RepositoryAccountSummariesDataSource(InMemoryAccountRepository repository)
    {
        this.repository = repository;
    }

    public Task<IReadOnlyCollection<AccountSummary>> Get()
    {
        IReadOnlyCollection<AccountSummary> summaries = this.repository.Data.Select(ToSummary).ToList();

        return Task.FromResult(summaries);
    }

    private static AccountSummary ToSummary(Account account) =>
        new(account.Id, account.Snapshot.Label, account.Snapshot.Balance, account.Snapshot.Tracked);
}