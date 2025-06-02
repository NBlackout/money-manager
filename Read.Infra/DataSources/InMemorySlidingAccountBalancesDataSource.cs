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

        TransactionSnapshot[] transactions = transactionRepository.Data.ToArray();
        if (transactions.Length == 0)
            return JustBalanceOf(account);

        return Task.FromResult(BalanceAtTheBeginningOfMonthOf(account, transactions));
    }

    private static SlidingAccountBalancesPresentation BalanceAtTheBeginningOfMonthOf(
        AccountSnapshot account,
        TransactionSnapshot[] transactions) =>
        new(
            new AccountBalancesByDatePresentation(
                new DateOnly(account.BalanceDate.Year, account.BalanceDate.Month, 1),
                new AccountBalancePresentation(
                    account.Label,
                    account.BalanceAmount -
                    transactions
                        .Where(t => t.Date.Month == account.BalanceDate.Month && t.Date.Year == account.BalanceDate.Year
                        )
                        .Sum(t => t.Amount)
                )
            )
        );

    private static Task<SlidingAccountBalancesPresentation> JustBalanceOf(AccountSnapshot account) =>
        Task.FromResult(
            new SlidingAccountBalancesPresentation(
                new AccountBalancesByDatePresentation(
                    new DateOnly(account.BalanceDate.Year, account.BalanceDate.Month, 1),
                    new AccountBalancePresentation(account.Label, account.BalanceAmount)
                )
            )
        );
}