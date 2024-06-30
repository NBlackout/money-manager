namespace Read.Infra.DataSources.BudgetSummaries;

public class StubbedBudgetSummariesDataSource : IBudgetSummariesDataSource
{
    private BudgetSummaryPresentation[] data = null!;

    public Task<BudgetSummaryPresentation[]> All() =>
        Task.FromResult(this.data);

    public void Feed(BudgetSummaryPresentation[] summaries) =>
        this.data = summaries;
}