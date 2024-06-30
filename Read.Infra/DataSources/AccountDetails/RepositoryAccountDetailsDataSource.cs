using Write.Infra.Repositories;

namespace Read.Infra.DataSources.AccountDetails;

public class RepositoryAccountDetailsDataSource(InMemoryAccountRepository accountRepository) : IAccountDetailsDataSource
{
    public async Task<AccountDetailsPresentation> Get(Guid id)
    {
        Account account = await accountRepository.By(id);

        return new AccountDetailsPresentation(id, account.Snapshot.Label, account.Snapshot.Number,
            account.Snapshot.Balance, account.Snapshot.BalanceDate);
    }
}
