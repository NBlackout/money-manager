using System.Text.Json;
using App.Read.Ports;

namespace App.Tests.Read.TestDoubles;

public class StubbedBalanceForecastsDataSource : IBalanceForecastsDataSource
{
    private readonly Dictionary<string, BalanceForecastPresentation[]> data = [];

    public Task<BalanceForecastPresentation[]> Fetch(decimal balance, SamplePeriod[] samplePeriods, ForecastPeriod[] forecastPeriods) =>
        Task.FromResult(this.data[Serialize(balance, samplePeriods, forecastPeriods)]);

    public void Feed(decimal balance, SamplePeriod[] samplePeriods, ForecastPeriod[] forecastPeriods, BalanceForecastPresentation[] forecasts) =>
        this.data[Serialize(balance, samplePeriods, forecastPeriods)] = forecasts;

    private static string Serialize(decimal balance, SamplePeriod[] samplePeriods, ForecastPeriod[] forecastPeriods) =>
        JsonSerializer.Serialize(new { balance, samplePeriods, forecastPeriods });
}