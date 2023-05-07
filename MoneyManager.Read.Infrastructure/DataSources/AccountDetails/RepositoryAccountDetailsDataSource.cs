using MoneyManager.Write.Infrastructure.Repositories;

namespace MoneyManager.Read.Infrastructure.DataSources.AccountDetails;

public class RepositoryAccountDetailsDataSource : IAccountDetailsDataSource
{
    private readonly InMemoryAccountRepository accountRepository;
    private readonly InMemoryTransactionRepository transactionRepository;

    public RepositoryAccountDetailsDataSource(InMemoryAccountRepository accountRepository,
        InMemoryTransactionRepository transactionRepository)
    {
        this.accountRepository = accountRepository;
        this.transactionRepository = transactionRepository;
    }

    public async Task<AccountDetailsPresentation> Get(Guid id)
    {
        Account account = await this.accountRepository.ById(id);
        TransactionSummary[] transactions = this.transactionRepository.Data.Where(t => t.Snapshot.AccountId == id)
            .Select(t => new TransactionSummary(t.Id)).ToArray();

        return new AccountDetailsPresentation(id, account.Snapshot.Label, account.Snapshot.Balance, transactions);
    }
}