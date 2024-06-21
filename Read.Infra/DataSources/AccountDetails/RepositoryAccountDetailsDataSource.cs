﻿using Write.Infra.Repositories;

namespace Read.Infra.DataSources.AccountDetails;

public class RepositoryAccountDetailsDataSource : IAccountDetailsDataSource
{
    private readonly InMemoryAccountRepository accountRepository;

    public RepositoryAccountDetailsDataSource(InMemoryAccountRepository accountRepository)
    {
        this.accountRepository = accountRepository;
    }

    public async Task<AccountDetailsPresentation> Get(Guid id)
    {
        Account account = await this.accountRepository.By(id);

        return new AccountDetailsPresentation(id, account.Snapshot.Label, account.Snapshot.Number,
            account.Snapshot.Balance, account.Snapshot.BalanceDate);
    }
}