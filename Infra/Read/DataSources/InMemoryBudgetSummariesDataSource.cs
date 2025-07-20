using App.Read.Ports;
using App.Shared.Ports;
using Infra.Write.Repositories;

namespace Infra.Read.DataSources;

public class InMemoryBudgetSummariesDataSource(InMemoryBudgetRepository repository, IClock clock) : IBudgetSummariesDataSource
{
    public Task<BudgetSummaryPresentation[]> All()
    {
        DateOnly today = clock.Today;
        BudgetSummaryPresentation[] presentations =
        [
            ..repository.Data.Select(c => new BudgetSummaryPresentation(
                    c.Id.Value,
                    c.Name,
                    c.Amount,
                    c.BeginDate,
                    c.Amount * MonthDifferenceBetween(today, c.BeginDate)
                )
            )
        ];

        return Task.FromResult(presentations);
    }

    private static int MonthDifferenceBetween(DateOnly today, DateOnly beginDate)
    {
        DateTime todayMonth = new(today.Year, today.Month, 1);
        DateTime beginDateMonth = new(beginDate.Year, beginDate.Month, 1);

        if (todayMonth < beginDateMonth)
            return 0;

        int difference = 0;
        while (beginDateMonth <= todayMonth)
        {
            difference++;
            beginDateMonth = beginDateMonth.AddMonths(1);
        }

        return difference;
    }
}