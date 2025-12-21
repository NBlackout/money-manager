using App.Read.Ports;
using App.Shared;
using App.Shared.Ports;

namespace Infra.Read.DataSources;

public class InMemoryBalanceForecastsDataSource(IClock clock) : IBalanceForecastsDataSource
{
    public Task<BalanceForecastPresentation[]> Fetch(decimal balance, SamplePeriod[] samplePeriods, ForecastPeriod[] forecastPeriods)
    {
        decimal average = AverageDuring(samplePeriods);

        BalanceForecastPresentation endOfMonthForecast = new(clock.LastDayOfMonth, balance + average);
        List<BalanceForecastPresentation> forecasts = [endOfMonthForecast];
        foreach ((Period period, decimal net) in forecastPeriods)
            forecasts.Add(new BalanceForecastPresentation(period.To, forecasts.Last().Balance + net + average));

        return Task.FromResult(forecasts.ToArray());
    }

    private static decimal AverageDuring(SamplePeriod[] samplePeriods)
    {
        if (samplePeriods.Length == 0)
            return 0;

        return Math.Round(samplePeriods.Sum(t => t.Amount) / samplePeriods.Length, 2);
    }
}