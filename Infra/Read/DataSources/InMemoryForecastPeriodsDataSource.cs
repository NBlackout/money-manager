using App.Read.Ports;
using App.Shared;
using Infra.Write.Repositories;

namespace Infra.Read.DataSources;

public class InMemoryForecastPeriodsDataSource(InMemoryRecurringTransactionRepository recurringTransactionRepository) : IForecastPeriodsDataSource
{
    public Task<ForecastPeriod[]> Of(Period[] periods)
    {
        decimal recurringAmount = recurringTransactionRepository.Data.Sum(t => t.Amount);
        ForecastPeriod[] forecastPeriods = periods.Select(p => new ForecastPeriod(p, recurringAmount)).ToArray();

        return Task.FromResult(forecastPeriods);
    }
}