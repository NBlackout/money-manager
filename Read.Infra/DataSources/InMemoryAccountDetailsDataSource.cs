namespace Read.Infra.DataSources;

public class InMemoryAccountDetailsDataSource(InMemoryAccountRepository accountRepository) : IAccountDetailsDataSource
{
    public async Task<AccountDetailsPresentation> By(Guid id)
    {
        Account account = await accountRepository.By(new AccountId(id));

        return new AccountDetailsPresentation(id, account.Snapshot.Label, account.Snapshot.Number, account.Snapshot.Balance, account.Snapshot.BalanceDate);
    }
}