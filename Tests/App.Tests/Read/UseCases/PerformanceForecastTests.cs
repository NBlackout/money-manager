using App.Read.Ports;
using App.Read.UseCases;
using App.Tests.Read.TestDoubles;

namespace App.Tests.Read.UseCases;

public class PerformanceForecastTests
{
    private readonly StubbedPerformanceForecastDataSource dataSource = new();
    private readonly PerformanceForecast sut;

    public PerformanceForecastTests() =>
        this.sut = new PerformanceForecast(this.dataSource);

    [Theory]
    [RandomData]
    public async Task Gives_category_summaries(PerformanceForecastPresentation expected)
    {
        this.Feed(expected);
        await this.Verify(expected);
    }

    private async Task Verify(PerformanceForecastPresentation expected)
    {
        PerformanceForecastPresentation actual = await this.sut.Execute();
        actual.Should().Be(expected);
    }

    private void Feed(PerformanceForecastPresentation expected) =>
        this.dataSource.Feed(expected);
}