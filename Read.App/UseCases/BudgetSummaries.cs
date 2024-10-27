namespace Read.App.UseCases;

public class BudgetSummaries(IBudgetSummariesDataSource dataSource)
{
    public async Task<BudgetSummaryPresentation[]> Execute() =>
        await dataSource.All();
}