using App.Read.Ports;

namespace App.Tests.Read.TestDoubles;

public class StubbedPerformanceForecastDataSource : IPerformanceForecastDataSource
{
    private PerformanceForecastPresentation data = null!;

    public Task<PerformanceForecastPresentation> Fetch() =>
        Task.FromResult(this.data);

    public void Feed(PerformanceForecastPresentation forecast) =>
        this.data = forecast;
}