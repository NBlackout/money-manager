using MoneyManager.Write.Infrastructure.Repositories;

namespace MoneyManager.Read.Infrastructure.DataSources;

public class RepositoryAccountDetailsDataSource : IAccountDetailsDataSource
{
    private readonly InMemoryAccountRepository repository;

    public RepositoryAccountDetailsDataSource(InMemoryAccountRepository repository)
    {
        this.repository = repository;
    }

    public async Task<AccountDetailsPresentation> Get(Guid id)
    {
        Account account = await this.repository.ById(id);

        return new AccountDetailsPresentation(account.Id, account.Snapshot.Label, account.Snapshot.Balance);
    }
}