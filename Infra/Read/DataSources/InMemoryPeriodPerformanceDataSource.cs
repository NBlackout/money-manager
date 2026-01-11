using App.Read.Ports;
using App.Shared;
using App.Write.Model.Accounts;
using App.Write.Model.Transactions;
using Infra.Shared.Extensions;
using Infra.Write.Repositories;

namespace Infra.Read.DataSources;

public class InMemoryPeriodPerformanceDataSource(InMemoryAccountRepository accountRepository, InMemoryTransactionRepository transactionRepository)
    : IPeriodPerformanceDataSource
{
    public Task<PeriodPerformancePresentation[]> All(Period[] periods)
    {
        AccountSnapshot[] accounts = accountRepository.Data.ToArray();
        if (accounts.Length == 0)
            return Task.FromResult(Array.Empty<PeriodPerformancePresentation>());

        return Task.FromResult(this.PerformancesOf(accounts, periods));
    }

    private PeriodPerformancePresentation[] PerformancesOf(AccountSnapshot[] accounts, Period[] periods)
    {
        List<PeriodPerformancePresentation> performances = [];

        DateOnly beginningOfThisMonth = periods.Last().From;
        decimal balancesOfMonth = accounts.Sum(a => a.Balance); // TODO depends on account BalanceDate

        foreach (Period period in periods.Reverse())
        {
            TransactionSnapshot[] transactions = this.TransactionOf(period);

            PerformancePresentation performance = transactions.Aggregate(
                new PerformancePresentation(0, 0, 0),
                (p, t) => new PerformancePresentation(p.Revenue + Math.Max(t.Amount, 0), p.Expenses + Math.Min(t.Amount, 0), p.Net + t.Amount)
            );
            balancesOfMonth -= performance.Net;

            performances.Add(new PeriodPerformancePresentation(period, balancesOfMonth, performance));

            beginningOfThisMonth = beginningOfThisMonth.AddMonths(-1);
        }

        return performances.ToArray().Reverse().ToArray();
    }

    private TransactionSnapshot[] TransactionOf(Period period) =>
        transactionRepository.Data.Where(t => period.Includes(t.Date)).ToArray();
}