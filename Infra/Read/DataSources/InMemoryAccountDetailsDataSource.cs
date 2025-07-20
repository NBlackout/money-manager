using App.Read.Ports;
using App.Write.Model.Accounts;
using Infra.Write.Repositories;

namespace Infra.Read.DataSources;

public class InMemoryAccountDetailsDataSource(InMemoryAccountRepository accountRepository) : IAccountDetailsDataSource
{
    public async Task<AccountDetailsPresentation> By(Guid id)
    {
        Account account = await accountRepository.By(new AccountId(id));

        return new AccountDetailsPresentation(id, account.Snapshot.Label, account.Snapshot.Number, account.Snapshot.Balance, account.Snapshot.BalanceDate);
    }
}