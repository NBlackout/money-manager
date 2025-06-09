using Write.App.Model.Transactions;

namespace Read.Infra.DataSources;

public class InMemorySlidingBalancesDataSource(
    InMemoryAccountRepository accountRepository,
    InMemoryTransactionRepository transactionRepository
) : ISlidingBalancesDataSource
{
    public Task<SlidingBalancesPresentation> All(DateOnly baseline, DateOnly startingFrom)
    {
        AccountSnapshot[] accounts = accountRepository.Data.ToArray();
        if (accounts.Length == 0)
            return Task.FromResult(new SlidingBalancesPresentation());

        return Task.FromResult(this.SlidingBalancesOf(accounts, baseline, startingFrom));
    }

    private SlidingBalancesPresentation SlidingBalancesOf(
        AccountSnapshot[] accounts,
        DateOnly baseline,
        DateOnly startingFrom)
    {
        TransactionSnapshot[] transactions = transactionRepository.Data.ToArray();
        List<SlidingBalancePresentation> slidingBalances = [];

        DateOnly beginningOfThisMonth = baseline;
        Dictionary<string, decimal> balancesOfMonth = accounts.ToDictionary(a => a.Label, a => a.Balance);
        for (DateOnly startOfMonth = baseline; startOfMonth >= startingFrom; startOfMonth = startOfMonth.AddMonths(-1))
        {
            foreach (AccountSnapshot account in accounts)
                balancesOfMonth[account.Label] -= TotalAmountOf(account, transactions, beginningOfThisMonth);

            slidingBalances.Add(PresentationsFrom(beginningOfThisMonth, balancesOfMonth));
            beginningOfThisMonth = beginningOfThisMonth.AddMonths(-1);
        }

        return new SlidingBalancesPresentation(slidingBalances.OrderBy(d => d.BalanceDate).ToArray());
    }

    private static decimal TotalAmountOf(AccountSnapshot account, TransactionSnapshot[] transactions, DateOnly month) =>
        transactions
            .Where(t => t.AccountId == account.Id && t.Date.Month == month.Month && t.Date.Year == month.Year)
            .Sum(t => t.Amount);

    private static SlidingBalancePresentation PresentationsFrom(
        DateOnly balanceDate,
        Dictionary<string, decimal> balances) =>
        new(balanceDate, balances.Select(bom => new AccountBalancePresentation(bom.Key, bom.Value)).ToArray());
}