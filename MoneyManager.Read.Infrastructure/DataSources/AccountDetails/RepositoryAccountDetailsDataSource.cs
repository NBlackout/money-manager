using MoneyManager.Write.Infrastructure.Repositories;

namespace MoneyManager.Read.Infrastructure.DataSources.AccountDetails;

public class RepositoryAccountDetailsDataSource : IAccountDetailsDataSource
{
    private readonly InMemoryAccountRepository accountRepository;

    public RepositoryAccountDetailsDataSource(InMemoryAccountRepository accountRepository)
    {
        this.accountRepository = accountRepository;
    }

    public async Task<AccountDetailsPresentation> Get(Guid id)
    {
        Account account = await this.accountRepository.ById(id);

        return new AccountDetailsPresentation(id, account.Snapshot.Label, account.Snapshot.Number, account.Snapshot.Balance);
    }
}