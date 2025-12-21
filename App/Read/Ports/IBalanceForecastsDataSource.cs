namespace App.Read.Ports;

public interface IBalanceForecastsDataSource
{
    Task<BalanceForecastPresentation[]> Fetch(decimal balance, SamplePeriod[] samplePeriods, ForecastPeriod[] forecastPeriods);
}

public record BalanceForecastPresentation(DateOnly Date, decimal Balance);