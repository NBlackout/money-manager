using Write.App.Model.Transactions;

namespace Read.Infra.DataSources;

public class InMemorySlidingAccountBalancesDataSource(
    InMemoryAccountRepository accountRepository,
    InMemoryTransactionRepository transactionRepository
) : ISlidingAccountBalancesDataSource
{
    public Task<SlidingAccountBalancesPresentation> All()
    {
        AccountSnapshot? account = accountRepository.Data.SingleOrDefault();
        if (account == null)
            return Task.FromResult(new SlidingAccountBalancesPresentation());

        TransactionSnapshot? transaction = transactionRepository.Data.SingleOrDefault();

        return Task.FromResult(
            new SlidingAccountBalancesPresentation(
                new AccountBalancesByDatePresentation(
                    account.BalanceDate,
                    new AccountBalancePresentation(account.Label, account.BalanceAmount - transaction?.Amount ?? 0)
                )
            )
        );
    }
}