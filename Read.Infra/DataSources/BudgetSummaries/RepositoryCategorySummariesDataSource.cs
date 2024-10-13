using Shared.Ports;
using Write.Infra.Repositories;

namespace Read.Infra.DataSources.BudgetSummaries;

public class RepositoryBudgetSummariesDataSource(
    InMemoryBudgetRepository repository,
    IDateOnlyProvider dateOnlyProvider) : IBudgetSummariesDataSource
{
    public Task<BudgetSummaryPresentation[]> All()
    {
        DateOnly today = dateOnlyProvider.Today;
        BudgetSummaryPresentation[] presentations =
            repository.Data.Select(c => new BudgetSummaryPresentation(c.Id, c.Name, c.Amount, c.BeginDate,
                    c.Amount * MonthDifferenceBetween(today, c.BeginDate)))
                .ToArray();

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