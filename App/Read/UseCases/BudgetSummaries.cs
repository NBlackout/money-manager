using App.Read.Ports;

namespace App.Read.UseCases;

public class BudgetSummaries(IBudgetSummariesDataSource dataSource)
{
    public async Task<BudgetSummaryPresentation[]> Execute() =>
        await dataSource.All();
}