using System.Text.Json;
using App.Read.Ports;
using App.Shared;

namespace App.Tests.Read.TestDoubles;

public class StubbedForecastPeriodsDataSource : IForecastPeriodsDataSource
{
    private readonly Dictionary<string, ForecastPeriod[]> data = [];

    public Task<ForecastPeriod[]> Of(Period[] periods) =>
        Task.FromResult(this.data[Serialize(periods)]);

    public void Feed(Period[] periods, ForecastPeriod[] forecastPeriods) =>
        this.data[Serialize(periods)] = forecastPeriods;

    private static string Serialize(Period[] periods) =>
        JsonSerializer.Serialize(periods);
}