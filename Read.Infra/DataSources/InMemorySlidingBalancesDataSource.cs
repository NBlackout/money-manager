using Write.App.Model.Transactions;

namespace Read.Infra.DataSources;

public class InMemorySlidingBalancesDataSource(
    InMemoryAccountRepository accountRepository,
    InMemoryTransactionRepository transactionRepository
) : ISlidingBalancesDataSource
{
    public Task<SlidingBalancesPresentation> All(DateOnly baseline, DateOnly startingFrom)
    {
        AccountSnapshot? account = accountRepository.Data.SingleOrDefault();
        if (account == null)
            return Task.FromResult(new SlidingBalancesPresentation());

        return Task.FromResult(this.SlidingBalancesOf(account, baseline, startingFrom));
    }

    private SlidingBalancesPresentation SlidingBalancesOf(AccountSnapshot account, DateOnly baseline, DateOnly startingFrom)
    {
        TransactionSnapshot[] transactions = transactionRepository.Data.ToArray();
        List<SlidingBalancePresentation> slidingBalances = [];

        DateOnly beginningOfThisMonth = baseline;
        decimal balanceOfMonth = account.BalanceAmount;
        for (DateOnly startOfMonth = baseline; startOfMonth >= startingFrom; startOfMonth = startOfMonth.AddMonths(-1))
        {
            balanceOfMonth -= TotalAmountOf(transactions, beginningOfThisMonth);

            slidingBalances.Add(PresentationFrom(account, beginningOfThisMonth, balanceOfMonth));
            beginningOfThisMonth = beginningOfThisMonth.AddMonths(-1);
        }

        return new SlidingBalancesPresentation(slidingBalances.OrderBy(d => d.BalanceDate).ToArray());
    }

    private static decimal TotalAmountOf(TransactionSnapshot[] transactions, DateOnly month) =>
        transactions.Where(t => t.Date.Month == month.Month && t.Date.Year == month.Year).Sum(t => t.Amount);

    private static SlidingBalancePresentation PresentationFrom(
        AccountSnapshot account,
        DateOnly month,
        decimal balance) =>
        new(month, new AccountBalancePresentation(account.Label, balance));
}