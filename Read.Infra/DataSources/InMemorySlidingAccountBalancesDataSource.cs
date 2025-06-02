using Write.App.Model.Transactions;

namespace Read.Infra.DataSources;

// Use case gives minimum date or month range (like Period.OneYear or Period.TwelveMonth)
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

        TransactionSnapshot[] transactions =
            transactionRepository.Data.Where(t => t.Date > account.BalanceDate.AddYears(-1)).ToArray();
        // if (transactions.Length == 0)
        //     return JustBalanceOf(account);

        return Task.FromResult(BalanceAtTheBeginningOfMonthOf(account, transactions));
    }

    private static SlidingAccountBalancesPresentation BalanceAtTheBeginningOfMonthOf(
        AccountSnapshot account,
        TransactionSnapshot[] transactions)
    {
        DateOnly beginningOfThisMonth = new(account.BalanceDate.Year, account.BalanceDate.Month, 1);

        List<AccountBalancesByDatePresentation> yoy = [];
        decimal balanceOfMonth = account.BalanceAmount -
            transactions
                .Where(t => t.Date.Month == beginningOfThisMonth.Month && t.Date.Year == beginningOfThisMonth.Year)
                .Sum(t => t.Amount);
        if (true)
        {
            yoy.Add(
                new AccountBalancesByDatePresentation(
                    beginningOfThisMonth,
                    new AccountBalancePresentation(account.Label, balanceOfMonth)
                )
            );
            beginningOfThisMonth = beginningOfThisMonth.AddMonths(-1);
        }

        if (true)
        {
            yoy.Add(Tutu2(account, transactions, beginningOfThisMonth, ref balanceOfMonth));
            beginningOfThisMonth = beginningOfThisMonth.AddMonths(-1);
        }

        if (true)
        {
            yoy.Add(Tutu2(account, transactions, beginningOfThisMonth, ref balanceOfMonth));
            beginningOfThisMonth = beginningOfThisMonth.AddMonths(-1);
        }

        if (true)
        {
            yoy.Add(Tutu2(account, transactions, beginningOfThisMonth, ref balanceOfMonth));
            beginningOfThisMonth = beginningOfThisMonth.AddMonths(-1);
        }

        if (true)
        {
            yoy.Add(Tutu2(account, transactions, beginningOfThisMonth, ref balanceOfMonth));
            beginningOfThisMonth = beginningOfThisMonth.AddMonths(-1);
        }

        if (true)
        {
            yoy.Add(Tutu2(account, transactions, beginningOfThisMonth, ref balanceOfMonth));
            beginningOfThisMonth = beginningOfThisMonth.AddMonths(-1);
        }

        if (true)
        {
            yoy.Add(Tutu2(account, transactions, beginningOfThisMonth, ref balanceOfMonth));
            beginningOfThisMonth = beginningOfThisMonth.AddMonths(-1);
        }

        if (true)
        {
            yoy.Add(Tutu2(account, transactions, beginningOfThisMonth, ref balanceOfMonth));
            beginningOfThisMonth = beginningOfThisMonth.AddMonths(-1);
        }

        if (true)
        {
            yoy.Add(Tutu2(account, transactions, beginningOfThisMonth, ref balanceOfMonth));
            beginningOfThisMonth = beginningOfThisMonth.AddMonths(-1);
        }

        if (true)
        {
            yoy.Add(Tutu2(account, transactions, beginningOfThisMonth, ref balanceOfMonth));
            beginningOfThisMonth = beginningOfThisMonth.AddMonths(-1);
        }

        if (true)
        {
            yoy.Add(Tutu2(account, transactions, beginningOfThisMonth, ref balanceOfMonth));
            beginningOfThisMonth = beginningOfThisMonth.AddMonths(-1);
        }

        if (true)
        {
            yoy.Add(Tutu2(account, transactions, beginningOfThisMonth, ref balanceOfMonth));
            beginningOfThisMonth = beginningOfThisMonth.AddMonths(-1);
        }

        return new SlidingAccountBalancesPresentation(yoy.ToArray());
    }

    private static AccountBalancesByDatePresentation Tutu2(
        AccountSnapshot account,
        TransactionSnapshot[] transactions,
        DateOnly beginningOfThisMonth,
        ref decimal balanceOfMonth)
    {
        balanceOfMonth -= transactions
            .Where(t => t.Date.Month == beginningOfThisMonth.Month && t.Date.Year == beginningOfThisMonth.Year)
            .Sum(t => t.Amount);

        AccountBalancesByDatePresentation tutu = new AccountBalancesByDatePresentation(
            beginningOfThisMonth,
            new AccountBalancePresentation(account.Label, balanceOfMonth)
        );
        return tutu;
    }

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