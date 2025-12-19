using App.Read.Ports;
using App.Read.UseCases;
using App.Tests.Read.TestDoubles;

namespace App.Tests.Read.UseCases;

public class BalanceForecastsTests
{
    private readonly StubbedBalanceForecastsDataSource dataSource = new();
    private readonly BalanceForecasts sut;

    public BalanceForecastsTests() =>
        this.sut = new BalanceForecasts(this.dataSource);

    [Theory]
    [RandomData]
    public async Task Gives_category_summaries(BalanceForecastPresentation[] expected)
    {
        this.Feed(expected);
        await this.Verify(expected);
    }

    private async Task Verify(params BalanceForecastPresentation[] expected)
    {
        BalanceForecastPresentation[] actual = await this.sut.Execute();
        actual.Should().Equal(expected);
    }

    private void Feed(params BalanceForecastPresentation[] expected) =>
        this.dataSource.Feed(expected);
}