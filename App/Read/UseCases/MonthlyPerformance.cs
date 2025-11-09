using App.Read.Ports;
using App.Shared;

namespace App.Read.UseCases;

public class MonthlyPerformance(IDateRangeProvider dateRangeProvider, IPeriodPerformanceDataSource dataSource)
{
    public async Task<PeriodPerformancePresentation[]> Execute()
    {
        DateRange[] rollingTwelveMonths = await dateRangeProvider.RollingTwelveMonths();

        return await dataSource.All(rollingTwelveMonths);
    }
}