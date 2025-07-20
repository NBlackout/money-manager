namespace Read.App.Ports;

public interface IBudgetSummariesDataSource
{
    Task<BudgetSummaryPresentation[]> All();
}

public record BudgetSummaryPresentation(Guid Id, string Name, decimal Amount, DateOnly BeginDate, decimal TotalAmount);