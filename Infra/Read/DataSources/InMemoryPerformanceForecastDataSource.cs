using App.Read.Ports;
using App.Write.Model.RecurringTransactions;
using Infra.Write.Repositories;

namespace Infra.Read.DataSources;

public class InMemoryPerformanceForecastDataSource(
    InMemoryAccountRepository accountRepository,
    InMemoryRecurringTransactionRepository recurringTransactionRepository
) : IPerformanceForecastDataSource
{
    public Task<PerformanceForecastPresentation> Fetch()
    {
        decimal totalBalance = accountRepository.Data.Sum(a => a.Balance);

        RecurringTransactionSnapshot[] recurringTransactions = [..recurringTransactionRepository.Data];
        PerformancePresentation performance = recurringTransactions.Aggregate(
            new PerformancePresentation(0, 0, 0),
            (p, t) => new PerformancePresentation(p.Revenue + Math.Max(t.Amount, 0), p.Expenses + Math.Min(t.Amount, 0), p.Net + t.Amount)
        );

        return Task.FromResult(new PerformanceForecastPresentation(totalBalance, performance.Revenue, performance.Expenses, performance.Net));
    }
}