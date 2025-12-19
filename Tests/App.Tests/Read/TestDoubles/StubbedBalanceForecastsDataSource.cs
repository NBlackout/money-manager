using App.Read.Ports;

namespace App.Tests.Read.TestDoubles;

public class StubbedBalanceForecastsDataSource : IBalanceForecastsDataSource
{
    private BalanceForecastPresentation[] data = null!;

    public Task<BalanceForecastPresentation[]> Fetch() =>
        Task.FromResult(this.data);

    public void Feed(BalanceForecastPresentation[] forecasts) =>
        this.data = forecasts;
}