using App.Read.Ports;
using App.Shared;

namespace App.Read.UseCases.Dashboard;

public class MonthlyPerformance(IPeriodProvider periodProvider, IPeriodPerformanceDataSource dataSource)
{
    public async Task<PeriodPerformancePresentation[]> Execute()
    {
        Period[] rollingTwelveMonths = await periodProvider.RollingTwelveMonths();

        return await dataSource.All(rollingTwelveMonths);
    }
}