namespace Read.App.Ports;

public interface IBudgetSummariesDataSource
{
    Task<BudgetSummaryPresentation[]> All();
}
