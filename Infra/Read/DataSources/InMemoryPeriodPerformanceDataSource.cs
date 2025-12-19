using App.Read.Ports;
using App.Shared;
using App.Write.Model.Accounts;
using App.Write.Model.Transactions;
using Infra.Write.Repositories;

namespace Infra.Read.DataSources;

public class InMemoryPeriodPerformanceDataSource(InMemoryAccountRepository accountRepository, InMemoryTransactionRepository transactionRepository)
    : IPeriodPerformanceDataSource
{
    public Task<PeriodPerformancePresentation[]> All(Period[] dateRanges)
    {
        AccountSnapshot[] accounts = accountRepository.Data.ToArray();
        if (accounts.Length == 0)
            return Task.FromResult(Array.Empty<PeriodPerformancePresentation>());

        return Task.FromResult(this.PerformanceOf(accounts, dateRanges));
    }

    private PeriodPerformancePresentation[] PerformanceOf(AccountSnapshot[] accounts, Period[] dateRanges)
    {
        List<PeriodPerformancePresentation> periods = [];

        DateOnly beginningOfThisMonth = dateRanges.Last().From;
        decimal balancesOfMonth = accounts.Sum(a => a.Balance); // TODO depends on account BalanceDate

        foreach (Period period in dateRanges.Reverse())
        {
            TransactionSnapshot[] transactions = this.TransactionOf(period);

            PerformancePresentation performance = transactions.Aggregate(
                new PerformancePresentation(0, 0, 0),
                (p, t) => new PerformancePresentation(p.Revenue + Math.Max(t.Amount, 0), p.Expenses + Math.Min(t.Amount, 0), p.Net + t.Amount)
            );
            balancesOfMonth -= performance.Net;

            periods.Add(new PeriodPerformancePresentation(period, balancesOfMonth, performance));

            beginningOfThisMonth = beginningOfThisMonth.AddMonths(-1);
        }

        return periods.ToArray().Reverse().ToArray();
    }

    private TransactionSnapshot[] TransactionOf(Period period) =>
        transactionRepository.Data.Where(t => t.Date >= period.From && t.Date <= period.To).ToArray();
}