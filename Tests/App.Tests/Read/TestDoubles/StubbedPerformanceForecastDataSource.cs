#region

using App.Read.Ports;

#endregion

namespace App.Tests.Read.TestDoubles;

public class StubbedPerformanceForecastDataSource : IPerformanceForecastDataSource
{
    private PerformanceForecastPresentation data = null!;

    public Task<PerformanceForecastPresentation> Fetch()
    {
        return Task.FromResult(this.data);
    }

    public void Feed(PerformanceForecastPresentation forecast)
    {
        this.data = forecast;
    }
}