using App.Read.Ports;
using App.Read.UseCases;
using App.Shared;
using App.Tests.Read.TestDoubles;

namespace App.Tests.Read.UseCases;

public class MonthlyPerformanceTests
{
    private readonly StubbedDateRangeProvider dateRangeProvider = new();
    private readonly StubbedPeriodPerformanceDataSource dataSource = new();
    private readonly MonthlyPerformance sut;

    public MonthlyPerformanceTests() =>
        this.sut = new MonthlyPerformance(this.dateRangeProvider, this.dataSource);

    [Theory, RandomData]
    public async Task Gives_performance_of_rolling_twelve_months(DateRange[] dateRanges, PeriodPerformancePresentation[] expected)
    {
        this.RollingTwelveMonthsAre(dateRanges);
        this.MonthlyPerformanceIs(dateRanges, expected);
        await this.Verify(expected);
    }

    private async Task Verify(PeriodPerformancePresentation[] expected)
    {
        PeriodPerformancePresentation[] actual = await this.sut.Execute();
        actual.Should().Equal(expected);
    }

    private void RollingTwelveMonthsAre(DateRange[] dateRanges) =>
        this.dateRangeProvider.Feed( dateRanges);

    private void MonthlyPerformanceIs(DateRange[] dateRanges, PeriodPerformancePresentation[] expected) =>
        this.dataSource.Feed(dateRanges, expected);
}

public class StubbedDateRangeProvider : IDateRangeProvider
{
    private DateRange[] data = [];

    public Task<DateRange[]> RollingTwelveMonths() =>
        Task.FromResult(this.data);

    public void Feed(DateRange[] dateRanges) =>
        this.data = dateRanges;
}